using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Aquary.Models;
using Aquary.Services;
using Aquary.ViewModels;
using Newtonsoft.Json;
using Xamarin.Forms;


namespace Aquary.Views
{ 
    public partial class Sub_Service : ContentPage
    {
        RestService _restService;
       
  
        public Sub_Service()
        {
            InitializeComponent();
            _restService = new RestService();
            BindingContext = new Sub_Service_header();
          
            int box = 7;
            string API_ads = Constants.GitHubReposEndpoint1 + "get_slider?box_id=" + box;
            GetSliderPhoto(API_ads);

            // BindingContext = new Sub_ServiceViewModel();
        }

        protected async override void OnAppearing()
        {
            //Write the code of your page here
            base.OnAppearing();
            Get_SubService();

        }
        private async void Search_Clicked_1(object sender, EventArgs e)
        {         
            await Navigation.PushAsync(new Search_Page());
        }
        public async void Get_SubService()
        {
            int main_service_id =Convert.ToInt32(Application.Current.Properties["main_service_id"]);
            string API = Constants.GitHubReposEndpoint1 + "get_sub_service?main_service_Id=" + main_service_id + "";
            List<Repository> repositories = await _restService.GetRepositoriesAsync(API);
            collectionView.ItemsSource = repositories;
        }
        void CollectionViewListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateSelectionData(e.PreviousSelection, e.CurrentSelection);
        }
        private async void UpdateSelectionData(IEnumerable<object> previousSelectedContact, IEnumerable<object> currentSelectedContact)
        {
            var selectedContact = currentSelectedContact.FirstOrDefault() as Repository;         
            Application.Current.Properties["sub_service_id"] = selectedContact.sub_service_id;

            var selectedLanguage = await Xamarin.Essentials.SecureStorage.GetAsync(Constants.SelctedLanguage);

            if (selectedLanguage is null || selectedLanguage.ToString() == "En")
            {
                Application.Current.Properties["sub_service_Name"] = selectedContact.service_en;
            }
            else if (selectedLanguage.ToString() == "Ar")
            {
                Application.Current.Properties["sub_service_Name"] = selectedContact.service_ar;

            }
            Move_next(selectedContact.code);          
          
        }

        private async void Move_next(string code)
        {
            int add_ads = Convert.ToInt32(Application.Current.Properties["add_ads_session"]);
            if (add_ads == 1)
            {
                if (code != "DE")
                {
                    Application.Current.Properties["service_code"] = code;
                    await Navigation.PushAsync(new Ads_Form());
                }
                else
                {

                    await Navigation.PushAsync(new Ads_Deco_Form());
                }
            }
            else
            {
                await Navigation.PushAsync(new Active_Ads_View_List());
            }

        }



        public async Task<List<Get_Slider_Property>> GetSliderPhoto(string uri)
        {

            List<Get_Slider_Property> repositories = null;
            try
            {
                using (HttpClient httpClient = new HttpClient(new HttpClientHandler()))
                {
                    using (HttpResponseMessage response = await httpClient.GetAsync(uri))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string content = await response.Content.ReadAsStringAsync();
                            repositories = JsonConvert.DeserializeObject<List<Get_Slider_Property>>(content);
                            collectionView_Photo.ItemsSource = repositories;
                            // latitude.Text = repositories[0].latitude;
                        }


                    }
                }


            }
            catch (Exception ex)
            {
                //Debug.WriteLine("\tERROR {0}", ex.Message);
            }

            return repositories;
        }
    }
    class Sub_Service_header : BaseViewModel
    {
        private Item _selectedItem;


        public Sub_Service_header()
        {
            Title = Application.Current.Properties["main_service_Name"].ToString();
            // Items = new ObservableCollection<Item>();
            // LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            // ItemTapped = new Command<Item>(OnItemSelected);

            //AddItemCommand = new Command(OnAddItem);
        }

    }
    public class Get_Slider_Property
    {
     
        [JsonProperty("image")]
        public string image { get; set; }

       
    }
    
}