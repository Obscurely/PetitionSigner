using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace PetitionSpammer
{
    public class Config
    {
        // FIELDS
        // READONLY
        private int _threadsNumber;
        private int _proxiesTimeoutMs;
        private string _petitionUrlName;
        private bool _isConfigLoaded;

        // PROPERTIES
        // READONLY
        public int ThreadsNumber { get { return _threadsNumber; } }
        public int ProxiesTimeoutMs { get { return _proxiesTimeoutMs; } }
        public string PetitionUrlName { get { return _petitionUrlName; }}
        public bool IsConfigLoaded { get { return _isConfigLoaded; } }

        /// <summary>
        /// Constructor for config that gets back a parsed config and checked.
        /// </summary>
        /// <param name="configPath">the config file location</param>
        public Config(string configPath)
        {
            // new empty dict to translate the json file to
            Dictionary<string, string> config = new();

            // checks if there is actually a config file and if it's formated as json
            try
            {
                string configText = File.ReadAllText(configPath);
                config = JsonSerializer.Deserialize<Dictionary<string, string>>(configText);
            }
            catch (Exception)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                System.Console.WriteLine("The config.json file either doesn't exist or is not formated as a json file, please check it again or redownload it from the repo.");
                Environment.Exit(100);
            }

            // Check and set threads number from config
            if(!int.TryParse(config["threads_number"], out _threadsNumber))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                System.Console.WriteLine("Invalid threads_number value in config, it has to be a valid number, double your available cpu threads (not cores) for most efficiency" +
                    " (going over will not work or slow the application down)");
                Environment.Exit(100);
            }

            // Check and set proxies timeout ms from config
            if (!int.TryParse(config["proxies_timeout_ms"], out _proxiesTimeoutMs))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                System.Console.WriteLine("You entered an invalid nubmer for proxies_timeout_ms in the config, it has to be a number and in range 50-10000");
                Environment.Exit(100);
            }
            else if (!(_proxiesTimeoutMs >= 50) && !(_proxiesTimeoutMs <= 10000))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                System.Console.WriteLine("The proxies_timeout_ms has to be in range 50-10000, your number was outside the bounds.");
                Environment.Exit(100);
            }

            _petitionUrlName = config["petition_url_name"]; // sets petition url name

            _isConfigLoaded = true; // indicates if the config is fully loaded, useful for extra checks in case there was an unreported exception
        }
    }
}