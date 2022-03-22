using System;
using System.Globalization;
using Aquary.Models;
using Xamarin.Forms;

namespace Aquary.Converters
{
    public class LanguageConverter:IValueConverter
    {
        
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null)
            {
                return string.Empty;
            }
            else
            {
                var selectedLang =  Xamarin.Essentials.SecureStorage.GetAsync(Constants.SelctedLanguage).Result;
                var obj = value as Repository;
                return (selectedLang is null || selectedLang.Equals("En"))?obj.service_en:obj.service_ar;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}