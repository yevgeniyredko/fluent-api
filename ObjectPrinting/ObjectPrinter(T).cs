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
	        
	        if (IsFinalType(obj.GetType()))
	            return PrintFinalObject(obj) + (nestingLevel == 0 ? Environment.NewLine : "");
            
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
	                ? config.PropertySerializers[propertyInfo](propertyInfo.GetValue(obj))
	                : PrintToString(propertyInfo.GetValue(obj), nestingLevel + 1));
	            sb.Append(Environment.NewLine);
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

	    private bool IsFinalType(Type type)
	    {
	        return config.TypeSerializers.ContainsKey(type)
	               || finalTypes.Contains(type)
	               || type.GetProperties().Length == 0;
	    }
	}
}
