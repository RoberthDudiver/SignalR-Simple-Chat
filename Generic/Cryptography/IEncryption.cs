namespace Generic.Cryptography
{
    public interface IEncryption
    {
        string Base64Decode(string base64EncodedData);
        string Base64Encode(string plainText);
        string decrypt(string text);
        string Decrypt(string text, bool full);
        string encrypt(string text);
        string Encrypt(string text, bool full);
    }
}