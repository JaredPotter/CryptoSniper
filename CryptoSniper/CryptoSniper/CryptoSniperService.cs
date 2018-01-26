using CryptoSniper.Database;
using CryptoSniper.Models;
using System;
using System.Collections.Generic;
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
        /// 
        /// </summary>
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

            //PriceFetcherThread = new Thread(GetPrices) { IsBackground = true };
            //PriceFetcherThread.Start();

            StartThread = new Thread(StartService) { IsBackground = true };
            StartThread.Start();

            return true;
        }

        /// <summary>
        ///     The looping method.
        /// </summary>
        private static void StartService()
        {
            // Get all users.
            var users = DatabaseServiceHandler.GetAllUsers();
            var validatedUsers = new List<User>();

            // Validate active CEX.IO account. Remove invalid ones.
            foreach (var user in users)
            {
                if (user.CexIoCredentials.UserId != null || user.CexIoCredentials.Key != null || user.CexIoCredentials.Secret != null)
                {
                    validatedUsers.Add(user);
                }
            }

            // Iterate over all valid users.
            foreach (var user in validatedUsers)
            {
                var investmentDate = user.NextInvestmentCheck;
                var now = DateTime.Now;

                // If investment date past, calculate investment.
                //if the current date has passed the date the user wants to place the order
                if (DateTime.Compare(investmentDate.Value, now) <= 0)
                {
                    // Increment next investment check;
                    now = DateTime.Now;
                    var nextInvestmentDateTime = now.AddMinutes(user.PriceDerivativeTime);
                    //DatabaseServiceHandler.UpdateNextInvestmentCheck(user.UserId, nextInvestmentDateTime);

                    // Get user's investment plans.
                    var investmentPlans = DatabaseServiceHandler.GetUserInvestmentPlans(user.UserId);

                    foreach (var investmentPlan in investmentPlans)
                    {
                        var symbol1 = investmentPlan.Currency;
                        var baseCurrency = "USD"; // TODO: make this a user database parameter.
                        var symbol2 = baseCurrency;

                        // Check if any incomplete instant orders exist. 
                        // If yes, try to sell.
                        // If no, then try to buy.
                        //implementing sell orders here

                        var openInstantIncompleteOrders = DatabaseServiceHandler.GetAllInstantOrders(user.UserId);


                        //Create Point A - now 
                        var pointA = DatabaseServiceHandler.GetLastPrice(symbol1);

                        // Point B - now - price_derivative_time (15 mins)
                        var pointB = DatabaseServiceHandler.GetLastPrice(symbol1, 15);

                        //Point C - now - price_derivative_time (30 mins)
                        var pointC = DatabaseServiceHandler.GetLastPrice(symbol1, 30);

                        var result = CalculateBuyOption(pointA, pointB, pointC);

                        if (result == true)
                        {
                            var usdBalanceAmount = Convert.ToDecimal(ApiService.GetAccountBalance(user.CexIoCredentials).USD.Available);
                            var amountToBuy = usdBalanceAmount * user.InvestmentPercentage;
                            
                            // Place the buy order.
                            var response = ApiService.PlaceInstantOrder(user.CexIoCredentials, symbol1, symbol2, "buy", amountToBuy.ToString());
                            // TODO: verify payment went through.

                            // TODO: add the rest of the cryptocurrency smallest demoninations.
                            // ETH
                            // BTG
                            // XRP
                            // BCH
                            // DASH
                            // ZEC
                            // BCH 
                            var smallestDemoninations = new Dictionary<string, double>
                            {
                                { "USD", 0.00 },
                                { "BTC",  0.00000001 }
                            };

                            var symbol1SmallestDemonination = smallestDemoninations[symbol1];
                            var symbol2SmallestDemonination = smallestDemoninations[symbol2];
                            var symbol1amount = Convert.ToInt32(response.Symbol1Amount);
                            var symbol2amount = Convert.ToInt32(response.Symbol2Amount);

                            var buyPrice = (symbol2amount * symbol2SmallestDemonination) / (symbol1amount * symbol1SmallestDemonination);
                            buyPrice = Math.Round(buyPrice, 2);

                            DatabaseServiceHandler.CreateInstantOrder(DateTime.UtcNow, user.UserId, buyPrice, Math.Round(symbol1amount * symbol1SmallestDemonination, 8), symbol1);
                        //We need to start here when we come back
                        //DatabaseServiceHandler.CreateOrder(DateTime.UtcNow, user.UserId, response.Symbol1Amount);
                        }
                    }
                }
            }

            // Check result...
            // If good, create InstantOrder on database.

            // Execute investment.
         //   var response = ApiService.PlaceInstantOrder("BTC", baseCurrency, "buy", "0.0002");

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
                var dashUsd = ApiService.GetLastPrice("DASH", "USD");
                var zecUsd = ApiService.GetLastPrice("ZEC", "USD");

                // Store them in database.
                DatabaseServiceHandler.CreateLastPriceRecords(btcUsd, ethUsd, btgUsd, xrpUsd, bchUsd, dashUsd, zecUsd);

                Thread.Sleep(1000 * 57); // Once a minute.
            }
        }

        /// <summary>
        /// Checks the latest entries and determines if the price has fallen low enough to buy
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        public static bool CalculateBuyOption(Decimal a, Decimal b, Decimal c)
        {
            //A is the most recent price, while C is the latest price

            var percentageChange = c - ((decimal).10 * c);

            if (percentageChange >= b)
            {
                var stabilization = a * (decimal).02;

                var upperbound = b + stabilization;
                var lowerbound = b - stabilization;

                if (a >= lowerbound && a <= upperbound)
                {
                    return true;
                }
            }

            //else, assume price is still above the threshhold 

            return false;

        }

        public static void CalculateSellOption(Decimal a, Decimal b, Decimal c)
        {
            //A is the most recent price, while C is the latest price

            var percentageChange = c + ((decimal).10 * b);

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
