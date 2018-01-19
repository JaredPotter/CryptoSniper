using System;

namespace CryptoSniper.Models
{
    public class User
    {
        #region Properties        

        public int UserId { get; set; }

        public string Username { get; set; }

        public string CexioUserId { get; set; }

        public string CexioKey { get; set; }

        public string CexioSecret { get; set; }

        public int PriceDerivativeTime { get; set; }

        public decimal InvestmentPercentage { get; set; }

        public DateTime? NextInvestmentCheck { get; set; }

        #endregion
    }
}
