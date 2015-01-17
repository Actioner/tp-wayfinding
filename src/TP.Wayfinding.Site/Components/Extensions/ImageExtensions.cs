using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace TP.Wayfinding.Site.Components.Extensions
{
    public static class ImageExtensions
    {
        public static string ImageToBase64(this Image image, ImageFormat format)
        {
            const string header = "data:image/png;base64,";

            using (var ms = new MemoryStream())
            {
                // Convert Image to byte[]
                image.Save(ms, format);
                byte[] imageBytes = ms.ToArray();

                // Convert byte[] to Base64 String
                string base64String = Convert.ToBase64String(imageBytes);
                return header + base64String;
            }
        }

        public static Image Base64ToImage(this string base64String)
        {
            // Convert Base64 String to byte[]
            string imageInfo = base64String.Split(',')[1];
            byte[] imageBytes = Convert.FromBase64String(imageInfo);
            Image image = null;
            using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
            {
                // Convert byte[] to Image
                ms.Write(imageBytes, 0, imageBytes.Length);
                image = Image.FromStream(ms, true);
                ms.Position = 0;
            }

            
            return image;
        }
    }
}