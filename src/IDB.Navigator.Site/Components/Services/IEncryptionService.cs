namespace IDB.Navigator.Site.Components.Services
{
    public interface IEncryptionService
    {
        string EncryptPassword(string plainPassword);

        string EncryptToken(string message);
        string DecryptToken(string message);
    }
}