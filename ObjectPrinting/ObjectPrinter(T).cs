using System;
using System.Linq;
using System.Text;

namespace ObjectPrinting
{
    public class ObjectPrinter<T> : ObjectPrinter
	{
	    private readonly IPrintingConfig config;

	    public ObjectPrinter(IPrintingConfig config)
	    {
	        this.config = config;
	    }

	    public string PrintToString(T obj)
	    {
	        return PrintToString(obj, 0);
	    }

	    private string PrintToString(object obj, int nestingLevel)
	    {
	        if (obj == null)
	            return "null" + Environment.NewLine;
	        
	        if (IsValueType(obj.GetType()))
	            return PrintFinalObject(obj) + Environment.NewLine;
            
	        var identation = new string('\t', nestingLevel + 1);
	        var sb = new StringBuilder();
	        var type = obj.GetType();

	        sb.AppendLine(type.Name);
	        foreach (var propertyInfo in type.GetProperties())
	        {
                if (config.ExcludedProperties.Contains(propertyInfo)
                    || config.ExcludedTypes.Contains(propertyInfo.PropertyType))
                    continue;

	            sb.Append(identation + propertyInfo.Name + " = ");
	            sb.Append(config.PropertySerializers.ContainsKey(propertyInfo)
	                ? config.PropertySerializers[propertyInfo](propertyInfo.GetValue(obj)) + Environment.NewLine
	                : PrintToString(propertyInfo.GetValue(obj), nestingLevel + 1));
	        }
	        return sb.ToString();
	    }

	    private string PrintFinalObject(object obj)
	    {
	        var type = obj.GetType();
	        return config.TypeSerializers.ContainsKey(type)
	            ? config.TypeSerializers[type](obj)
	            : obj.ToString();
	    }

	    private bool IsValueType(Type type)
	    {
	        return config.TypeSerializers.ContainsKey(type)
	               || valueTypes.Contains(type)
	               || type.GetProperties().Length == 0;
	    }
	}
}
