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
        ///     Configuration for connecting to the database.
        /// </summary>
        public DatabaseConnectionInfo DatabaseConnectionInfo { get; set; }

        #endregion
    }
}
