namespace FileStorage.Infrastructure.Encryption;

public interface IEncrypt
{
    string Encrypt(string planText);
    string Decrypt(string cipherText);
}