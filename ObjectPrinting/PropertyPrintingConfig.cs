using System;
using System.Reflection;

namespace ObjectPrinting
{
    public class PropertyPrintingConfig<TOwner, TPropType>
    {
        private readonly PrintingConfig<TOwner> printingConfig;
        private readonly PropertyInfo property;

        public PropertyPrintingConfig(PrintingConfig<TOwner> printingConfig)
        {
            this.printingConfig = printingConfig;
        }

        public PropertyPrintingConfig(PrintingConfig<TOwner> printingConfig, PropertyInfo property)
        {
            this.printingConfig = printingConfig;
            this.property = property ?? throw new ArgumentNullException(nameof(property));
        }

        public PrintingConfig<TOwner> Using(Func<TPropType, string> serializer)
        {
            Func<object, string> objectSerializer = obj => serializer((TPropType)obj);
            if (property == null)
            {
                ((IPrintingConfig)printingConfig).TypeSerializers.Add(typeof(TPropType), objectSerializer);
            }
            else
            {
                ((IPrintingConfig)printingConfig).PropertySerializers.Add(property, objectSerializer);
            }
            return printingConfig;
        }
    }
}