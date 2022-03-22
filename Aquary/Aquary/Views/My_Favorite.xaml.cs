using Aquary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Aquary.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class My_Favorite : ContentPage
    {
        RestService _restService;
        public My_Favorite()
        {
            InitializeComponent();
            _restService = new RestService();
           
        }
        protected async override void OnAppearing()
        {
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
            Get_My_FavService(selectedLanguage);
            base.OnAppearing();
           

        }
            public async void Get_My_FavService(string Lang)
        {

            int register_id = Convert.ToInt32(Application.Current.Properties["register_id"]);
            string API = Constants.GitHubReposEndpoint1 + "get_my_fav_ads?register_id=" + register_id + "";
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
        void CollectionViewListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateSelectionData(e.PreviousSelection, e.CurrentSelection);


        }
        void UpdateSelectionData(IEnumerable<object> previousSelectedContact, IEnumerable<object> currentSelectedContact)
        {
            var selectedContact = currentSelectedContact.FirstOrDefault() as Repository;
            Application.Current.Properties["ads_id"] = selectedContact.ads_id;
            Application.Current.Properties["sub_service_id"]= selectedContact.sub_service_id;
            Move_next(selectedContact.code);

        }
        private async void Move_next(string code)
        {
          //  Application.Current.Properties["Is_Favorite"] = 1;
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
}