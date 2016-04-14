using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Svn.Service
{
    public class CaptchaImage
    {
        public Bitmap Image
        {
            get { return this.image; }
        }

        private string text;
        private readonly int width;
        private readonly int height;
        private readonly System.Drawing.FontFamily fontFamily;
        private Bitmap image;

        private readonly Random random = new Random();

        public CaptchaImage(int width, int height, System.Drawing.FontFamily fontFamily)
        {
            this.width = width;
            this.height = height;
            this.fontFamily = fontFamily;
        }
        public CaptchaImage(string text, int width, int height, System.Drawing.FontFamily fontFamily)
        {
            this.text = text;
            this.width = width;
            this.height = height;
            this.fontFamily = fontFamily;
        }
        public string CreateRandomText(int length)
        {
            string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ1234567890";
            char[] chars = new char[length];
            Random rd = new Random();

            for (int i = 0; i < length; i++)
            {
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }

            return new string(chars);
        }

        public void GenerateImage()
        {
            // Create a new 32-bit bitmap image.
            Bitmap bitmap = new Bitmap(this.width, this.height, PixelFormat.Format32bppArgb);

            // Create a graphics object for drawing.
            Graphics g = Graphics.FromImage(bitmap);
            g.PageUnit = GraphicsUnit.Pixel;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle rect = new Rectangle(0, 0, this.width, this.height);

            // Fill in the background.
            Color bgColor = System.Drawing.ColorTranslator.FromHtml("#d9230f");
            HatchBrush hatchBrush = new HatchBrush(HatchStyle.Shingle, bgColor, bgColor);
            g.FillRectangle(hatchBrush, rect);

            // Set up the text font.
            SizeF size;
            float fontSize = rect.Height + 1;
            Font font;
            // Adjust the font size until the text fits within the image.
            do
            {
                fontSize--;
                font = new Font(this.fontFamily.Name, fontSize, GraphicsUnit.Pixel);
                size = g.MeasureString(this.text, font);
            } while (size.Width > rect.Width);

            // Set up the text format.
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;

            // Create a path using the text and warp it randomly.
            GraphicsPath path = new GraphicsPath();

            path.AddString(this.text, font.FontFamily, (int)font.Style, font.Size, rect, format);
            float v = 4F;
            PointF[] points =
			{
				new PointF(this.random.Next(rect.Width) / v, this.random.Next(rect.Height) / v),
				new PointF(rect.Width - this.random.Next(rect.Width) / v, this.random.Next(rect.Height) / v),
				new PointF(this.random.Next(rect.Width) / v, rect.Height - this.random.Next(rect.Height) / v),
				new PointF(rect.Width - this.random.Next(rect.Width) / v, rect.Height - this.random.Next(rect.Height) / v)
			};
            Matrix matrix = new Matrix();
            matrix.Translate(0F, 0F);
            path.Warp(points, rect, matrix, WarpMode.Perspective, 0F);

            // Draw the text.
            hatchBrush = new HatchBrush(HatchStyle.Shingle, Color.White, Color.White);
            g.FillPath(hatchBrush, path);

            // Add some random noise.
            int m = Math.Max(rect.Width, rect.Height);
            for (int i = 0; i < (int)(rect.Width * rect.Height / 30F); i++)
            {
                int x = this.random.Next(rect.Width);
                int y = this.random.Next(rect.Height);
                int w = this.random.Next(m / 50);
                int h = this.random.Next(m / 50);
                g.FillEllipse(hatchBrush, x, y, w, h);
            }

            // Clean up.
            font.Dispose();
            hatchBrush.Dispose();
            g.Dispose();

            // Set the image.
            this.image = bitmap;
        }

        public void SetText(string txt)
        {
            this.text = txt;
        }

        // This method is for converting bitmap into a byte array
        public static byte[] BitmapToBytes(Image image)
        {
            byte[] byteArray = new byte[0];
            using (MemoryStream stream = new MemoryStream())
            {
                image.Save(stream, System.Drawing.Imaging.ImageFormat.Gif);
                stream.Close();
                byteArray = stream.ToArray();
            }
            return byteArray;
        }
    }
}
