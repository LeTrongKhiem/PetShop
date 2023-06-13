using Microsoft.AspNetCore.DataProtection;

namespace FileStorage.Infrastructure.Encryption;

public class Encryptor : IEncrypt
{
    private readonly IDataProtector _dataProtector;

    public Encryptor(IDataProtectionProvider dataProtectionProvider)
    {
        _dataProtector = dataProtectionProvider.CreateProtector(GetType().FullName);
    }
    public string Encrypt(string planText)
    {
        return !string.IsNullOrEmpty(planText) ? _dataProtector.Protect(planText) : planText;
    }

    public string Decrypt(string cipherText)
    {
        return !string.IsNullOrEmpty(cipherText) ? _dataProtector.Unprotect(cipherText) : cipherText;
    }
}