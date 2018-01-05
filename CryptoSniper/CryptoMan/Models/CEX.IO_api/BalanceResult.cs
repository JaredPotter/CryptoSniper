using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoMan.Models
{
    public class BalanceResult
    {
        public string TimeStamp { get; set; }

        public string Username { get; set; }

        public BTC BTC { get; set; }

        public BCH BCH { get; set; }

        public ETH ETH { get; set; }

        public XRP USD { get; set; }

        public BTG BTG { get; set; }

        public DASH DASH { get; set; }

        public ZEC ZEC { get; set; }
    }

    public class BTC
    {
        public string Available { get; set; }

        public string Orders { get; set; }
    }

    public class BCH
    {
        public string Available { get; set; }

        public string Orders { get; set; }
    }

    public class ETH
    {
        public string Available { get; set; }

        public string Orders { get; set; }
    }

    public class USD
    {
        public string Available { get; set; }

        public string Orders { get; set; }
    }

    public class XRP
    {
        public string Available { get; set; }

        public string Orders { get; set; }
    }

    public class BTG
    {
        public string Available { get; set; }

        public string Orders { get; set; }
    }

    public class DASH
    {
        public string Available { get; set; }

        public string Orders { get; set; }
    }

    public class ZEC
    {
        public string Available { get; set; }

        public string Orders { get; set; }
    }
}
