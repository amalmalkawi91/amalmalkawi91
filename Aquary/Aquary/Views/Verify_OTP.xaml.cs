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
 
    public partial class Verify_OTP : ContentPage
    {
        public RegisterViewModel model =new RegisterViewModel();
        public Verify_OTP()
        {
            InitializeComponent();
            this.BindingContext = new Verify_Email_1();
          //  Console.WriteLine(model.Name);
           // Code.Text = model.Name;
    
          //  Code.Text = Application.Current.Properties["Register_name"].ToString();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            string API = Constants.GitHubReposEndpoint1 + "verifyotp";
            string result = await verify_1(API);
          //  Navigation.PushAsync(new LoginPage());
        }

        public async Task<string> verify_1(string url)
        {

            var client = new HttpClient();
            Verify_OTP_Parameters VP = new Verify_OTP_Parameters();
            VP.register_id = Convert.ToInt32 (Application.Current.Properties["register_id"]);
            VP.code = Code.Text;
          

            string jsonData = JsonConvert.SerializeObject(VP);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(url, content);

            var result = await response.Content.ReadAsStringAsync();

            // var Get_Result = JsonConvert.DeserializeObject(result);
            Verify_OTP_Result VER = new Verify_OTP_Result();
            VER = JsonConvert.DeserializeObject<Verify_OTP_Result>(result);

            if (VER.result == "success")
            {
                await DisplayAlert("Alert", "The account Verified successfully", "OK");
                await Navigation.PushAsync(new Change_Password());
            }
            else
            {
                await DisplayAlert("Alert", VER.msg_en, "OK");
            }
            return result;
        }

        private void Resend_Clicked_1(object sender, EventArgs e)
        {

        }
    }

    class Verify_OTP_Result
    {

        public int register_id { get; set; }
        public string result { get; set; }
        public int error_code { get; set; }
        public string msg_en { get; set; }
        public string msg_ar { get; set; }

    }
    public class Verify_OTP_1 : BaseViewModel
    {
        public Verify_OTP_1()
        {
            Title = "Verify OTP";
           
        }

    }


    class Verify_OTP_Parameters
    {
        public int register_id { get; set; }
        public string code { get; set; }
       

    }
}