using AsyncConverter.Settings;
using JetBrains.ReSharper.Feature.Services.Daemon;

[assembly: RegisterConfigurableHighlightingsGroup(AsyncConverterGroupSettings.Id, AsyncConverterGroupSettings.Name)]

namespace AsyncConverter.Settings
{
    public static class AsyncConverterGroupSettings
    {
        public const string Id = "AsyncConverter";
        public const string Name = "Async converter plugin";
    }
}

