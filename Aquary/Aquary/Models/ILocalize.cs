using System;
using System.Globalization;

namespace Aquary.Helper
{
    public interface ILocalize
    {
        CultureInfo GetCurrentCultureInfo();
        void SetLocale(CultureInfo ci);

    }
}
