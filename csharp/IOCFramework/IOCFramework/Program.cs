// See https://aka.ms/new-console-template for more information

using IOCFramework;

using (var root = new Container()
           .Register<IFoo, Foo>(Lifetime.Transient)
           .Register<IBar>(_ => new Bar(), Lifetime.Self)
           .Register<IBaz, Baz>(Lifetime.Root)
           .Register(typeof(Foo).Assembly)
      )
{
    using (var container = root.CreateChild())
    {
        container.GetService<IFoo>();
        container.GetService<IBar>();
        container.GetService<IBaz>();
        container.GetService<IQux>();
        Console.WriteLine("Child cat is disposed.");
    }
    Console.WriteLine("Root cat is disposed.");
}