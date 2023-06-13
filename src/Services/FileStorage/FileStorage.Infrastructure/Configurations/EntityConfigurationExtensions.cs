using FileStorage.Infrastructure.Encryption;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FileStorage.Infrastructure.Configurations;

public static class EntityConfigurationExtensions
{
    public static PropertyBuilder<string> Encrypt(this PropertyBuilder<string> propertyBuilder, IEncrypt encryptor)
    {
        return propertyBuilder.HasConversion(
            v => encryptor.Encrypt(v),
            v => encryptor.Decrypt(v));
    }

    public static PropertyBuilder<T> Encrypt<T>(this PropertyBuilder<T> propertyBuilder, IEncrypt encryptor)
        where T : new()
    {
        return propertyBuilder.HasConversion(
            x => encryptor.Encrypt(ChangeType<string>(x)),
            x => ChangeType<T>(encryptor.Decrypt(x)));
    }

    private static T ChangeType<T>(Object value)
    {
        return (T)Convert.ChangeType(value, typeof(T));
    }
}