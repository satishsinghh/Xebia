using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Http;
using System.Text;
using System.Linq;
using System.Web;
using System.Net;

namespace Xebia.Client.ApiClient
{
    public class HttpClientSocket : IDisposable
    {
        public HttpResponseMessage SendAsync(string host, string apiName, string apiToken, object parameters)
        {
            string url = host + "/" + apiName.ToLower();
            using (var httpClient = new HttpClient())
            {
                if (parameters != null && parameters.GetType().Name == "NameValueCollection")
                    url += ToQueryString((NameValueCollection)parameters);

                var httpRequest = GetRequest(url, HttpMethod.Post, apiToken);

                httpRequest.Content = new ObjectContent(parameters.GetType(), parameters, new System.Net.Http.Formatting.JsonMediaTypeFormatter());
                return httpClient.SendAsync(httpRequest).Result;
            }
        }

        public HttpResponseMessage GetAsync(string host, string apiName, string apiToken, object parameters)
        {
            using (var httpClient = new HttpClient())
            {
                string url = host + "/" + apiName.ToLower();
                if (parameters != null && parameters.GetType().Name == "NameValueCollection")
                    url += ToQueryString((NameValueCollection)parameters);

                httpClient.DefaultRequestHeaders.Add("X-API-TOKEN", apiToken);
                httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                return httpClient.GetAsync(url).Result;
            }
        }

        public HttpResponseMessage PostAsync(string host, string apiName, string apiToken, object value)
        {
            string url = host + "/" + apiName.ToLower();
            using (var httpClient = new HttpClient())
            {
                HttpContent content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json");
                httpClient.DefaultRequestHeaders.Add("X-API-TOKEN", apiToken);
                var postTask = httpClient.PostAsync(url, content);
                postTask.Wait();

                return postTask.Result;
            }
        }

        public void Dispose()
        {

        }

        #region Private Methods

        private HttpRequestMessage GetRequest(string requestUrl, HttpMethod method, string apiToken)
        {
            var request = new HttpRequestMessage(method, requestUrl);
            request.Headers.Add("User-Agent", "Tmp.Jd.DatabaseCore.ApiClient");
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("X-API-TOKEN", apiToken);

            return request;
        }

        private string ToQueryString(NameValueCollection nvc)
        {
            var array = (from key in nvc.AllKeys
                         from value in nvc.GetValues(key)
                         select string.Format("{0}={1}", WebUtility.UrlEncode(key), WebUtility.UrlEncode(value)))
                .ToArray();
            return "?" + string.Join("&", array);
        }

        #endregion Private Methods
    }
}
