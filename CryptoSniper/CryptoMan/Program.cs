using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoMan
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get Last Price 
            //var curr1 = "BTC";
            //var curr2 = "USD";
            //ApiService.GetLastPrice(curr1, curr2);

            // Get Open Orders
            //ApiService.GetOpenOrders();

            // Get Account Balance
            //ApiService.GetAccountBalance();

            // Get Active Order Status
            ApiService.GetActiveOrderStatus();
        }
    }
}
