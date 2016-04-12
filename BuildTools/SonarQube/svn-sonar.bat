echo off
cls

cd /d "%~dp0"

cd

echo Please wait for a while...
echo ----------------------------------------------------- >> sonar-build.log
echo ----------------------------------------------------- >> sonar-build.log

MSBuild.SonarQube.Runner.exe begin /k:SriVaikhanasa.Net /n:SriVaikhanasa.Net /v:1.0 /s:"%~dp0SonarQube.Analysis.xml" >> sonar-build.log

msbuild ..\..\SVN.Web\Svn.Web.csproj >> sonar-build.log

MSBuild.SonarQube.Runner.exe end >> sonar-build.log

pause