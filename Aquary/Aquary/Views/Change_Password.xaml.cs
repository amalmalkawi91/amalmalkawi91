using Aquary.Models;
using Aquary.ViewModels;
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
    public partial class Change_Password : ContentPage
    {
        public Change_Password()
        {
            InitializeComponent();
            this.BindingContext = new Change_Password_1();
        }

        protected async override void OnAppearing()
        {
            //Write the code of your page here
            base.OnAppearing();
            password.Text = "";
            Confirm_password.Text = "";



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

        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (password.Text == "" || Confirm_password.Text == "" )
            {
                await DisplayAlert("Alert", "Please Fill all fields", "OK");

            }
            else if (password.Text != Confirm_password.Text)
            {
                await DisplayAlert("Alert", "Password and confirm password not match", "OK");
            }
            else
            {
                string API = Constants.GitHubReposEndpoint1 + "Change_Password";
                string result = await Change_Pass(API);
            }
           
          
        }

        public async Task<string> Change_Pass(string url)
        {

            var client = new HttpClient();
            Change_Password_Parameters VP = new Change_Password_Parameters();
            VP.register_id = Convert.ToInt32(Application.Current.Properties["register_id"]);
            VP.Password = password.Text;


            string jsonData = JsonConvert.SerializeObject(VP);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(url, content);

            var result = await response.Content.ReadAsStringAsync();

            // var Get_Result = JsonConvert.DeserializeObject(result);
            Common_Result VER = new Common_Result();
            VER = JsonConvert.DeserializeObject<Common_Result>(result);

            if (VER.result == "success")
            {
                await DisplayAlert("Alert", "your Password changed successfully", "OK");
                await Navigation.PushAsync(new LoginPage());
              
            }
            else
            {
                await DisplayAlert("Alert", VER.msg_en, "OK");
            }
            return result;
        }
    }

    class Common_Result
    {

        public int register_id { get; set; }
        public string result { get; set; }
        public int error_code { get; set; }
        public string msg_en { get; set; }
        public string msg_ar { get; set; }

    }
    public class Change_Password_1 : BaseViewModel
    {
        public Change_Password_1()
        {
            Title = "Change password";
          
        }

    }
    class Change_Password_Parameters
    {
        public int register_id { get; set; }
        public string Password { get; set; }


    }
}