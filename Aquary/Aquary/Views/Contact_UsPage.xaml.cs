using Aquary.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace Aquary.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Contact_UsPage : ContentPage
	{
        double latitude, longitude = 0;
        public Contact_UsPage ()
		{
			InitializeComponent ();
            SetupMap();

        }
        #region Map
        public async void SetupMap()
        {
            try
            {

                var addressPosition = new Position();
                latitude = 31.9777706;
                longitude = 35.855822;
                SetAddress(addressPosition);



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                await DisplayAlert("Alert", ex.Message, "OK");
            }
        }

        private async void MapObject_MapClicked(object sender, MapClickedEventArgs e)
        {
            try
            {
                var postion = e.Position;
                latitude = postion.Latitude;
                longitude = postion.Longitude;
                SetAddress(postion);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Alert", ex.Message, "OK");
            }

        }

        public async void AddPins(Position position)
        {
            try
            {
                var pin = new Pin
                {
                    Type = PinType.Place,
                    Position = position,
                    Label = addressEntry.Text
                };

                mapObject.Pins.Add(pin);
                mapObject.MoveToRegion(MapSpan.FromCenterAndRadius(position, new Distance(500)));

            }
            catch (Exception ex)
            {
                await DisplayAlert("Alert", ex.Message, "OK");
            }
        }

        private async void SetAddress(Position p)
        {
            try
            {


                var addrs = (await Geocoding.GetPlacemarksAsync(new Xamarin.Essentials.Location(latitude, longitude))).FirstOrDefault();

                // var addrs = (await Geocoding.GetPlacemarksAsync(new Xamarin.Essentials.Location(p.Latitude, p.Longitude))).FirstOrDefault();
                string Street = $"{addrs.Thoroughfare} {addrs.SubThoroughfare} ";
                string City = $"{addrs.PostalCode} {addrs.Locality} ";
                string Country = addrs.CountryName;

                addressEntry.Text = Street + City + Country;

                AddPins(p);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Alert", ex.Message, "OK");
            }
        }

        private async void SetPinByAddress(string Country)
        {
            var addressPosition = new Position();
            // var addressLocation = await geocoder.GetPositionsForAddressAsync(Country);           
            addressPosition = new Position(31.9777706, 35.855822);

            //  foreach (var position in addressLocation)
            //  {
            //    addressPosition = new Position(position.Latitude, position.Longitude);
            // }

            AddPins(addressPosition);
        }
        #endregion

    }

    class complain_Parameters
    {
        public int register_id { get; set; }
        public string Tittle { get; set; }
        public string description { get; set; }
   

    }
}