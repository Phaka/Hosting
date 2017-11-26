namespace Phaka.Hosting
{
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
}