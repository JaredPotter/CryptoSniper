using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoMan.Database
{
    /// <summary>
    ///     The database singleton.
    /// </summary>
    public class DbConnection
    {
        #region Fields

        /// <summary>
        ///     The private database singleton.
        /// </summary>
        private static MySqlConnection _connection;

        /// <summary>
        ///     The publically available database singleton.
        /// </summary>
        public static MySqlConnection Connection
        {
            get
            {
                if (_connection == null || _connection.State == ConnectionState.Closed)
                {
                    // ReSharper disable once ObjectCreationAsStatement
                    new DbConnection();
                }

                return _connection;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        ///     Database connection constructor.
        /// </summary>
        private DbConnection()
        {
            var hostname = "";
            var databaseName = "";
            var username = "";
            var password = "";

            var connectionString = $"Server={hostname}; database={databaseName}; UID={username}; password={password}";
            var connection = new MySqlConnection(connectionString);
            connection.Open();
            _connection = connection;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Closes the database connection.
        /// </summary>
        public void Close()
        {
            _connection.Close();
        }

        #endregion
    }
}
