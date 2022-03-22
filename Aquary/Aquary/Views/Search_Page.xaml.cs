using Aquary.Models;
using Aquary.ViewModels;
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
	public partial class Search_Page : ContentPage
	{

        RestService _restService;
        private Item _selectedItem;
        Main_ServiceViewModel _viewModel;
        BaseViewModel _BaseViewModel;
        public Search_Page ()
		{
			InitializeComponent ();
            _restService = new RestService();
            Get_City();
             Get_MainService();
            BindingContext = new Search_header();

        }
        protected async override void OnAppearing()
        {
            //Write the code of your page here
            base.OnAppearing();
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

        }
        public async void Get_MainService()
        {
            string API = Constants.GitHubReposEndpoint1 + "get_main_service";
            List<Repository> repositories = await _restService.GetRepositoriesAsync(API);          
            Category_Val.ItemsSource = repositories;
        }
        public async void Get_SubService(int main_service_id)
        {
            //  int main_service_id = Convert.ToInt32(Application.Current.Properties["main_service_id"]);
            string API = Constants.GitHubReposEndpoint1 + "get_sub_service?main_service_Id=" + main_service_id + "";
            List<Repository> repositories = await _restService.GetRepositoriesAsync(API);
            Sub_Category_val.ItemsSource = repositories;
        }
        public async void Get_City()
        {
            string API = Constants.GitHubReposEndpoint1 + "get_city";
            List<Repository> repositories = await _restService.GetRepositoriesAsync(API);
            City_Val.ItemsSource = repositories;

        }


        public async void Get_Region(int City_id)
        {
            // int City_id = Convert.ToInt32(Application.Current.Properties["get_region"]);
            string API = Constants.GitHubReposEndpoint1 + "get_region?city_Id=" + City_id + "";
            List<Repository> repositories = await _restService.GetRepositoriesAsync(API);
            Region_val.ItemsSource = repositories;
        }

        private void Category_Val_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedItem = (Repository)Category_Val.SelectedItem;

            int categ = Convert.ToInt32(selectedItem.main_service_id);
            Application.Current.Properties["main_service_id"] = categ;


            Get_SubService(categ);


        }

        private void Sub_Category_val_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedItem = (Repository)Sub_Category_val.SelectedItem;

            int Sub_categ = Convert.ToInt32(selectedItem.sub_service_id);
            Application.Current.Properties["sub_service_id"] = Sub_categ;

        }



        private void Region_val_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedItem = (Repository)Region_val.SelectedItem;

            int Region_Id = Convert.ToInt32(selectedItem.region_id);
            Application.Current.Properties["Region_Id"] = Region_Id;

        }
        private void City_Val_SelectedIndexChanged(object sender, EventArgs e)
        {

            var selectedItem = (Repository)City_Val.SelectedItem;
            int City_Id = Convert.ToInt32(selectedItem.city_id);
            Application.Current.Properties["City_Id"] = City_Id;

            Get_Region(City_Id);


        }

        private async void Search_Result_Clicked(object sender, EventArgs e)
        {
     
            if (Category_Val.SelectedIndex>-1)
            await Navigation.PushAsync(new Filter_Result());
            else await DisplayAlert("Alert", "Pleas choose Main category", "OK");

        }

        private  void Button_Clicked(object sender, EventArgs e)
        {
			this.Navigation.PopModalAsync();
		}

        private void RESET_Clicked(object sender, EventArgs e)
        {

        }

        private void Slider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            LblSlider_Price_To.Text = String.Format("From = {0}", (int)e.NewValue);
        }

        private void redSlider_ValueChanged(object sender, ValueChangedEventArgs e)
        {

        }

    

        private void Slider_Price_From_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            LblSlider_Price_From.Text= String.Format("To = {0}", (int)e.NewValue);
        }
    }

    class Search_header : BaseViewModel
    {
        private Item _selectedItem;


        public Search_header()
        {
            Title = "FILTER";
            // Items = new ObservableCollection<Item>();
            // LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            // ItemTapped = new Command<Item>(OnItemSelected);

            //AddItemCommand = new Command(OnAddItem);
        }

    }
}