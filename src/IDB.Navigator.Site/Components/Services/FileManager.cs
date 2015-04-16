using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;

namespace IDB.Navigator.Site.Components.Services
{
    public class FileManager : IFileManager
    {
        private readonly IFileSystem fileSystem;
        private readonly string workPath;

        public FileManager(IFileSystem fileSystem, string workPath)
        {
            this.fileSystem = fileSystem;
            this.workPath = workPath;
        }

        public void SaveImage(Image image, string filename, ImageFormat imageFormat)
        {
            if (image == null) throw new ArgumentNullException("image");

            string fullPath = HttpContext.Current.Server.MapPath(Path.Combine(workPath, filename));

            image.Save(fullPath, imageFormat);
        }


        public void Save(string filename, byte[] content)
        {
            if (filename == null) throw new ArgumentNullException("filename");

            string fullPath = HttpContext.Current.Server.MapPath((Path.Combine(workPath, filename)));

            fileSystem.Save(filename, content);
        }

        public void Delete(string filename)
        {
            string fullPath = HttpContext.Current.Server.MapPath((Path.Combine(workPath, filename)));
            fileSystem.Delete(fullPath);
        }

        public byte[] Load(string filename)
        {
            string fullPath = HttpContext.Current.Server.MapPath((Path.Combine(workPath, filename)));

            return fileSystem.Load(fullPath);
        }
    }
}