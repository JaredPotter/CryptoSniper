using MySql.Data.MySqlClient;
using System;

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

            ExecuteQuery(query);
        }

        /// <summary>
        ///     Executes the query.
        /// </summary>
        /// <param name="query">The query to be executed.</param>
        private static void ExecuteQuery(string query)
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
                    ExecuteQuery(query);

                    // continue.
                }
            }
        }

        #endregion
    }
}
