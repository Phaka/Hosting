namespace Phaka.Hosting
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Microsoft.Extensions.DependencyInjection;
    using Properties;

    /// <summary>
    /// Represents an implementation of <see cref="IStartup"/> that uses conventions
    /// </summary>
    /// <seealso cref="Phaka.Hosting.StartupBase" />
    public class ConventionBasedStartup : StartupBase
    {
        /// <summary>
        /// The startup type
        /// </summary>
        private readonly Type _startupType;
        /// <summary>
        /// The instance
        /// </summary>
        private readonly object _instance;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConventionBasedStartup"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="startupType">Type of the startup.</param>
        /// <exception cref="ArgumentNullException">
        /// serviceProvider
        /// or
        /// startupType
        /// </exception>
        public ConventionBasedStartup(IServiceProvider serviceProvider, Type startupType)
        {
            if (serviceProvider == null) 
                throw new ArgumentNullException(nameof(serviceProvider));
            
            if (startupType == null) 
                throw new ArgumentNullException(nameof(startupType));

            _instance = ActivatorUtilities.CreateInstance(serviceProvider, startupType);
            _startupType = startupType;
        }

        /// <summary>
        /// Configures the services.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <exception cref="InvalidOperationException">The ConfigureServices method must either be parameterless or take only one parameter of type IServiceCollection.</exception>
        public override void ConfigureServices(IServiceCollection services)
        {
            var methods = _startupType.GetTypeInfo().DeclaredMethods;
            var method = methods.SingleOrDefault(m => m.Name.Equals(nameof(ConfigureServices)));
            if (method == null)
            {
                // There is no method and thus nothing more for us to do
                return;
            }
            
            var parameters = method.GetParameters();
            if (parameters.Length > 1 ||
                parameters.Any(p => p.ParameterType != typeof(IServiceCollection)))
                throw new InvalidOperationException(Resources.ConventionBasedStartup_ConfigureServices_InvalidParameters);

            var arguments = new object[method.GetParameters().Length];
            if (parameters.Length > 0)
                arguments[0] = services;

            method.Invoke(_instance, arguments);
        }

        /// <summary>
        /// Configures the specified builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <exception cref="Exception"></exception>
        public override void Configure(IServiceProvider builder)
        {
            var methods = _startupType.GetTypeInfo().DeclaredMethods;
            var method = methods.SingleOrDefault(m => m.Name.Equals(nameof(Configure)));
            if (method == null)
            {
                // There is no method and thus nothing more for us to do
                return;
            }
            
            using (var scope = builder.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var parameters = method.GetParameters();
                var args = new object[parameters.Length];
                for (var index = 0; index < parameters.Length; index++)
                {
                    var parameter = parameters[index];
                    try
                    {
                        args[index] = serviceProvider.GetRequiredService(parameter.ParameterType);
                    }
                    catch (InvalidOperationException ex)
                    {
                        throw new Exception(string.Format(
                            Resources.ConventionBasedStartup_Configure_UnresolvableService,
                            parameter.ParameterType.FullName,
                            parameter.Name,
                            method.Name,
                            method.DeclaringType.FullName), ex);
                    }
                }
                method.Invoke(_instance, args);
            }
        }
    }
}