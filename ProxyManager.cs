using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace PetitionSpammer
{
    public class ProxyManager
    {
        private const string _proxiesUrl = "https://api.proxyscrape.com/v2/?request=getproxies&protocol=http&timeout=$TIMEOUT&country=all&ssl=all&anonymity=elite&simplified=true";
        private const string _proxiesFileName = "proxies";

        public string ProxiesUrl { get { return _proxiesUrl; } }
        public string ProxiesFileName { get { return _proxiesFileName; } }

        /// <summary>
        /// Downloads a list of proxies from "https://api.proxyscrape.com/" using the given timeout and proxyFileId to a file in the current dir named proxies{proxyFileId}.txt
        /// </summary>
        /// <param name="timeout">The timeout a proxy should have.</param>
        /// <param name="proxyFileId">The proxies file id.</param>
        /// <returns>A task which you can be used to check if download is finished.</returns>
        public async Task DownloadProxies(int timeout, int proxyFileId)
        {
            // Proxies file name
            string proxiesFileName = ProxiesFileName + proxyFileId + ".txt";

            // Download proxies
            WebClient client = new();
            await client.DownloadFileTaskAsync(ProxiesUrl.Replace("$TIMEOUT", timeout.ToString()), proxiesFileName);
        }

        /// <summary>
        /// Picks a random proxy from the proxies file with the given and removes it from the file. If there isn't no proxies file with this id or no more proxies in that file
        /// it'll download new proxies to that file (proxies{proxyFileId}.txt).
        /// </summary>
        /// <param name="timeout">The timeout a proxy should have.</param>
        /// <param name="proxyFileId">The proxies file id.</param>
        /// <returns>A string containing the proxy in format ip:port</returns>
        public async Task<string> PickProxy(int timeout, int proxyFileId)
        {
            // Proxies file name
            string proxiesFileName = ProxiesFileName + proxyFileId + ".txt";
            // New random with strong seed
            Random random = new(Guid.NewGuid().GetHashCode());
            // Checks if there isn't a proxies file in dir or the proxies file is empty and if so downloads new proxies.
            string[] textFilesInDir = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.txt");
            bool foundFile = false;
            for (int i = 0; i < textFilesInDir.Length; i++)
            {
                if (Path.GetFileName(textFilesInDir[i]).Equals(proxiesFileName))
                {
                    foundFile = true;
                }
            }
            
            if (!foundFile)
            {
                await DownloadProxies(5000, proxyFileId);
            }

            string[] proxies = await File.ReadAllLinesAsync(proxiesFileName);
            if (proxies.Length == 0)
            {
                await DownloadProxies(5000, proxyFileId);
                proxies = await File.ReadAllLinesAsync(proxiesFileName);
            }

            // Gives user back a proxy and removes it from file.
            string pickedProxy = proxies[random.Next(proxies.Length)];
            List<string> newProxiesList = new();
            newProxiesList.AddRange(proxies);
            newProxiesList.Remove(pickedProxy);

            await File.WriteAllLinesAsync(proxiesFileName, newProxiesList);

            return pickedProxy;
        }
    }
}