using CryptoSniper.Config.Models;

namespace CryptoSniper.Config
{
    /// <summary>
    ///     Configuration data populated from the config.json file.
    /// </summary>
    public class ConfigData
    {
        #region Properties

        /// <summary>
        ///     Configuration for Cex.io API.
        /// </summary>
        public CexIoApiInfo CexIoApiInfo { get; set; }

        /// <summary>
        ///     Configuration for connecting to the database.
        /// </summary>
        public DatabaseConnectionInfo DatabaseConnectionInfo { get; set; }

        #endregion
    }
}
