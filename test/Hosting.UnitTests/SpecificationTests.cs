namespace Phaka.Hosting
{
    using Xunit;

    public class SpecificationTests
    {
        [Fact]
        public void Simple()
        {
            // Act
            var host = new MockHostBuilder()
                .UseStartup<Startup>()
                .Build();
 
             // Assert
             Assert.NotNull(host.MockService);
             Assert.Equal("5", host.MockService.Value);
         }
     }
 }