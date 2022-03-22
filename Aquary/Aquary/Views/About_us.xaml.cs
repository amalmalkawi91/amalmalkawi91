using Aquary.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Aquary.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class About_us : ContentPage
    {
        public About_us()
        {
            InitializeComponent();
            Get_About_Us();
        }

        public async void Get_About_Us()
        {
            Deco_Ads_Details_Property DADP = new Deco_Ads_Details_Property();
            var client = new HttpClient();
            //  int main_service_id = Convert.ToInt32(Application.Current.Properties["main_service_id"]);
            string API = Constants.GitHubReposEndpoint1 + "get_company";


            GetRepositoriesAsync1(API);


        }


        public async Task<List<About_Us_Property>> GetRepositoriesAsync1(string uri)
        {

            List<About_Us_Property> repositories = null;
            try
            {
                using (HttpClient httpClient = new HttpClient(new HttpClientHandler()))
                {
                    using (HttpResponseMessage response = await httpClient.GetAsync(uri))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string content = await response.Content.ReadAsStringAsync();
                            repositories = JsonConvert.DeserializeObject<List<About_Us_Property>>(content);


                            company_name.Text = repositories[0].comp_name;
                            aboutus_en.Text = repositories[0].aboutus_en;
                            aboutus_ar.Text = repositories[0].aboutus_ar;
                            address.Text = repositories[0].address;
                            post_office.Text = repositories[0].post_office;
                          
                        }


                    }
                }


            }
            catch (Exception ex)
            {
                Debug.WriteLine("\tERROR {0}", ex.Message);
            }

            return repositories;
        }
    }



    public class About_Us_Property
    {
        #region Common

    

        [JsonProperty("company_name")]
        public string comp_name { get; set; }

        [JsonProperty("post_office")]
        public string post_office { get; set; }

        [JsonProperty("aboutus_ar")]
        public string aboutus_ar { get; set; }

        [JsonProperty("aboutus_en")]
        public string aboutus_en { get; set; }

        [JsonProperty("address")]
        public string address { get; set; }
       

       
        #endregion
    }
}