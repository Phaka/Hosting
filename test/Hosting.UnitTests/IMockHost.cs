namespace Phaka.Hosting
{
    internal interface IMockHost
    {
        IMockService MockService { get; }
    }
}