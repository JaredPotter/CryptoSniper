using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoMan.Database
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
        ///     Inserts a record into the databse indicating a file has been removed from the RabbitMQ input queue and saved to either the 'validate' or 'translate' folder.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="hostname">The originating Tibco machine processign the file.</param>
        //public static void InsertDequeuedFileEntry(string filename, string hostname)
        //{
        //    var now = DateTime.UtcNow;
        //    var query = $"INSERT INTO tibco_queue_file (filename, hostname, date_time, status) VALUES ('{filename}', '{hostname}', '{now:yyyy-MM-dd HH:mm:ss}', '{Status.Dequeued.ToString()}')";

        //    ExecuteQuery(query);
        //}


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
                    // Reestablishing connection 

                    DbConnection.Connection.Close();
                    ExecuteQuery(query);
                    // continue.
                }
            }
        }

        #endregion
    }
}
