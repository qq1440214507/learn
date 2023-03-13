using System.ComponentModel;
using System.Globalization;

namespace Configuration;
[TypeConverter(typeof(PointTypeConverter))]
public readonly record struct Point(double x, double y);

public class PointTypeConverter : TypeConverter
{
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
    {
        return sourceType == typeof(string);
    }

    public override object ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        var split = (value.ToString() ?? "0.0,0.0").Split(",");
        var x = double.Parse(split[0].Trim().TrimStart('('));
        var y = double.Parse(split[1].Trim().TrimEnd(')'));
        return new Point(x, y);
    }
}