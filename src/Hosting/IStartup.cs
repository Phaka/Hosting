namespace Phaka.Hosting
{
    using System;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Represents a startup class used by a host
    /// </summary>
    public interface IStartup
    {
        /// <summary>
        /// Configures the services.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns>IServiceProvider.</returns>
        IServiceProvider ConfigureServices(IServiceCollection services);

        /// <summary>
        /// Configures the specified service provider.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        void Configure(IServiceProvider serviceProvider);
    }
    
}