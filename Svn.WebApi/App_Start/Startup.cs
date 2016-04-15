using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Facebook;
using Microsoft.Owin.Security.OAuth;
using Owin;
using Svn.Model;
using Svn.Service.UserServices;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Svn.WebApi
{
    public partial class Startup
    {
        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        public static string PublicClientId { get; private set; }

        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void Configuration(IAppBuilder app)
        {
            // Configure the db context and user manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationUserDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseCookieAuthentication(new CookieAuthenticationOptions());
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Configure the application for OAuth based flow
            PublicClientId = "self";
            OAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/Token"),
                Provider = new ApplicationOAuthProvider(PublicClientId),
                AuthorizeEndpointPath = new PathString("/api/Account/ExternalLogin"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(30),
                // In production mode set AllowInsecureHttp = false
                AllowInsecureHttp = true
            };

            // Enable the application to use bearer tokens to authenticate users
            app.UseOAuthBearerTokens(OAuthOptions);

            // Uncomment the following lines to enable logging in with third party login providers
            OAuthConfigSection config = (OAuthConfigSection)ConfigurationManager.GetSection("oAuthConfig");

            #region Microsoft OAuth

            if (config.Providers["Microsoft"] != null)
            {
                var ms = new Microsoft.Owin.Security.MicrosoftAccount.MicrosoftAccountAuthenticationOptions();
                ms.Scope.Add("wl.emails");
                ms.Scope.Add("wl.basic");
                ms.ClientId = config.Providers["Microsoft"].ClientId;
                ms.ClientSecret = config.Providers["Microsoft"].ClientSecret;
                ms.Provider = new Microsoft.Owin.Security.MicrosoftAccount.MicrosoftAccountAuthenticationProvider
                {
                    OnAuthenticated =
                    (context) =>
                    {
                        context.Identity.AddClaim(new System.Security.Claims.Claim
                        ("urn:microsoftaccount:access_token", context.AccessToken));

                        foreach (var claim in context.User)
                        {
                            var claimType = string.Format("urn:microsoftaccount:{0}", claim.Key);
                            string claimValue = claim.Value.ToString();
                            if (!context.Identity.HasClaim(claimType, claimValue))
                                context.Identity.AddClaim(new System.Security.Claims.Claim
                                (claimType, claimValue, "XmlSchemaString", "Microsoft"));
                        }

                        return Task.FromResult(0);
                    }
                };

                app.UseMicrosoftAccountAuthentication(ms);
            }

            #endregion

            #region Facebook OAuth

            if (config.Providers["Facebook"] != null)
            {
                var facebookAuthenticationOptions = new FacebookAuthenticationOptions
                {
                    AppId = config.Providers["Facebook"].ClientId,
                    AppSecret = config.Providers["Facebook"].ClientSecret,
                    Scope = { "email" },
                    Provider = new FacebookAuthenticationProvider
                    {
                        OnAuthenticated = context =>
                        {
                            context.Identity.AddClaim(new System.Security.Claims.Claim("FacebookAccessToken", context.AccessToken));
                            return Task.FromResult(true);
                        }
                    }
                };
                app.UseFacebookAuthentication(facebookAuthenticationOptions);
            }

            #endregion

            #region Google OAuth

            if (config.Providers["Google"] != null)
            {
                app.UseGoogleAuthentication(
                    clientId: config.Providers["Google"].ClientId,
                    clientSecret: config.Providers["Google"].ClientSecret);
            }

            #endregion
        }
    }

    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;

        public ApplicationOAuthProvider(string publicClientId)
        {
            if (publicClientId == null)
            {
                throw new ArgumentNullException("publicClientId");
            }

            _publicClientId = publicClientId;
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();

            ApplicationUser user = await userManager.FindAsync(context.UserName, context.Password);

            if (user == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }

            ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(userManager,
               OAuthDefaults.AuthenticationType);
            ClaimsIdentity cookiesIdentity = await user.GenerateUserIdentityAsync(userManager,
                CookieAuthenticationDefaults.AuthenticationType);

            AuthenticationProperties properties = CreateProperties(user.UserName);
            AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);
            context.Validated(ticket);
            context.Request.Context.Authentication.SignIn(cookiesIdentity);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // Resource owner password credentials does not provide a client ID.
            if (context.ClientId == null)
            {
                context.Validated();
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            if (context.ClientId == _publicClientId)
            {
                Uri expectedRootUri = new Uri(context.Request.Uri, "/");

                if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                {
                    context.Validated();
                }
            }

            return Task.FromResult<object>(null);
        }

        public static AuthenticationProperties CreateProperties(string userName)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userName", userName }
            };
            return new AuthenticationProperties(data);
        }
    }

    public class ChallengeResult : IHttpActionResult
    {
        public ChallengeResult(string loginProvider, ApiController controller)
        {
            LoginProvider = loginProvider;
            Request = controller.Request;
        }

        public string LoginProvider { get; set; }
        public HttpRequestMessage Request { get; set; }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            Request.GetOwinContext().Authentication.Challenge(LoginProvider);

            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            response.RequestMessage = Request;
            return Task.FromResult(response);
        }
    }

    public class OAuthConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("providers", IsRequired = true)]
        [ConfigurationCollection(typeof(OAuthProviderCollection), AddItemName = "add", CollectionType = ConfigurationElementCollectionType.BasicMap)]
        public OAuthProviderCollection Providers
        {
            get { return ((OAuthProviderCollection)(base["providers"])); }
        }
    }

    [ConfigurationCollection(typeof(OAuthProviderElement))]
    public class OAuthProviderCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new OAuthProviderElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((OAuthProviderElement)(element)).Name;
        }

        public new OAuthProviderElement this[string name]
        {
            get { return (OAuthProviderElement)BaseGet(name); }
        }
    }

    public class OAuthProviderElement : ConfigurationElement
    {
        [ConfigurationProperty("providerName", IsKey = true, IsRequired = true)]
        public string Name { get { return base["providerName"] as string; } }

        [ConfigurationProperty("clientId", IsRequired = true)]
        public string ClientId { get { return base["clientId"] as string; } }

        [ConfigurationProperty("clientSecret", IsRequired = true)]
        public string ClientSecret { get { return base["clientSecret"] as string; } }

        [ConfigurationProperty("callbackUrl", IsRequired = false)]
        public string CallbackUrl { get { return base["callbackUrl"] as string; } }
    }
}