using System;

namespace ObjectPrinting
{
    public abstract class ObjectPrinter
    {
        public static ObjectPrinter<TPropType> For<TPropType>()
        {
            return new ObjectPrinter<TPropType>(new PrintingConfig<TPropType>());
        }

        public static ObjectPrinter<TPropType> For<TPropType>(
            Func<PrintingConfig<TPropType>, PrintingConfig<TPropType>> configurator)
        {
            var config = configurator(new PrintingConfig<TPropType>());
            return new ObjectPrinter<TPropType>(config);
        }

        protected static readonly Type[] valueTypes =
        {
            typeof(int), typeof(double), typeof(float), typeof(string),
            typeof(DateTime), typeof(TimeSpan)
        };
    }
}