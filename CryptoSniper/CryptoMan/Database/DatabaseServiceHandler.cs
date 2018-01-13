using CryptoSniper.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

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
        public static void GetAllUsers()
        {
            var query = $"SELECT * FROM User";

            ExecuteInsertUpdateQuery(query);
        }

        public static List<Order> GetAllOrders(int userId)
        {
            var query = $"SELECT * FROM crypto_sniper.`InstantOrder` WHERE user_id = '{userId}';";

            var result = ExecuteGetQuery(query);

            return result;
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

        private static List<Order> ExecuteGetQuery(string query)
        {
            var orders = new List<Order>();

            lock (ThisLock)
            {
                try
                {
                    var connection = DbConnection.Connection;
                    var cmd = new MySqlCommand(query, connection);
                    var reader = cmd.ExecuteReader();
                    
                    while (reader.Read())
                    {
                        var order = new Order();
                        order.OrderId = Convert.ToInt32(reader["order_id"]);
                        order.Completed = Convert.ToBoolean(reader["completed"]);
                        order.BuyDate = (DateTime)reader["buy_date"];
                        order.SellDate = (DateTime)reader["sell_date"];
                        order.UserId = (int)reader["user_id"];
                        order.BuyPrice = Convert.ToDecimal(reader["buy_price"]);
                        order.SellPrice = Convert.ToDecimal(reader["sell_price"]);
                        order.ProfitPercentage = Convert.ToDecimal(reader["profit_percentage"]);
                        order.Amount = Convert.ToDecimal(reader["amount"]);

                        orders.Add(order);
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

            return orders;
        }

        #endregion
    }
}
