using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPass.Services;

public interface IEncryptionService
{
    byte[] DeriveKey(string password, byte[] salt);
    byte[] Encrypt(byte[] data, byte[] key, byte[] nonce);
    byte[] Decrypt(byte[] data, byte[] key, byte[] nonce);
}