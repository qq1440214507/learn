using Microsoft.Extensions.Configuration;

namespace Configuration;

public class CurrencyDecimalFormatOptions
{
    public int Digits { get; set; }
    public string Symbol { get; set; }

}