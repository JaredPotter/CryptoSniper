using System.IO;
using Newtonsoft.Json;
using System;

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

        #region Methods

        /// <summary>
        ///     Sets the configuration file path to the program data.
        /// </summary>
        /// <param name="path"></param>
        public static void SetConfigPathToProgramData(string path)
        {
            if (path == "" || !File.Exists(path))
            {
                throw new Exception($"Configuration file not specified or does not exist. path: {path}");
            }

            var configString = File.ReadAllText(path);
            var basePath = @"C:\Program Files\CryptoSniper";

            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
            }

            var filePath = basePath + "\\config.json";

            if (!File.Exists(filePath))
            {
                File.Create(filePath);
            }

            File.WriteAllText(filePath, configString);
        }

        /// <summary>
        ///     Loads the configuration data from config.json.
        /// </summary>
        /// <returns>The hydrated configuration singleton.</returns>
        private static ConfigData LoadConfigFile()
        {
            var filePath = @"C:\Program Files\CryptoSniper\config.json";
            ConfigData configData;

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
