// See https://aka.ms/new-console-template for more information

using Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

var config = new ConfigurationBuilder()
    .AddJsonFile(path: "application.json", optional: true, reloadOnChange: true)
    .Build();
ChangeToken.OnChange(() => config.GetReloadToken(), () =>
{
    var options = config.GetSection("format").Get<FormatOptions>()!;
    Console.WriteLine(options.DateTime);
    Console.WriteLine(options.CurrencyDecimal);
    Console.WriteLine(options.Center);
    Console.WriteLine(options.Profile);
    Console.WriteLine(options.Profiles[0]);
    Console.WriteLine(options.ProfileMap["angel"]);
});
Console.Read();
    