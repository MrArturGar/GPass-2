using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GPass.Models
{
    public class CredentialGroup
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public int Order { get; set; }

        public virtual ICollection<CredentialSet> CredentialSets { get; set; } = new List<CredentialSet>();

        public static CredentialGroup GetTestPlaceholders()
        {
            return new()
            {
                Name = "Test PoolList",
                Order = 1,
                CredentialSets = [CredentialSet.GetTestPlaceholders(), CredentialSet.GetTestPlaceholders(), CredentialSet.GetTestPlaceholders()]
            };
        }
    }
} 