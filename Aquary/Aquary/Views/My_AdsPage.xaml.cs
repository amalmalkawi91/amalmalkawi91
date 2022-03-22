using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aquary.Models;
using Aquary.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Aquary.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class My_AdsPage : ContentPage
    {
        RestService _restService;
        AppShell _appShell;
      //  MyAdsViewModel _viewModel;
        public My_AdsPage()
        {
            _restService = new RestService();

            InitializeComponent();
           // BindingContext = _viewModel = new MyAdsViewModel();

          
          
        }

        protected async override void OnAppearing()
        {
            //Write the code of your page here
            var selectedLanguage = await Xamarin.Essentials.SecureStorage.GetAsync(Constants.SelctedLanguage);

            if (selectedLanguage is null || selectedLanguage.ToString() == "En")
            {
                //
                Menu_Scroll_Lang.FlowDirection = FlowDirection.LeftToRight;
            }
            else
            {
                Menu_Scroll_Lang.FlowDirection = FlowDirection.RightToLeft;
              
            }
            base.OnAppearing();
            Get_My_AdsService(selectedLanguage);

        }
        public async void Get_My_AdsService(string Lang)
        {
            try
            {
                int register_id = Convert.ToInt32(Application.Current.Properties["register_id"]);
                if (register_id > 0)
                {

                    string API = Constants.GitHubReposEndpoint1 + "get_my_ads?register_id=" + register_id + "";
                  
                    if (Lang == "Ar")
                    {
                        List<Repository> repositories = await _restService.GetRepositoriesAsync(API);
                        collectionView_Ar.ItemsSource = repositories;
                        collectionView_Ar.IsVisible = true;
                        collectionView.IsVisible = false;
                    }
                    else
                    {
                        List<Repository> repositories = await _restService.GetRepositoriesAsync(API);
                        collectionView.ItemsSource = repositories;
                        collectionView_Ar.IsVisible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Alert", "Please Login to continue the process", "OK");
            }
        }
       
        void  CollectionViewListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateSelectionData(e.PreviousSelection, e.CurrentSelection);
          
           
        }
        void UpdateSelectionData(IEnumerable<object> previousSelectedContact, IEnumerable<object> currentSelectedContact)
        {
            var selectedContact = currentSelectedContact.FirstOrDefault() as Repository;
            Application.Current.Properties["ads_id"] = selectedContact.ads_id;
            Application.Current.Properties["service_code"] = selectedContact.code;
            Application.Current.Properties["main_service_id"] = selectedContact.fk_main_service;
            Application.Current.Properties["sub_service_id"] = selectedContact.sub_service_id;
            Move_next(selectedContact.code);
         
        }
        private async void Move_next(string code)
        {
            Application.Current.Properties["Operation_Type"] = "U";
            if (code != "DE")
            {
                await Navigation.PushAsync(new Ads_Form());

            }
            else
            {
                await Navigation.PushAsync(new Ads_Deco_Form());
            }

        }
    }
}