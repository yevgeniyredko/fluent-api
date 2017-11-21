using System.Globalization;

namespace ObjectPrinting
{
    public static class PropertyPrintingConfigExtensions
    {
        public static PrintingConfig<TOwner> WithCulture<TOwner>(
            this PropertyPrintingConfig<TOwner, double> propertyPrintingConfig,
            CultureInfo cultureInfo)
        {
            return propertyPrintingConfig.Using(z => z.ToString(cultureInfo));
        }

        public static PrintingConfig<TOwner> WithCulture<TOwner>(
            this PropertyPrintingConfig<TOwner, int> propertyPrintingConfig,
            CultureInfo cultureInfo)
        {
            return propertyPrintingConfig.Using(z => z.ToString(cultureInfo));
        }

        public static PrintingConfig<TOwner> WithCulture<TOwner>(
            this PropertyPrintingConfig<TOwner, long> propertyPrintingConfig,
            CultureInfo cultureInfo)
        {
            return propertyPrintingConfig.Using(z => z.ToString(cultureInfo));
        }

        public static PrintingConfig<TOwner> TrimmedTo<TOwner>(
            this PropertyPrintingConfig<TOwner, string> propertyPrintingConfig,
            int maxLength)
        {
            return propertyPrintingConfig.Using(s => s.Length > maxLength ? s.Substring(0, maxLength) : s);
        }
    }
}