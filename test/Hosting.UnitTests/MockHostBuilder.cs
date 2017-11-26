namespace Phaka.Hosting
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.ObjectPool;

    internal class MockHostBuilder : HostBuilder
    {
        protected override void InternalConfigureHostingServices(IServiceCollection services)
        {
            var config = new ConfigurationBuilder()
                .AddEnvironmentVariables("MOCK_")
                .Build();

            services.AddSingleton<IConfiguration>(config);
            services.AddOptions();
            services.AddLogging();
            services.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();
            
            base.InternalConfigureHostingServices(services);
        }

        public MockHost Build()
        {
            return base.Build<MockHost>();
        }
    }

    internal static class MockHostBuilderExtensions
    {
        public static MockHostBuilder UseStartup<TStartup>(this MockHostBuilder builder)
        {
            return builder.UseStartup(typeof(TStartup));
        }
    }
}