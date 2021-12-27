using System;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;

namespace PetitionSpammer
{
    public class PetitionSpammer
    {
        private string _petitionUrl = "https://campaniamea.declic.ro/petitions/$PETITION_NAME/";
        private string _petitionSignUrl = "https://campaniamea.declic.ro/petitions/$PETITION_NAME/signatures";
        private Dictionary<string, string> _requestBody = new()
        {
            { "authenticity_token", "$AUTHENTICITY_TOKEN" },
            { "signature[first_name]", "$FIRST_NAME" },
            { "signature[last_name]", "$LAST_NAME" },
            { "signature[email]", "$EMAIL" },
            { "signature[additional_fields][county]", "$COUNTY" },
            { "signature[eu_data_processing_consent]", "1"},
            { "signature[email_opt_in_type_id]", "807"},
            { "signature[join_organisation]", "false"},
            { "signature[consent_content_version_id]", "722"},
            { "commit", "Semnează"}
        };
        private const string _authenticityTokenPattern = "(?<=content=\"authenticity_token\" />\n.+content=\").+(?=\" />)";
        private const string _agraSessionPattern = "(?<=_agra_session=).+(?=; path=/; secure; HttpOnly; SameSite=Lax)";
        private string _authenticityToken = string.Empty;
        private const string _cookieBase = "agreed_cookies=all; _agra_session=$AGRA_SESSION";
        private string _cookie = string.Empty;
        private const string _firstNamesFileName = "first_names.txt";
        private const string _lastNamesFileName = "last_names.txt";
        private const string _countiesFileName = "counties.txt";
        private string[] _firstNames;
        private string[] _lastNames;
        private string[] _counties;
        private HttpClient _clientWithoutProxy = new();
        private HttpClient _clientWithProxy;
        private HttpClientHandler _clientHandler;
        private readonly WebProxy _proxy = new();

        private string PetitionUrl { get { return _petitionUrl; } }
        private string PetitionSignUrl { get { return _petitionSignUrl; } }
        private Dictionary<string, string> RequestBody { get { return _requestBody; } }
        private string AuthenticityTokenPattern { get { return _authenticityTokenPattern; } }
        private string AgraSessionPattern { get { return _agraSessionPattern; } }
        private string AuthenticityToken { get { return _authenticityToken; } }
        private string CookieBase { get { return _cookieBase; } }
        private string Cookie { get { return _cookie; } }
        private string FirstNamesFileName { get { return _firstNamesFileName; } }
        private string LastNamesFileName { get { return _lastNamesFileName; } }
        private string CountiesFileName { get { return _countiesFileName; } }
        private string[] FirstNames { get { return _firstNames; } }
        private string[] LastNames { get { return _lastNames; } }
        private string[] Counties { get { return _counties; } }
        private HttpClient ClientWithoutProxy { get { return _clientWithoutProxy; } }
        private HttpClient ClientWithProxy { get { return _clientWithProxy; } set { _clientWithProxy = value; } }
        private HttpClientHandler ClientHandler { get { return _clientHandler; } set { _clientHandler = value; } }
        private WebProxy Proxy { get { return _proxy; } }

        /// <summary>
        /// Creates a petition spammer object with the given petition name (name in the url like nu-vrem-teze-anul-asta
        /// in the url https://campaniamea.declic.ro/petitions/nu-vrem-teze-anul-asta)
        /// </summary>
        /// <param name="petitionNameFromUrl">Name in the url like nu-vrem-teze-anul-asta in the url https://campaniamea.declic.ro/petitions/nu-vrem-teze-anul-asta</param>
        public PetitionSpammer(string petitionNameFromUrl, int proxyTimeoutMs)
        {
            _petitionUrl = _petitionUrl.Replace("$PETITION_NAME", petitionNameFromUrl);
            _petitionSignUrl = _petitionSignUrl.Replace("$PETITION_NAME", petitionNameFromUrl);


            // load first names
            _firstNames = File.ReadAllLines(FirstNamesFileName);
            // load last names
            _lastNames = File.ReadAllLines(LastNamesFileName);
            // load counties
            _counties = File.ReadAllLines(CountiesFileName);

            // setup http client for signing
            //  base
            _clientHandler = new() { AllowAutoRedirect = true, AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip, Proxy = _proxy };
            ClientWithProxy = new(ClientHandler);
            ClientWithProxy.Timeout = TimeSpan.FromMilliseconds(proxyTimeoutMs);
            //  headers
            ClientWithProxy.DefaultRequestHeaders.Accept.ParseAdd("text/javascript");
            ClientWithProxy.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");

        }

