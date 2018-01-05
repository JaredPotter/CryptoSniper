using System.IO;
using Newtonsoft.Json;

namespace CryptoSniper.Config
{
    /// <summary>
    ///     Manages the configuration singleton hydrated with config.json.
    /// </summary>
    public static class Configuration
    {
        /// <summary>
        ///     Private configuration singleton.
        /// </summary>
        private static ConfigData _instance;

        /// <summary>
        ///     Publically availabile configuration singleton.
        /// </summary>
        public static ConfigData ConfigData => _instance ?? (_instance = LoadConfigFile());

        /// <summary>
        ///     The configuration file path.
        /// </summary>
        public static string ConfigurationFilePath { get; set; }

        #region Methods

        /// <summary>
        ///     Sets the configuration file path to the program data.
        /// </summary>
        /// <param name="path"></param>
        public static void SetConfigPathToProgramData(string path)
        {
            Properties.Settings.Default["configPath"] = path;
            Properties.Settings.Default.Save();
        }

        /// <summary>
        ///     Loads the configuration data from config.json.
        /// </summary>
        /// <returns>The hydrated configuration singleton.</returns>
        private static ConfigData LoadConfigFile()
        {
            ConfigData configData;
            var filePath = "";

            filePath = Properties.Settings.Default["configPath"].ToString();

            using (StreamReader r = new StreamReader(filePath))
            {
                string json = r.ReadToEnd();
                configData = JsonConvert.DeserializeObject<ConfigData>(json);
            }

            return configData;
        }

        #endregion
    }
}
