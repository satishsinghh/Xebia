using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using Xebia.Client.ApiClient;
using Xebia.Model;

namespace Xebia.Client
{

    public class Assets
    {
        private string _host;
        private string _apiToken;
        public Assets(string host, string apiToken)
        {
            _host = host;
            _apiToken = apiToken;
        }

        public int AddOrUpdateAsset(Asset asset)
        {
            using (var socket = new HttpClientSocket())
            {
                var response = socket.PostAsync(_host, "Asset/AddOrUpdateAsset", _apiToken, asset);
                if (response.IsSuccessStatusCode)
                {
                    string output = response.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<int>(output);
                }
                else
                    return 0;
            }
        }

        public void DeleteAsset(string AssetId)
        {
            var parameters = new NameValueCollection
            {
                { "AssetId",  AssetId }
            };

            using (var socket = new HttpClientSocket())
            {
                socket.SendAsync(_host, "Asset/DeleteAsset", _apiToken, parameters);
            }
        }

        public Asset GetAsset(string AssetId)
        {
            var parameters = new NameValueCollection
            {
                { "AssetId",  AssetId }
            };
            using (var socket = new HttpClientSocket())
            {
                var response = socket.GetAsync(_host, "Asset/GetAsset", _apiToken, parameters);
                if (response.IsSuccessStatusCode)
                {
                    string output = response.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<Asset>(output);
                }
                else
                    return null;
            }
        }

    }
}
