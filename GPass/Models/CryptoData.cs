using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPass.Models
{
    public class CryptoData
    {
        public required ObservableCollection<PoolList> PoolLists { get; set; }

        public static CryptoData GetTestPlaceholders()
        {
            return new() { PoolLists = [
                PoolList.GetTestPlaceholders(),
                PoolList.GetTestPlaceholders(),
                PoolList.GetTestPlaceholders(),
                ]
            };
        }
    }
}
