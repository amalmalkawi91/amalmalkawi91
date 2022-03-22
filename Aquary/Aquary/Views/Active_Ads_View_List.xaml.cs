using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Aquary.Models;
using Aquary.Services;
using Aquary.ViewModels;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Aquary.Views
{

    public partial class Active_Ads_View_List : ContentPage
    {
        RestService _restService;
        int Subbox, box, newCount;
        List<Repository> FirstList = new List<Repository>();
        List<Repository> SecondList = new List<Repository>();
        public Active_Ads_View_List()
        {
            _restService = new RestService();
            InitializeComponent();
         
            BindingContext = new Active_Ads_header();

        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            //collectionView.ScrollTo(monkey, animate: false);
            var selectedLanguage = await Xamarin.Essentials.SecureStorage.GetAsync(Constants.SelctedLanguage); 
            if (selectedLanguage is null || selectedLanguage.ToString() == "En")
            { 
                Scroll_Lang.FlowDirection = FlowDirection.LeftToRight; 
            }
            else
            {
                Scroll_Lang.FlowDirection = FlowDirection.RightToLeft;
            }
            Get_Active_Ads(selectedLanguage);
            if (((App)(App.Current)).Sale_Section_Selected == true)
            {
                Subbox = 3;
                ((App)(App.Current)).Sale_Section_Selected = false;
            }
            else if (((App)(App.Current)).Rent_Section_Selected == true)
            {
                Subbox = 4;
                ((App)(App.Current)).Rent_Section_Selected = false;
            }
            else if (((App)(App.Current)).Decoration_Section_Selected == true)
            {
                Subbox = 5;
                ((App)(App.Current)).Decoration_Section_Selected = false;
            }
            else if (((App)(App.Current)).Maintenance_Section_Selected == true)
            {
                Subbox = 6;
                ((App)(App.Current)).Maintenance_Section_Selected = false;
            }
            string SubAPI_ads = Constants.GitHubReposEndpoint1 + "get_slider?box_id=" + Subbox;
            GetSubSliderPhoto(SubAPI_ads);
           
             box =7;
            string TopAPI_ads = Constants.GitHubReposEndpoint1 + "get_slider?box_id=" + box;
            GetTopSliderPhoto(TopAPI_ads);
        }
        private async void Search_Clicked_1(object sender, EventArgs e)
        {
           
            await Navigation.PushAsync(new Search_Page());

         
        }
        public async void Get_Active_Ads(string Lang)
        {
            int main_service_id = Convert.ToInt32(Application.Current.Properties["main_service_id"]);
            int sub_service_id = Convert.ToInt32(Application.Current.Properties["sub_service_id"]);

            if (Lang=="Ar")
            {
                string API = Constants.GitHubReposEndpoint1 + "Get_Active_Adv?main_service_Id=" + main_service_id + " &sub_service_Id=" + sub_service_id;
                List<Repository> repositories = await _restService.GetRepositoriesAsync(API);

               // collectionView_Ar.ItemsSource = repositories;
                collectionView_Ar.IsVisible = true;
                collectionView.IsVisible = false;
                collectionViewRest.IsVisible = false;
               
                
                
                 newCount = repositories.Count;
                for (int i = 0; i < newCount; i++)
                {
                    if (i < 5)
                    {
                        FirstList.Add(repositories[i]); 
                    }
                    else
                    {
                        SecondList.Add(repositories[i]);
                    }
                }
                collectionView.ItemsSource = FirstList;
                collectionViewRest.ItemsSource = SecondList;
            }
            else
            {
                string API = Constants.GitHubReposEndpoint1 + "Get_Active_Adv?main_service_Id=" + main_service_id + " &sub_service_Id=" + sub_service_id;
                List<Repository> repositories = await _restService.GetRepositoriesAsync(API);
                if (repositories.Count >= 3)
                {
                    newCount = repositories.Count;
                    for (int i = 0; i < newCount; i++)
                    {
                        if (i < 3)
                        {
                            FirstList.Add(repositories[i]);
                        }
                        else
                        {
                            SecondList.Add(repositories[i]);
                        }
                    }
                    collectionView.ItemsSource = FirstList;
                    collectionViewRest.ItemsSource = SecondList;
                    // collectionView.ItemsSource = repositories;
                    collectionView_Ar.IsVisible = false;

                }
                else
                {
                    collectionView.ItemsSource = repositories;
                    collectionView.VerticalOptions = LayoutOptions.Start ;
                    collectionView_Ar.IsVisible = false;
                    collectionViewRest.IsVisible = false;
                    collectionViewSub_Photo.IsVisible = true;
                    collectionViewSub_Photo.VerticalOptions = LayoutOptions.Start;
                }
            }
          

           // string Code = Application.Current.Properties["service_code"].ToString();
           


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
            if (code!= "DE")
             {
                await Navigation.PushAsync(new Ads_View_details());

            }
            else
            {
                await Navigation.PushAsync(new Ads_Deco_View_Details());
            }
          
        }

        private void collectionView_Scrolled(object sender, ItemsViewScrolledEventArgs e)
        {

        }

        public async Task<List<Get_Slider_Property>> GetSubSliderPhoto(string uri)
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
                            collectionViewSub_Photo.ItemsSource = repositories;
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
        public async Task<List<Get_Slider_Property>> GetTopSliderPhoto(string uri)
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
                            collectionViewTop_Photo.ItemsSource = repositories;
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

    class Active_Ads_header : BaseViewModel
    {
        private Item _selectedItem;


        public Active_Ads_header()
        {
            Title = Application.Current.Properties["sub_service_Name"].ToString();
            // Items = new ObservableCollection<Item>();
            // LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            // ItemTapped = new Command<Item>(OnItemSelected);

            //AddItemCommand = new Command(OnAddItem);
        }

    }
}