using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Graphics.Drawables;
using CustomRendererDemo;
using CustomRendererDemo.Droid;
using Android.Views;
using Android.Graphics;

[assembly: ExportRenderer(typeof(RoundedPicker), typeof(RoundedPickerRendererAndroid))]
namespace CustomRendererDemo.Droid
{
    class RoundedPickerRendererAndroid : PickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);

            var picker = e.NewElement;
       

         
            if (e.OldElement == null)
            {
                //Control.SetBackgroundResource(Resource.Layout.rounded_shape);
                var gradientDrawable = new GradientDrawable();
                gradientDrawable.SetCornerRadius(60f);
                gradientDrawable.SetStroke(0, Android.Graphics.Color.Gray);
                gradientDrawable.SetColor(Android.Graphics.Color.White);
                Control.SetBackground(gradientDrawable);

                Control.SetPadding(50, Control.PaddingTop, Control.PaddingRight,
                    Control.PaddingBottom);
            }
        }
    }
}