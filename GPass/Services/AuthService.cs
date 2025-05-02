using GPass.Models;
using GPass.Services;
using GPass.Utils;

namespace GPass.Services;

public class AuthService
{
    private readonly IEncryptionService _encryption;

    public AuthService(IEncryptionService encryption) => _encryption = encryption;

    public bool ValidatePassword(string password, Vault vault)
    {
        using var keyBuffer = new SecureBuffer();
        keyBuffer.Append(password);
        var key = _encryption.DeriveKey(password, vault.Salt);
        return key.Length == 32;
    }
}