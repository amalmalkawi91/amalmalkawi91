using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;
using Map = Xamarin.Forms.Maps.Map;

namespace Aquary.Views
{
     public partial class MAP : ContentPage
    {
        Geocoder geocoder;
        public MAP()
        {
            InitializeComponent();
            geocoder = new Geocoder();
            SetupMap();
        }
        public void SetupMap()
        {
            try
            {

                addressEntry.Text = "Jordan";

                SetPinByAddress();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void BtnClear_Clicked(object sender, EventArgs e)
        {
            mapObject.Pins.Clear();
            addressEntry.Text = "";
        }

        private void BtnAdd_Clicked(object sender, EventArgs e)
        {
            SetPinByAddress();
        }

        private void MapObject_MapClicked(object sender, MapClickedEventArgs e)
        {
            var postion = e.Position;
            SetAddress(postion);
        }

        public void AddPins(Position position)
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

        private async void SetAddress(Position p)
        {
            var addrs = (await Geocoding.GetPlacemarksAsync(new Location(p.Latitude, p.Longitude))).FirstOrDefault();
            string Street = $"{addrs.Thoroughfare} {addrs.SubThoroughfare} ";
            string City = $"{addrs.PostalCode} {addrs.Locality} ";
            string Country = addrs.CountryName;

            addressEntry.Text = Street + City + Country;

            AddPins(p);
        }

        private async void SetPinByAddress()
        {
            var addressPosition = new Position();
            //var addressLocation = await geocoder.GetPositionsForAddressAsync(addressEntry.Text);

            // foreach (var position in addressLocation) {
            //   addressPosition = new Position(position.Latitude, position.Longitude);
            // }
            addressPosition = new Position(32.021165, 35.844742);
            AddPins(addressPosition);
        }

    }
}