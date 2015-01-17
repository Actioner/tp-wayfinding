using System.Web;

namespace TP.Wayfinding.Site.Components.Services
{
    public interface IFileManager
    {
        void Save(string filename, byte[] content);
        void Delete(string filename);
        byte[] Load(string filePath);
    }
}