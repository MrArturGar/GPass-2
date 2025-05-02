using System;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using Konscious.Security.Cryptography;

namespace GPass.Services;

public class EncryptionService : IEncryptionService
{
    public byte[] DeriveKey(string password, byte[] salt)
    {
        using var argon2 = new Argon2id(System.Text.Encoding.UTF8.GetBytes(password));
        argon2.Salt = salt;
        argon2.DegreeOfParallelism = 4;
        argon2.Iterations = 10;
        argon2.MemorySize = 1024 * 1024;
        return argon2.GetBytes(32);
    }

    public byte[] Encrypt(byte[] data, byte[] key, byte[] nonce)
    {
        using var aes = new AesGcm(key);
        var ciphertext = new byte[data.Length];
        var tag = new byte[16];
        aes.Encrypt(nonce, data, ciphertext, tag);
        return Combine(nonce, tag, ciphertext);
    }

    public byte[] Decrypt(byte[] data, byte[] key, byte[] nonce)
    {
        using var aes = new AesGcm(key);
        var (nt, tag, ciphertext) = Split(data);
        var plaintext = new byte[ciphertext.Length];
        aes.Decrypt(nt, ciphertext, tag, plaintext);
        return plaintext;
    }

    private static byte[] Combine(params byte[][] arrays)
    {
        var result = new byte[arrays.Sum(a => a.Length)];
        var offset = 0;
        foreach (var array in arrays)
        {
            Buffer.BlockCopy(array, 0, result, offset, array.Length);
            offset += array.Length;
        }
        return result;
    }

    private static (byte[] nonce, byte[] tag, byte[] ciphertext) Split(byte[] data)
    {
        var nonce = new byte[12];
        var tag = new byte[16];
        var ciphertext = new byte[data.Length - 28];
        Buffer.BlockCopy(data, 0, nonce, 0, 12);
        Buffer.BlockCopy(data, 12, tag, 0, 16);
        Buffer.BlockCopy(data, 28, ciphertext, 0, ciphertext.Length);
        return (nonce, tag, ciphertext);
    }
}