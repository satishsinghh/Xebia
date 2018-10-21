using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using Xebia.CommonUtility.Interface;

namespace Xebia.CommonUtility
{
    public class VaultConfigProvider : IVaultConfigProvider
    {
        private readonly string _vaultTokenValue;
        private readonly string _vaultUrl;
        private readonly string _dbSecretPath;

        public VaultConfigProvider(string vaultUrl, string dbSecretPath)
        {
            _vaultTokenValue = Environment.GetEnvironmentVariable("VAULT_TOKEN");
            _vaultUrl = vaultUrl;
            _dbSecretPath = dbSecretPath;
        }

        public VaultDataContext GetDatabaseConnectionAsync()
        {
            var requestUrl = string.Format("{0}/v1/{1}", _vaultUrl, _dbSecretPath);
            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            request.Headers.Add("X-Vault-Token", _vaultTokenValue); //pass the token here

            var handler = new HttpClientHandler();

            using (var client = new HttpClient(handler))
            {
                var response = client.SendAsync(request).Result;

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    var jsonResult = JsonConvert.DeserializeObject<dynamic>(result);
                    var vaultModel = new VaultDataContext
                    {
                        ConnectionString = jsonResult.data.connectionString
                    };
                    return vaultModel;
                }
                return null;
            }
        }

        public string GetDatabaseConnectionc()
        {
            string connectionString = "";
            try
            {
                var requestUrl = string.Format("{0}/v1/{1}", _vaultUrl, _dbSecretPath);
                var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
                request.Headers.Add("X-Vault-Token", _vaultTokenValue); //pass the token here

                var handler = new HttpClientHandler();

                using (var client = new HttpClient(handler))
                {
                    var response = client.SendAsync(request).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var result = response.Content.ReadAsStringAsync().Result;
                        var jsonResult = JsonConvert.DeserializeObject<dynamic>(result);

                        connectionString = jsonResult.data.connectionString;
                    }

                    connectionString += "\nIsSuccessStatusCode: " + response.IsSuccessStatusCode;
                    connectionString += "\nStatusCode: " + response.StatusCode;
                    connectionString += "\nReasonPhrase: " + response.ReasonPhrase;
                    connectionString += "\nRequestMessage: " + response.RequestMessage;
                }
            }
            catch (WebException wEx)
            {
                connectionString = wEx.Message;
                if (wEx.InnerException != null)
                {
                    connectionString += " - Inner Message: " + wEx.InnerException;
                }
                if (wEx.Response != null)
                {
                    StreamReader ResponseStream = new StreamReader(wEx.Response.GetResponseStream());
                    connectionString += ResponseStream.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                connectionString = e.Message;
                if (e.InnerException != null)
                {
                    connectionString += " - Inner Message: " + e.InnerException;
                }
            }
            // connectionString = "Test";
            return connectionString;
        }
    }
}
