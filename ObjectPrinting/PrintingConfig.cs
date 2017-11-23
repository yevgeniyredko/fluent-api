using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace ObjectPrinting
{
    public class PrintingConfig<TOwner> : IPrintingConfig
    {
        private readonly HashSet<Type> excludedTypes = new HashSet<Type>();
        private readonly HashSet<PropertyInfo> excludedProperties = new HashSet<PropertyInfo>();

        private readonly Dictionary<Type, Func<object, string>> typeSerializers =
            new Dictionary<Type, Func<object, string>>();

        private readonly Dictionary<PropertyInfo, Func<object, string>> propertySerializers =
            new Dictionary<PropertyInfo, Func<object, string>>();

        public PrintingConfig<TOwner> ExcludeType<TPropType>()
        {
            excludedTypes.Add(typeof(TPropType));
            return this;
        }

        public PrintingConfig<TOwner> ExcludeProperty<TPropType>(
            Expression<Func<TOwner, TPropType>> selector)
        {
            var property = ExtractPropertyInfo(selector);
            excludedProperties.Add(property);
            return this;
        }

        public PropertyPrintingConfig<TOwner, TPropType> Print<TPropType>()
        {
            return new PropertyPrintingConfig<TOwner, TPropType>(this);
        }

        public PropertyPrintingConfig<TOwner, TPropType> Print<TPropType>(
            Expression<Func<TOwner, TPropType>> selector)
        {
            var property = ExtractPropertyInfo(selector);
            return new PropertyPrintingConfig<TOwner, TPropType>(this, property);
        }

        private static PropertyInfo ExtractPropertyInfo<TPropType>(
            Expression<Func<TOwner, TPropType>> selector)
        {
            if (selector.Body is MemberExpression memberExpression)
            {
                if (memberExpression.Member is PropertyInfo propertyInfo)
                {
                    return propertyInfo;
                }
            }
            throw new ArgumentException("Selector should be an expression that is " +
                                        "simple property access", nameof(selector));
        }

        ICollection<Type> IPrintingConfig.ExcludedTypes => excludedTypes;
        ICollection<PropertyInfo> IPrintingConfig.ExcludedProperties => excludedProperties;
        IDictionary<Type, Func<object, string>> IPrintingConfig.TypeSerializers => typeSerializers;
        IDictionary<PropertyInfo, Func<object, string>> IPrintingConfig.PropertySerializers => propertySerializers;
    }
}