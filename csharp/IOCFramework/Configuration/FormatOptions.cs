using Microsoft.Extensions.Configuration;

namespace Configuration;

public class FormatOptions
{
    public DateTimeFormatOptions DateTime { get; set; }
    public CurrencyDecimalFormatOptions CurrencyDecimal { get; set; }

    public Point Center { get; set; }

    public Profile Profile { get; set; }
    
    public IList<Profile> Profiles { get; set; }
    
    public IDictionary<string,Profile> ProfileMap { get; set; }
}