
using CryptoSniper.Database;
using System;
using System.Threading;

namespace CryptoSniper
{
    public class CryptoSniperService
    {
        #region Fields

        /// <summary>
        ///     The thread that starts the RabbitMQ receive consumer queue and watches for changing files.
        /// </summary>
        private static Thread StartThread { get; set; }

        private static Thread PriceFetcherThread { get; set; }

        /// <summary>
        ///     Flag indicating that the service has stopped.
        /// </summary>
        private static bool ServiceStopped { get; set; }

        #endregion

        #region Methods

        /// <summary>
        ///     Service startup method.
        /// </summary>
        /// <returns>Flag indicating service has started.</returns>
        public bool Start()
        {
            ServiceStopped = false;

            PriceFetcherThread = new Thread(GetPrices) { IsBackground = true };
            PriceFetcherThread.Start();

            StartThread = new Thread(StartService) { IsBackground = true };
            StartThread.Start();

            return true;
        }

        /// <summary>
        ///     The looping method.
        /// </summary>
        private static void StartService()
        {


            //DatabaseServiceHandler.GetAllUsers();

            //DatabaseServiceHandler.CreateOrder(DateTime.Now, 1, 10, 1, "BTC");

            // DatabaseServiceHandler.CompleteOrder(1, DateTime.Now, 200000, (decimal) 0.75);   //tested, works
            //DatabaseServiceHandler.GetAllInstantOrders(1);                                           //tested, works

            //DatabaseServiceHandler.GetLastPrice("BTC");

            //DatabaseServiceHandler.GetAllUsers();

            // Get Last Price 
            //var curr1 = "BTC";
            //var curr2 = "USD";
            //ApiService.GetLastPrice(curr1, curr2);

            // Get Open Orders
            //ApiService.GetOpenOrders();

            // Get Account Balance
            //ApiService.GetAccountBalance();

            // Get Active Order Status
            //ApiService.GetActiveOrderStatus();

            //while (true)
            //{
            //    if (ServiceStopped)
            //    {
            //        return;
            //    }

            // Get all users.
            var users = DatabaseServiceHandler.GetAllUsers();

            // Validate active CEX.IO account. Remove invalid ones.
            foreach (var user in users)
            {
                if (user.CexioUserId == null || user.CexioKey == null || user.CexioSecret == null)
                {
                    users.Remove(user);
                }
            }

            // If investment date past, calculate investment.
            foreach (var user in users)
            {
                var investmentDate = user.NextInvestmentCheck;
                var now = DateTime.Now;
                

                if (DateTime.Compare(investmentDate.Value, now) >= 0)
                {
                    // Increment next investment check;
                    now = DateTime.Now;
                    var nextInvestmentDateTime = now.AddMinutes(user.PriceDerivativeTime);
                    DatabaseServiceHandler.UpdateNextInvestmentCheck(user.UserId, nextInvestmentDateTime);
                }
            }



            // Check result...
            // If good, create InstantOrder on database.

            // Point A - now 
            //var pointA = DatabaseServiceHandler.GetLastPrice("BTC");

            // Point B - now - price_derivative_time
            //var pointB = DatabaseServiceHandler.GetLastPrice("BTC", 2);

            var baseCurrency = "USD"; // TODO: make this a user database parameter.

            // Execute investment.
            var response = ApiService.PlaceInstantOrder("BTC", baseCurrency, "buy", "0.0002");

            //Thread.Sleep(1000 * 60);
           //}
        }

        private static void GetPrices()
        {
            while (true)
            {
                if (ServiceStopped)
                {
                    return;
                }

                // Fetch current prices.
                var btcUsd = ApiService.GetLastPrice("BTC", "USD");
                var ethUsd = ApiService.GetLastPrice("ETH", "USD");
                var btgUsd = ApiService.GetLastPrice("BTG", "USD");
                var xrpUsd = ApiService.GetLastPrice("XRP", "USD");
                var bchUsd = ApiService.GetLastPrice("BCH", "USD");

                // Store them in database.
                DatabaseServiceHandler.CreateLastPriceRecords(btcUsd, ethUsd, btgUsd, xrpUsd, bchUsd);

                Thread.Sleep(1000 * 60); // Once a minute.
            }
        }

        /// <summary>
        ///     Service stop method.
        /// </summary>
        /// <returns>Flag indicating service has stopped.</returns>
        public bool Stop()
        {
            ServiceStopped = true;
            DbConnection.Connection.Close();

            return true;
        }

        #endregion
    }
}
