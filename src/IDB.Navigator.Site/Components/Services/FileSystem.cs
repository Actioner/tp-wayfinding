using System;
using System.IO;

namespace IDB.Navigator.Site.Components.Services
{
    public class FileSystem : IFileSystem
    {
        public void Save(string path, byte[] bytes)
        {
            File.WriteAllBytes(path, bytes);
        }

        public void Delete(string path)
        {
            try
            {
                File.Delete(path);
            }
            catch (Exception)
            {
            }
        }

        public byte[] Load(string path)
        {
            return File.ReadAllBytes(path);
        }
    }
}
