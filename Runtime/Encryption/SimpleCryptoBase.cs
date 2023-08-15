using System.Threading.Tasks;

namespace SensenToolkit
{
    // Usage
    // Create your own class that inherits from SimpleCryptoBase
    // and pass in your own key and iv strings.
    //
    // Example
    // public sealed class SimpleCrypto : SimpleCryptoBase
    // {
    //     private static SimpleCrypto _instance;
    //     public static SimpleCrypto Instance => _instance ??= new();
    //     private SimpleCrypto() : base(
    //         keyString: "3377e7262417352e71085a81fafb412118929f8a38cd8a04ac1a7ec6913c56b1",
    //         ivString: "f4d65c5da2b2de3a43b30480229355078d5328099a16fb1387d680b530051485"
    //     ) { }
    // }

    public class SimpleCryptoBase
    {
        private readonly byte[] _key;
        private readonly Crypto _crypto;

        protected SimpleCryptoBase(string keyString, string ivString)
        {
            byte[] iv = CryptoUtilities.GenerateIVFromString(ivString);
            _key = CryptoUtilities.DeriveKeyFromString(keyString, iv);
            _crypto = new Crypto(iv);
        }
        public Task<byte[]> Encrypt(string plainText)
        {
            return _crypto.Encrypt(plainText, _key);
        }

        public Task<string> Decrypt(byte[] encryptedContent)
        {
            return _crypto.Decrypt(encryptedContent, _key);
        }
        public Task EncryptToFile(string plainText, string filePath)
        {
            return _crypto.EncryptToFile(plainText, filePath, _key);
        }

        public Task<string> DecryptFromFile(string filePath)
        {
            return _crypto.DecryptFromFile(filePath, _key);
        }
    }
}
