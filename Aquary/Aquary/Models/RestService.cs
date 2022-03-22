using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Aquary.Models
{
    class RestService
    {
        HttpClient _client;

        public RestService()
        {
            _client = new HttpClient();

            if (Device.RuntimePlatform == Device.UWP)
            {
                _client.
                    DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            }
        }

        public async Task<List<Repository>> GetRepositoriesAsync(string uri)
        {
            List<Repository> repositories = null;
            try
            {
                using (HttpClient httpClient = new HttpClient(new HttpClientHandler()))
                {
                    using (HttpResponseMessage response = await httpClient.GetAsync(uri))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string content = await response.Content.ReadAsStringAsync();
                            repositories = JsonConvert.DeserializeObject<List<Repository>>(content);
                        }

                    }
                }
                   // HttpResponseMessage response = await _client.GetAsync(uri);
              
            }
            catch (Exception ex)
            {
              //  await DisplayAlert("Alert", ex.Message, "OK");
              //  Debug.WriteLine("\tERROR {0}", ex.Message);
            }

            return repositories;
        }



        public async Task<string> MakePostRequest(string url, string serializedDataString, bool isJson = true)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            if (isJson)
                request.ContentType = "application/json";
            else
                request.ContentType = "application/x-www-form-urlencoded";

            request.Method = "POST";
            var stream = await request.GetRequestStreamAsync();
            using (var writer = new StreamWriter(stream))
            {
                writer.Write(serializedDataString);
                writer.Flush();
                writer.Dispose();
            }

            var response = await request.GetResponseAsync();
            var respStream = response.GetResponseStream();

            using (StreamReader sr = new StreamReader(respStream))
            {
                return sr.ReadToEnd();
            }
        }

        public async Task<string> login(string url,string username, string password)
        {
            var tokenJson="";
            var client = new HttpClient();
            var content = new StringContent(
                JsonConvert.SerializeObject(new { email = username, password = password }));
            var result = await client.PostAsync(url, content).ConfigureAwait(false);
            if (result.IsSuccessStatusCode)
            {
                 tokenJson = await result.Content.ReadAsStringAsync();
                return tokenJson;
            }
            return tokenJson;
        }




    }
}
