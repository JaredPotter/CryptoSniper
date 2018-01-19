using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoSniper.Models
{
    public class UserInvestmentPlan
    {
        #region Properties

        public int Id { get; set; }

        public string Currency { get; set; }

        public decimal Percent { get; set; }

        public int UserId { get; set; }

        public decimal FallPercent { get; set; }

        public decimal StabalizePercent { get; set; }

        #endregion
    }
}
