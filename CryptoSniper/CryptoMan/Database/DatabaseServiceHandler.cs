﻿using CryptoSniper.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CryptoSniper.Database
{
    /// <summary>
    ///     Handles specific database query operations such as inserting data.
    /// </summary>
    public static class DatabaseServiceHandler
    {
        #region Fields

        /// <summary>
        ///     Lock to prevent race conditions.
        /// </summary>
        private static readonly Object ThisLock = new Object();

        public static object DebugLog { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        ///      Retrieves all users from the database.
        /// </summary>
        public static List<User> GetAllUsers()
        {
            var query = $"SELECT * FROM User;";
            var results = ExecuteGetQuery(query);
            var users = new List<User>();

            foreach (var result in results)
            {
                var user = new User();

                user.UserId = Convert.ToInt32(result["user_id"]);
                user.Username = (string)result["username"];
                user.CexioUserId = (string)result["cexio_user_id"];
                user.CexioKey = (string)result["cexio_key"];
                user.CexioSecret = (string)result["cexio_secret"];
                user.PriceDerivativeTime = Convert.ToInt32(result["price_derivative_time"]);
                user.InvestmentPercentage = Convert.ToInt32(result["investment_percentage"]);
                user.NextInvestmentCheck = (DateTime?)result["next_investment_check"];

                users.Add(user);      
            }

            return users;
        }

        public static void UpdateNextInvestmentCheck(int userId, DateTime nextInvestmentCheck)
        {
            var query = $"UPDATE User SET next_investment_check = '{nextInvestmentCheck.ToString("yyyy-MM-dd HH:mm:ss")}' WHERE user_id = {userId};";

            // TODO: get rows affected. verify equals 1.
            ExecuteInsertUpdateQuery(query);
        }

        public static List<InstantOrder> GetAllInstantOrders(int userId)
        {
            var query = $"SELECT * FROM crypto_sniper.`InstantOrder` WHERE user_id = '{userId}';";

            var results = ExecuteGetQuery(query);

            var orders = new List<InstantOrder>();

            foreach(var result in results)
            {
                var order = new InstantOrder();

                order.OrderId = Convert.ToInt32(result["order_id"]);
                order.Completed = Convert.ToBoolean(result["completed"]);
                order.BuyDate = (DateTime?)result["buy_date"];
                order.SellDate = (DateTime?)result["sell_date"];
                order.UserId = (int)result["user_id"];
                order.BuyPrice = Convert.ToDecimal(result["buy_price"]);
                order.SellPrice = Convert.ToDecimal(result["sell_price"]);
                order.ProfitPercentage = Convert.ToDecimal(result["profit_percentage"]);
                order.Amount = Convert.ToDecimal(result["amount"]);

                orders.Add(order);
            }

            return orders;
        }

        public static void CompleteOrder(int orderId, DateTime sellDate, decimal sellPrice, decimal profitPercentage)
        {
            var query = $"UPDATE crypto_sniper.`InstantOrder` SET completed = 1, sell_date = '{sellDate.ToString("yyyy-MM-dd H:mm:ss")}', sell_price = {sellPrice}, profit_percentage = {profitPercentage} " +
            $"WHERE order_id = { orderId };";

            ExecuteInsertUpdateQuery(query);
        }

        public static void CreateOrder(DateTime buyDate, int userId, int buyPrice, int amount, string cryptoCurrency)
        {
            var query = $"INSERT INTO crypto_sniper.`InstantOrder` (buy_date, user_id, buy_price, amount, crypto_currency) " +
                                                  $"VALUES ('{buyDate.ToString("yyyy-MM-dd H:mm:ss")}', {userId}, {buyPrice}, {amount}, '{cryptoCurrency}');";
            ExecuteInsertUpdateQuery(query);
        }

        public static void CreateLastPriceRecords(LastPriceResult btcUsd, LastPriceResult ethUsd, LastPriceResult btgUsd, LastPriceResult xrpUsd, LastPriceResult bchUsd)
        {
            var query1 = $"INSERT INTO HistoricalPriceBtcUsd (price, date) VALUES ({Convert.ToDecimal(btcUsd.lprice)}, NOW());";
            var query2 = $"INSERT INTO HistoricalPriceEthUsd (price, date) VALUES ({Convert.ToDecimal(ethUsd.lprice)}, NOW());";
            var query3 = $"INSERT INTO HistoricalPriceBtgUsd (price, date) VALUES ({Convert.ToDecimal(btgUsd.lprice)}, NOW());";
            var query4 = $"INSERT INTO HistoricalPriceXrpUsd (price, date) VALUES ({Convert.ToDecimal(xrpUsd.lprice)}, NOW());";
            var query5 = $"INSERT INTO HistoricalPriceBchUsd (price, date) VALUES ({Convert.ToDecimal(bchUsd.lprice)}, NOW());";

            ExecuteInsertUpdateQuery(query1);
            ExecuteInsertUpdateQuery(query2);
            ExecuteInsertUpdateQuery(query3);
            ExecuteInsertUpdateQuery(query4);
            ExecuteInsertUpdateQuery(query5);
        }

        public static decimal GetLastPrice(string currency, int priceDerivativeTime = 0)
        {
           // var tempquery = $"SELECT * FROM HistoricalPriceBtcUsd ORDER BY id DESC LIMIT 3;";
            var now = DateTime.UtcNow.AddMinutes(-priceDerivativeTime);
            var nowString = now.ToString("yyyy-MM-dd HH:mm") + ":%%";
            var query = "";

            switch (currency)
            {
                case "BTC":
                    query = $"SELECT * FROM HistoricalPriceBtcUsd WHERE date LIKE '{nowString}';";
                    break;

                case "BTG":
                    query = $"SELECT * FROM HistoricalPriceBtgUsd WHERE date LIKE '{nowString}';";
                    break;

                case "ETH":
                    query = $"SELECT * FROM HistoricalPriceEthUsd WHERE date LIKE '{nowString}';";
                    break;

                case "XRP":
                    query = $"SELECT * FROM HistoricalPriceXrpUsd WHERE date LIKE '{nowString}';";
                    break;

                case "BCH":
                    query = $"SELECT * FROM HistoricalPriceBchUsd WHERE date LIKE '{nowString}';";
                    break;

                default:
                break;
            }

            var results = ExecuteGetQuery(query);

            decimal lastPrice = 0;

            var result = results.FirstOrDefault();

            lastPrice = result != null ? (decimal)result["price"] : 0;

            if (lastPrice == 0)
            {
                throw new Exception("Last price not available.");
            }

            return lastPrice;
        }

        /// <summary>
        ///     Executes the query.
        /// </summary>
        /// <param name="query">The query to be executed.</param>
        private static void ExecuteInsertUpdateQuery(string query)
        {
            lock (ThisLock)
            {
                try
                {
                    var connection = DbConnection.Connection;

                    var cmd = new MySqlCommand(query, connection);

                    cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    // MySQL Error
                    
                    // Closes the connection.
                    DbConnection.Connection.Close();

                    // Reestablishes connection with new query.
                    ExecuteInsertUpdateQuery(query);

                    // continue.
                }
            }
        }

        private static List<Dictionary<string, object>> ExecuteGetQuery(string query)
        {
            var results = new List<Dictionary<string, object>>();

            lock (ThisLock)
            {
                try
                {
                    var connection = DbConnection.Connection;
                    var cmd = new MySqlCommand(query, connection);
                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var row = new Dictionary<string, object>();
                        var values = new Object[reader.FieldCount];
                        var fieldCount = reader.GetValues(values);

                        for (int i = 0; i < fieldCount; i++)
                        {
                            var key = reader.GetName(i);
                            var value = values[i];

                            if(value.ToString() == "")
                            {
                                value = null;
                            }

                            row.Add(key, value);
                        }

                        results.Add(row);
                    }

                    reader.Close();
                }
                catch (Exception e)
                {
                    // MySQL Error

                    // Closes the connection.
                    DbConnection.Connection.Close();

                    // Reestablishes connection with new query.
                    ExecuteGetQuery(query);

                    // continue.
                }
            }

            return results;
        }

        #endregion
    }
}
