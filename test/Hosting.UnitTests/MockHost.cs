namespace Phaka.Hosting
{
    using System;

    internal class MockHost : IMockHost
    {
        public MockHost(IMockService mockService)
        {
            MockService = mockService ?? throw new ArgumentNullException(nameof(mockService));
        }

        public IMockService MockService { get; }
    }
}