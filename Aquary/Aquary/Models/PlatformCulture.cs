using System;
namespace Aquary.Helper
{
    public class PlatformCulture
    {
        public PlatformCulture(string platformCultureString)
        {
            if (string.IsNullOrEmpty(platformCultureString))
                throw new ArgumentException("Expected culture identifier", nameof(platformCultureString));

            this.PlatformString = platformCultureString.Replace("_", "-"); // .NET expects dash, not underscore

            string[] segments = this.PlatformString.Split('-');

            this.LanguageCode = segments[0];
            this.LocaleCode = segments.Length > 1 ? segments[1] : string.Empty;
        }
        public string PlatformString { get; private set; }
        public string LanguageCode { get; private set; }
        public string LocaleCode { get; private set; }
        public override string ToString()
        {
            return PlatformString;
        }
    }
}
