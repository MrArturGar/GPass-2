using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GPass.Models
{
    public class CredentialSet
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int GroupId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public int Order { get; set; }

        [ForeignKey(nameof(GroupId))]
        public virtual CredentialGroup Group { get; set; } = null!;

        public virtual ICollection<CredentialBase> Credentials { get; set; } = new List<CredentialBase>();

        public static CredentialSet GetTestPlaceholders()
        {
            return new()
            {
                Name = "Test CredentialPool",
                Order = 1,
                Credentials = [
                    new CredTitle() { Title = "Test title" },
                    new CredField() { Field = "Test field" },
                    new CredSecretField() { SecretField = "Test secret dield" },
                    new CredLine(),
                    new CredTitle() { Title = "Test title 2" },
                ]
            };
        }
    }
} 