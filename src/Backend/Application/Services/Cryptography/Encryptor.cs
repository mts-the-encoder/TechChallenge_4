using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Application.Services.Cryptography;

public class Encryptor
{
    private readonly string _encryptionKey;
    public Encryptor(string encryptionKey)
    {
        _encryptionKey = encryptionKey;
    }

    public string Encrypt(string password)
    {
        var passwordWithKey = $"{password}{_encryptionKey}";

        var bytes = Encoding.UTF8.GetBytes(passwordWithKey);
        var sha512 = SHA512.Create();
        var hashBytes = sha512.ComputeHash(bytes);
        return StringBytes(hashBytes);
    }

    private static string StringBytes(byte[] hashBytes)
    {
        var sb = new StringBuilder();
        foreach (var b in hashBytes)
        {
            var hex = b.ToString("x2");
            sb.Append(hex);
        }

        return sb.ToString();
    }
}