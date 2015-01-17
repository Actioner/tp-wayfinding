namespace TP.Wayfinding.Site.Components.Services
{
    public interface IFileSystem
    {
        void Save(string path, byte[] bytes);
        void Delete(string path);
        byte[] Load(string path);
    }
}