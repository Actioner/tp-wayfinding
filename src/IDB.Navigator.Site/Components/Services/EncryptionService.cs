using System;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace IDB.Navigator.Site.Components.Services
{
    public class EncryptionService : IEncryptionService
    {
        private readonly MD5 _md5;
        private readonly ASCIIEncoding _asciiEncoding;
        private readonly UTF8Encoding _utf8Encoding;
        private readonly string _passPhrase;

        public EncryptionService()
        {
            _md5 = MD5.Create();
            _asciiEncoding = new ASCIIEncoding();
            _utf8Encoding = new UTF8Encoding();
            _passPhrase = ConfigurationManager.AppSettings["ApiPassphrase"] ?? "apiPassphrase";
        }

        public string EncryptPassword(string plainPassword)
        {
            var sb = new StringBuilder();
            var stream = _md5.ComputeHash(_asciiEncoding.GetBytes(plainPassword));
            foreach (var t in stream)
                sb.AppendFormat("{0:x2}", t);
            return sb.ToString();
        }

        public string EncryptToken(string message)
        {
            byte[] results;
            byte[] tdesKey = _md5.ComputeHash(_utf8Encoding.GetBytes(_passPhrase));
            var tdesAlgorithm = new TripleDESCryptoServiceProvider
                                    {
                                        Key = tdesKey,
                                        Mode = CipherMode.ECB,
                                        Padding = PaddingMode.PKCS7
                                    };

            byte[] dataToEncrypt = _utf8Encoding.GetBytes(message);

            try
            {
                var encryptor = tdesAlgorithm.CreateEncryptor();
                results = encryptor.TransformFinalBlock(dataToEncrypt, 0, dataToEncrypt.Length);
            }
            finally
            {
                tdesAlgorithm.Clear();
            }
            return Convert.ToBase64String(results);
        }

        public string DecryptToken(string message)
        {
            byte[] results;

            byte[] tdesKey = _md5.ComputeHash(_utf8Encoding.GetBytes(_passPhrase));
            var tdesAlgorithm = new TripleDESCryptoServiceProvider
                                    {
                                        Key = tdesKey, 
                                        Mode = CipherMode.ECB, 
                                        Padding = PaddingMode.PKCS7
                                    };
            byte[] dataToDecrypt = Convert.FromBase64String(message);

            try
            {
                var decryptor = tdesAlgorithm.CreateDecryptor();
                results = decryptor.TransformFinalBlock(dataToDecrypt, 0, dataToDecrypt.Length);
            }
            finally
            {
                tdesAlgorithm.Clear();
            }
            
            return _utf8Encoding.GetString(results);
        }
    }
}
