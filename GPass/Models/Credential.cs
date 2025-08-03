using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;

namespace GPass.Models
{
    public interface ICredential
    {
        int Id { get; set; }
        int SetId { get; set; }
        string Type { get; set; }
        int Order { get; set; }
    }

    public abstract class CredentialBase : ICredential
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int SetId { get; set; }

        [Required]
        public string Type { get; set; } = string.Empty;

        [Required]
        public int Order { get; set; }

        [ForeignKey(nameof(SetId))]
        public virtual CredentialSet Set { get; set; } = null!;
    }

    public class CredTitle : CredentialBase
    {
        [Required]
        public string Title { get; set; } = "New Title";
    }

    public class CredField : CredentialBase
    {
        [Required]
        public string Field { get; set; } = "New Field";
    }

    public class CredSecretField : CredentialBase
    {
        private string _secretField = string.Empty;
        private string _encryptedField = string.Empty;

        [Required]
        public string SecretField 
        { 
            get => _secretField;
            set => _secretField = value;
        }

        public string EncryptedField
        {
            get => _encryptedField;
            set => _encryptedField = value;
        }

        public void Encrypt(byte[] key)
        {
            using var aes = Aes.Create();
            aes.Key = key;
            aes.GenerateIV();

            using var encryptor = aes.CreateEncryptor();
            var plainBytes = Encoding.UTF8.GetBytes(_secretField);
            var cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

            // Сохраняем IV и зашифрованные данные
            var result = new byte[aes.IV.Length + cipherBytes.Length];
            Buffer.BlockCopy(aes.IV, 0, result, 0, aes.IV.Length);
            Buffer.BlockCopy(cipherBytes, 0, result, aes.IV.Length, cipherBytes.Length);

            _encryptedField = Convert.ToBase64String(result);
        }

        public void Decrypt(byte[] key)
        {
            if (string.IsNullOrEmpty(_encryptedField)) return;

            var encryptedBytes = Convert.FromBase64String(_encryptedField);
            
            using var aes = Aes.Create();
            aes.Key = key;

            // Извлекаем IV
            var iv = new byte[aes.IV.Length];
            Buffer.BlockCopy(encryptedBytes, 0, iv, 0, iv.Length);
            aes.IV = iv;

            // Извлекаем зашифрованные данные
            var cipherBytes = new byte[encryptedBytes.Length - iv.Length];
            Buffer.BlockCopy(encryptedBytes, iv.Length, cipherBytes, 0, cipherBytes.Length);

            using var decryptor = aes.CreateDecryptor();
            var plainBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
            _secretField = Encoding.UTF8.GetString(plainBytes);
        }
    }

    public class CredLine : CredentialBase
    {
    }
}
