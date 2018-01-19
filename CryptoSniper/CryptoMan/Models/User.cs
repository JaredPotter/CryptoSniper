using CryptoSniper.Config.Models;
using System;

namespace CryptoSniper.Models
{
    public class User
    {
        #region Properties        

        public int UserId { get; set; }

        public string Username { get; set; }

        public CexIoApiInfo CexIoCredentials { get; set; }

        public int PriceDerivativeTime { get; set; }

        public decimal InvestmentPercentage { get; set; }

        public DateTime? NextInvestmentCheck { get; set; }

        #endregion
    }
}