        /// <summary>
        /// Sets the authenticity token and cookie of the website as an object global variable.
        /// </summary>
        /// <returns>A task which can be used to see if the proccess is done or whatever.</returns>
        private async Task SetAuthenticityTokenAndCookie()
        {
            HttpResponseMessage response = await ClientWithoutProxy.GetAsync(PetitionUrl);

            string responseBody = await response.Content.ReadAsStringAsync(); // response body converted to string.

            // Setting authenticity token.
            _authenticityToken = Regex.Match(responseBody, AuthenticityTokenPattern).ToString();

            // Setting cookie
            string setCookie = response.Headers.GetValues("Set-Cookie").ToArray()[0];
            string agraSession = Regex.Match(setCookie, AgraSessionPattern).ToString();
            _cookie = CookieBase.Replace("$AGRA_SESSION", agraSession);
        }

        /// <summary>
        /// Generates a request body containing the random picked and generated firstName, lastName, email, county and authenticity token
        /// </summary>
        /// <returns>A FormUrlEncodedContent object that represents the request body to be sent.</returns>
        private FormUrlEncodedContent GenRequestBody()
        {
            Person person = new(FirstNames, LastNames, Counties);
            RequestBody["authenticity_token"] = AuthenticityToken;
            RequestBody["signature[first_name]"] = person.FirstName;
            RequestBody["signature[last_name]"] = person.LastName;
            RequestBody["signature[email]"] = person.Email;
            RequestBody["signature[additional_fields][county]"] = person.County;
            FormUrlEncodedContent requestBody = new(RequestBody);

            return requestBody;
        }

        /// <summary>
        /// Changes the http client for the signing with new authenticity token and cookie.
        /// </summary>
        /// <param name="proxy">The proxy to be used with the http client.</param>
        public async Task ChangeClientProxyAndTokens(string proxy)
        {
            // Grabs new authenticity token and cookie.
            await SetAuthenticityTokenAndCookie();

            // change proxy
            ClientWithProxy.CancelPendingRequests();

            // If the proxy is well formated the current proxy gets set if not it skips changing it. (some proxies can have a bad host or not be accepted by C#)
            if (Uri.IsWellFormedUriString($"http://{proxy}", UriKind.Absolute))
            {
                Proxy.Address = new Uri($"http://{proxy}");
            }

            // setup headers
            ClientWithProxy.DefaultRequestHeaders.Remove("Cookie");
            ClientWithProxy.DefaultRequestHeaders.Add("Cookie", Cookie);
            ClientWithProxy.DefaultRequestHeaders.Remove("X-CSRF-Token");
            ClientWithProxy.DefaultRequestHeaders.Add("X-CSRF-Token", AuthenticityToken);
        }

        /// <summary>
        /// Changes the http client for sign in the exact way newhttpclient method does but without setting new authenticity token and cookie.
        /// </summary>
        /// <param name="proxy">The proxy to be used with the http client.</param>
        public void ChangeClientProxy(string proxy)
        {
            // change proxy
            ClientWithProxy.CancelPendingRequests();

            // If the proxy is well formated the current proxy gets set if not it skips changing it. (some proxies can have a bad host or not be accepted by C#)
            if (Uri.IsWellFormedUriString($"http://{proxy}", UriKind.Absolute))
            {
                Proxy.Address = new Uri($"http://{proxy}");
            }

            // setup headers
            ClientWithProxy.DefaultRequestHeaders.Remove("Cookie");
            ClientWithProxy.DefaultRequestHeaders.Add("Cookie", Cookie);
            ClientWithProxy.DefaultRequestHeaders.Remove("X-CSRF-Token");
            ClientWithProxy.DefaultRequestHeaders.Add("X-CSRF-Token", AuthenticityToken);
        }

        /// <summary>
        /// Signs the petition using the given HttpClient instance (configured by the object).
        /// </summary>
        /// <param name="client">HttpClient instance (configured by the object).</param>
        /// <returns>True if petition was signed successfully, false if not.</returns>
        public async Task<bool> SignPetition()
        {
            // modify content and convert request body
            FormUrlEncodedContent requestBody = GenRequestBody();

            // do request
            HttpResponseMessage response = await ClientWithProxy.PostAsync(PetitionSignUrl, requestBody);

            // check response
            string responseContent = await response.Content.ReadAsStringAsync();
            if (response.StatusCode == HttpStatusCode.OK && responseContent.Split("\"action\":\"signed\"").Length >= 2 && responseContent.Split("'Signed Petition'").Length >= 2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}