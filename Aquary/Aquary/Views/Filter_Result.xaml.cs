using Aquary.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Aquary.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Filter_Result : ContentPage
    {
        RestService _restService;
        public Filter_Result()
        {
            _restService = new RestService();
            InitializeComponent();
        }
        protected async override void OnAppearing()
        {
            //Write the code of your page here
            base.OnAppearing();
            Get_Result();

        }
        public async void Get_Result()
        {
            List<Repository> repositories = null;
            string API = Constants.GitHubReposEndpoint1 + "Filter";

            var client = new HttpClient();
            Filter_Parameters RP = new Filter_Parameters();
            RP.fk_main_service_id = Convert.ToInt32(Application.Current.Properties["main_service_id"]);
            RP.fk_sub_service_id = Convert.ToInt32(Application.Current.Properties["sub_service_id"]);
            RP.fk_area_id = Convert.ToInt32(Application.Current.Properties["City_Id"]);
            RP.fk_region = Convert.ToInt32(Application.Current.Properties["Region_Id"]);
            RP.Price_From = Convert.ToInt32(Application.Current.Properties["Price_From"]);
            RP.Price_To = Convert.ToInt32(Application.Current.Properties["Price_To"]);





            string jsonData = JsonConvert.SerializeObject(RP);
            // string jsonData = @"{""email"" : ""username"", ""password"" :  " + password + "";
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(API, content);

            var result = await response.Content.ReadAsStringAsync();

            
            repositories = JsonConvert.DeserializeObject<List<Repository>>(result);

            collectionView.ItemsSource = repositories;
        }

        void CollectionViewListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateSelectionData(e.PreviousSelection, e.CurrentSelection);
        }
        void UpdateSelectionData(IEnumerable<object> previousSelectedContact, IEnumerable<object> currentSelectedContact)
        {
            var selectedContact = currentSelectedContact.FirstOrDefault() as Repository;
            Application.Current.Properties["ads_id"] = selectedContact.ads_id;
            Application.Current.Properties["service_code"] = selectedContact.code;
            Move_next(selectedContact.code);
        }
        private async void Move_next(string code)
        {
            if (code != "DE")
            {
                await Navigation.PushAsync(new Ads_View_details());

            }
            else
            {
                await Navigation.PushAsync(new Ads_Deco_View_Details());
            }

        }
    }


    class Filter_Parameters
    {
        public int fk_main_service_id { get; set; }
        public int fk_sub_service_id { get; set; }
        public int fk_area_id { get; set; }
        public int fk_region { get; set; }
        public int Price_From { get; set; }
        public int Price_To { get; set; }

    }

}