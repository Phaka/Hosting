namespace Phaka.Hosting
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Represents a host builder
    /// </summary>
    public abstract class HostBuilder
    {
        /// <summary>
        /// The configure services delegates
        /// </summary>
        private readonly List<Action<IServiceCollection>> _configureServicesDelegates =
            new List<Action<IServiceCollection>>();

        /// <summary>
        /// Builds this instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>T.</returns>
        protected T Build<T>() 
        {
            var hostingServices = new ServiceCollection();
            ConfigureHostingServices(hostingServices);
            var hostingServiceProvider = hostingServices.BuildServiceProvider();

            var startup = hostingServiceProvider.GetRequiredService<IStartup>();
            var applicationServices = hostingServices.Clone();
            var applicationService = startup.ConfigureServices(applicationServices);
            startup.Configure(applicationService);

            var host = ActivatorUtilities.CreateInstance<T>(applicationService);
            return host;
        }

        /// <summary>
        /// Configures the services.
        /// </summary>
        /// <param name="configureServices">The configure services.</param>
        /// <exception cref="ArgumentNullException">configureServices</exception>
        protected internal void ConfigureServices(Action<IServiceCollection> configureServices)
        {
            if (configureServices == null) 
                throw new ArgumentNullException(nameof(configureServices));
            
            _configureServicesDelegates.Add(configureServices);
        }

        /// <summary>
        /// Configures the hosting services.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <exception cref="ArgumentNullException">services</exception>
        private void ConfigureHostingServices(IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            
            services.AddSingleton<IServiceProviderFactory<IServiceCollection>, DefaultServiceProviderFactory>();

            InternalConfigureHostingServices(services);
            
            foreach (var configureServices in _configureServicesDelegates)
                configureServices(services);
        }

        /// <summary>
        /// Configure the hosting services.
        /// </summary>
        /// <param name="services">The services.</param>
        protected virtual void InternalConfigureHostingServices(IServiceCollection services)
        {
        }
    }
}