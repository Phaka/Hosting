namespace Phaka.Hosting
{
    using System;
    using System.Reflection;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Represents extension methods of <see cref="HostBuilder"/>
    /// </summary>
    public static class HostBuilderExtensions
    {
        /// <summary>
        /// Configures the services.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder">The builder.</param>
        /// <param name="configureServices">The configure services.</param>
        /// <returns>T.</returns>
        public static T ConfigureServices<T>(this T builder, Action<IServiceCollection> configureServices)
            where T : HostBuilder
        {
            builder.ConfigureServices(configureServices);
            return builder;
        }

        /// <summary>
        /// Uses the startup.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder">The builder.</param>
        /// <param name="startupType">Type of the startup.</param>
        /// <returns>T.</returns>
        /// <exception cref="ArgumentNullException">startupType</exception>
        public static T UseStartup<T>(this T builder, Type startupType)
            where T : HostBuilder
        {
            if (startupType == null) 
                throw new ArgumentNullException(nameof(startupType));
            
            builder.ConfigureServices(services =>
            {
                if (typeof(IStartup).GetTypeInfo().IsAssignableFrom(startupType.GetTypeInfo()))
                    services.AddSingleton(typeof(IStartup), startupType);
                else
                    services.AddSingleton(typeof(IStartup), sp => new ConventionBasedStartup(sp, startupType));
            });

            return builder;
        }
    }
}