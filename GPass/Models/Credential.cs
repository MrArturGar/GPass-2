using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPass.Models
{
    public interface ICredential
    {
        public int Order { get; set; }
    }

    public class CredTitle: ICredential
    {
        public int Order { get; set; }
        public required string Title { get; set; } = "New Title";
    }

    public class CredSecretField : ICredential
    {
        public int Order { get; set; }
        public required string SecretField { get; set; } = string.Empty;
    }

    public class CredField : ICredential
    {
        public int Order { get; set; }
        public required string Field { get; set; } = "New Field";
    }

    public class CredLine : ICredential
    {
        public int Order { get; set; }
    }
}
