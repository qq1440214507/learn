// See https://aka.ms/new-console-template for more information


using System.Reflection;
using File;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

var assembly = Assembly.GetEntryAssembly()!;
var content =await new ServiceCollection()
    .AddSingleton<IFileProvider>(new EmbeddedFileProvider(assembly))
    .AddSingleton<IFileSystem,FileSystem>()
    .BuildServiceProvider()
    .GetRequiredService<IFileSystem>()
    .ReadAllTextAsync("data.txt");

Console.WriteLine(content);
