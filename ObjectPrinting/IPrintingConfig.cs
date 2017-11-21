using System;
using System.Collections.Generic;
using System.Reflection;

namespace ObjectPrinting
{
    public interface IPrintingConfig
    {
        ICollection<Type> ExcludedTypes { get; }
        ICollection<PropertyInfo> ExcludedProperties { get; }
        IDictionary<Type, Func<object, string>> TypeSerializers { get; }
        IDictionary<PropertyInfo, Func<object, string>> PropertySerializers { get; }
    }
}