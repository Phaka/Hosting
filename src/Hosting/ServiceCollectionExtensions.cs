namespace Phaka.Hosting
{
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Represents extension methods for <see cref="ServiceCollection"/>
    /// </summary>
    internal static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Clones the specified service collection.
        /// </summary>
        /// <param name="serviceCollection">The service collection.</param>
        /// <returns>IServiceCollection.</returns>
        public static IServiceCollection Clone(this IServiceCollection serviceCollection)
        {
            IServiceCollection clone = new ServiceCollection();
            foreach (var service in serviceCollection)
                clone.Add(service);
            
            return clone;
        }
    }
}