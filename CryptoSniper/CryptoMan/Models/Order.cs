using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoSniper.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public bool Completed { get; set; }
        public DateTime BuyDate { get; set; }
        public DateTime SellDate { get; set; }
        public int UserId { get; set; }
        public decimal BuyPrice { get; set; }
        public decimal SellPrice { get; set; }
        public decimal ProfitPercentage { get; set; }
        public decimal Amount { get; set; }
    }
}
