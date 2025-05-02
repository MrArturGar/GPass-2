using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPass.Models
{
    public class PoolList
    {
        public required string Name { get; set; }
        public required ObservableCollection<CredentialPool> CredentialPools { get; set; }
        public required int Order { get; set; }

        public static PoolList GetTestPlaceholders()
        {
            return new()
            {
                Name = "Test PoolList",
                Order = 1,
                CredentialPools = [CredentialPool.GetTestPlaceholders(), CredentialPool.GetTestPlaceholders(), CredentialPool.GetTestPlaceholders()]
            };
        }
    }
}
