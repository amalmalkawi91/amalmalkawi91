using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Aquary.Models;
using Aquary.Services;
using Aquary.ViewModels;
using Xamarin.Forms;


namespace Aquary.Views
{

    public partial class Main_Service_add_ads : ContentPage
    {
        int add_ads_session;
        RestService _restService;
        private Item _selectedItem;
        Main_ServiceViewModel _viewModel;
        BaseViewModel _BaseViewModel;
        public Main_Service_add_ads()
        {
            InitializeComponent();
            _restService = new RestService();
            NavigationPage.SetHasNavigationBar(this, false);

        }

        protected async override void OnAppearing()
        {
            //Write the code of your page here
            base.OnAppearing();

            try
            {
                int register_id = Convert.ToInt32(Application.Current.Properties["register_id"]);
                if (register_id > 0)
                {
                    Application.Current.Properties["add_ads_session"] = 1;
                    Application.Current.Properties["Operation_Type"] = "I";
                    Get_MainService();
                    BindingContext = new Main_ServiceViewModel();
                    add_ads_session = 0;
                }
                else
                {
                    var result = await this.DisplayAlert("Alert!", "Please Login to continue the process?", "Login", "Cancel");

                    if (result)
                    {
                        Navigation.PushAsync(new LoginPage());
                    }
                    else
                    {
                        App.CurrentSelectedIndex = 0;
                        await Navigation.PopToRootAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                var result = await this.DisplayAlert("Alert!", "Please Login to continue the process?", "Login", "Cancel");

                if (result)
                {
                    Navigation.PushAsync(new LoginPage());
                }
                else
                {
                    App.CurrentSelectedIndex = 0;
                    await Navigation.PopToRootAsync();
                }
            }
         

        }
        public async void Get_MainService()
        {
            string API = Constants.GitHubReposEndpoint1 + "get_main_service";
            List<Repository> repositories = await _restService.GetRepositoriesAsync(API);
            collectionView.ItemsSource = repositories;
        }
       
        private async void OnLoginClicked(object obj)
        {
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            await Shell.Current.GoToAsync(nameof(NewItemPage));
        }       
        void CollectionViewListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateSelectionData(e.PreviousSelection, e.CurrentSelection);         

        }
        private async void  UpdateSelectionData(IEnumerable<object> previousSelectedContact, IEnumerable<object> currentSelectedContact)
        {
            var selectedContact = currentSelectedContact.FirstOrDefault() as Repository;
            Application.Current.Properties["main_service_id"] = selectedContact.main_service_id;


            var selectedLanguage = await Xamarin.Essentials.SecureStorage.GetAsync(Constants.SelctedLanguage);
            if (selectedLanguage is null || selectedLanguage.ToString() == "En")
            {
                Application.Current.Properties["main_service_Name"] = selectedContact.service_en;
            }
            else if (selectedLanguage.ToString() == "Ar")
            {
                Application.Current.Properties["main_service_Name"] = selectedContact.service_ar;
            }
            if (selectedContact.service_en == "Property For sale")
            {
                ((App)(App.Current)).Sale_Section_Selected = true;
                ((App)(App.Current)).Rent_Section_Selected = false;
            }
            else if (selectedContact.service_en == " Property For Rent")
            {
                ((App)(App.Current)).Rent_Section_Selected = true;
                ((App)(App.Current)).Sale_Section_Selected = false;
            }
            else if (selectedContact.service_en == " Decoration and design")
            {
                ((App)(App.Current)).Decoration_Section_Selected = true;
            }
            else if (selectedContact.service_en == "Construction and maintenance")
            {
                ((App)(App.Current)).Maintenance_Section_Selected = true;
            }
            Move_next();
        }

        private async void Move_next()
        {
            // await Shell.Current.GoToAsync("//Sub_Service", false);
            await Navigation.PushAsync(new Sub_Service());
            //DateTime yesterday = DateTime.Now.AddDays(-1).Date;
        }

      
    }
}