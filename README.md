# Phaka Hosting

This project was inspired by the ease at which one could create a WebHost in ASP.NET Core and that simplicity was something useful in other classes of applications like services or command line utilities.

## Usage

Create a custom host for your application or service:

```
internal interface IMockHost
{
    IMockService MockService { get; }
}

internal class MockHost : IMockHost
{
    public MockHost(IMockService mockService, ILogger<MockHost> logger)
    {
        MockService = mockService ?? throw new ArgumentNullException(nameof(mockService));
        Logger = logger;
    }

    public IMockService MockService { get; }
    public ILogger Logger { get; }
}
```

The host is using a custom service IMockService, which looks something like this:

```
internal interface IMockService
{
    string Value { get; set; }
}

internal class MockService : IMockService
{
    public string Value { get; set; }
}
```

Create a host builder class; this just wraps the `HostBuilderBase` class.

```
using Phaka.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.ObjectPool;

internal class MockHostBuilder
{
    private readonly HostBuilderBase<IMockHost, MockHost> _hostBuilder;

    public MockHostBuilder()
    {
        _hostBuilder = new HostBuilderBase<IMockHost, MockHost>();
    }

    public IMockHost Build()
    {
        return _hostBuilder.Build();
    }

    public MockHostBuilder UseStartup<T>() where T : class
    {
        _hostBuilder.UseStartup<T>();
        return this;
    }
}
```

Create a startup class

```
using Microsoft.Extensions.DependencyInjection;

internal class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IMockService, MockService>();
    }

    public void Configure(IMockService mockService)
    {
        mockService.Value = "5";
    }
}
```

And configure your entry point:

```
public class Program 
{
    public static void Main() 
    {
        var host = new MockHostBuilder()
            .UseStartup<Startup>()
            .Build();

        Console.WriteLine(host.MockService.Value);
    }
}
```

