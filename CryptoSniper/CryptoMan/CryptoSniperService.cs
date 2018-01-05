
using CryptoSniper.Database;
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

            StartThread = new Thread(StartService) { IsBackground = true };
            StartThread.Start();

            return true;
        }

        /// <summary>
        ///     The looping method.
        /// </summary>
        private static void StartService()
        {
            // Get Last Price 
            //var curr1 = "BTC";
            //var curr2 = "USD";
            //ApiService.GetLastPrice(curr1, curr2);

            // Get Open Orders
            //ApiService.GetOpenOrders();

            // Get Account Balance
            ApiService.GetAccountBalance();

            // Get Active Order Status
            //ApiService.GetActiveOrderStatus();

            //while (true)
            //{
            //    if (ServiceStopped)
            //    {
            //        return;
            //    }

            //    Thread.Sleep(1000 * 60);
            //}
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
