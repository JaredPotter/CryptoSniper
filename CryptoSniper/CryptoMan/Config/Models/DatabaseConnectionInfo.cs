namespace CryptoSniper.Config
{
    /// <summary>
    ///     Basic database information for json de-serialization. 
    /// </summary>
    public class DatabaseConnectionInfo
    {
        #region Properties

        /// <summary>
        ///     The database hostname.
        /// </summary>
        public string Hostname { get; set; }

        /// <summary>
        ///     The database name.
        /// </summary>
        public string Database { get; set; }

        /// <summary>
        ///     The database username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     The database password.
        /// </summary>
        public string Password { get; set; }

        #endregion
    }
}