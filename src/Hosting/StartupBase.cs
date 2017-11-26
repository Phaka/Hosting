namespace Phaka.Hosting
{
    using System;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Represents a base implementaton of <see cref="IStartup"/>
    /// </summary>
    /// <seealso cref="Phaka.Hosting.IStartup" />
    public abstract class StartupBase : IStartup
    {
        /// <summary>
        /// Configures the specified service provider.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        public abstract void Configure(IServiceProvider serviceProvider);

        /// <summary>
        /// Configures the services.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns>IServiceProvider.</returns>
        IServiceProvider IStartup.ConfigureServices(IServiceCollection services)
        {
            ConfigureServices(services);
            return CreateServiceProvider(services);
        }

        /// <summary>
        /// Configures the services.
        /// </summary>
        /// <param name="services">The services.</param>
        public virtual void ConfigureServices(IServiceCollection services)
        {
        }

        /// <summary>
        /// Creates the service provider.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns>IServiceProvider.</returns>
        public virtual IServiceProvider CreateServiceProvider(IServiceCollection services)
        {
            return services.BuildServiceProvider();
        }
    }
}