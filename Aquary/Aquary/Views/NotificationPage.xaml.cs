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
	public partial class NotificationPage : ContentPage
	{
		RestService _restService;
		public NotificationPage ()
		{
			InitializeComponent ();
			_restService = new RestService();
		}

		protected async override void OnAppearing()
		{
			//Write the code of your page here
			base.OnAppearing();
			Get_My_Notification();

		}

		public async void Get_My_Notification()
		{
			try
			{
				int register_id = Convert.ToInt32(Application.Current.Properties["register_id"]);
				if (register_id > 0)
				{
					string API = Constants.GitHubReposEndpoint1 + "get_notification?FK_Register_Id=" + register_id + "";
					List<Repository> repositories = await _restService.GetRepositoriesAsync(API);
					collectionView1.ItemsSource = repositories;
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

		void CollectionViewListSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			UpdateSelectionData(e.PreviousSelection, e.CurrentSelection);


		}
		void UpdateSelectionData(IEnumerable<object> previousSelectedContact, IEnumerable<object> currentSelectedContact)
		{
			var selectedContact = currentSelectedContact.FirstOrDefault() as Repository;
			Application.Current.Properties["ads_id"] = selectedContact.FK_Ads_Id;
			Application.Current.Properties["service_code"] = selectedContact.code;
			Application.Current.Properties["main_service_id"] = selectedContact.fk_main_service;
			Application.Current.Properties["sub_service_id"] = selectedContact.FK_Sub_Service;
			string API = Constants.GitHubReposEndpoint1 + "Notification/Read_Notification";
			 Notification_Read(API);
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

		public async Task<string> Notification_Read(string url)
		{

			var client = new HttpClient();
			Cls_Notification_Read FI = new Cls_Notification_Read();
			FI.FK_Ads_Id = Convert.ToInt32(Application.Current.Properties["ads_id"]);
			FI.FK_Sub_Service = Convert.ToInt32(Application.Current.Properties["sub_service_id"]);
			


			string jsonData = JsonConvert.SerializeObject(FI);
			var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
			HttpResponseMessage response = await client.PostAsync(url, content);
			var result = await response.Content.ReadAsStringAsync();


			Common_Response_API CRA = new Common_Response_API();
			CRA = JsonConvert.DeserializeObject<Common_Response_API>(result);

			if (CRA.result == "success")
			{
				

			}
			else
			{
				await DisplayAlert("Alert", CRA.msg_en, "OK");
			}
			return result;
		}
	}
	class Cls_Notification_Read
	{
		public int FK_Ads_Id { get; set; }
		public int FK_Sub_Service { get; set; }



	}
}