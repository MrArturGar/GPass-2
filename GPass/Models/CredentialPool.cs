using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPass.Models
{
    public class CredentialPool
    {
        public required string Name { get; set; }
        public required ObservableCollection<ICredential> Credentials { get; set; }

        public static CredentialPool GetTestPlaceholders()
        {
            return new()
            {
                Name = "Test CredentialPool",
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
