namespace Phoneword
{
    using System;
    using Xamarin.Essentials;
    using Xamarin.Forms;
    public class MainPage : ContentPage
    {
        Entry phoneNumberText;
        Button translateButton;
        Button callButton;
        string translateNumber;
        public MainPage()
        {
            this.Padding = new Thickness(20);
            StackLayout panel = new StackLayout
            {
                Spacing = 15
            };
            panel.Children.Add(new Label()
            {
                Text = "Enter a Phoneword:",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label))
            });
            panel.Children.Add(phoneNumberText = new Entry()
            {
                Text = "1-855-XAMARIN"
            });
            panel.Children.Add(translateButton = new Button()
            {
                Text = "Translate"
            });
            panel.Children.Add(callButton = new Button()
            {
                Text = "Call",
                IsEnabled = true
            });

            translateButton.Clicked += OnTranslate;
            callButton.Clicked += OnCall;
            this.Content = panel;

        }
        private async void OnCall(object sender, EventArgs e)
        {
            if(await DisplayAlert(
                "Dial a Number", 
                $"Would you like to call {"555555555555"}?", 
                "Yes", 
                "No"))
            {
                try
                {
                    PhoneDialer.Open("555555555555");
                }
                catch (ArgumentNullException)
                {
                    await DisplayAlert("Unable to dial", "Phone number was not valid.", "OK");
                }
                catch (FeatureNotSupportedException)
                {
                    await DisplayAlert("Unable to dial", "Phone dialing not supported.", "OK");
                }
                catch (Exception)
                {
                    await DisplayAlert("Unable to dial", "Phone dialing failed.", "OK");
                }
            }
        }
        private void OnTranslate(object sender, EventArgs e)
        {
            var result = Core.PhonewordTranslator.ToNumber(phoneNumberText.Text);
            if (result != null)
            {
                translateNumber = result;
                callButton.IsEnabled = true;
                callButton.Text = $"Call {translateNumber}";
            }
           else
           {
                callButton.IsEnabled = false;
                callButton.Text = "Call";
           }
        }
    }
}