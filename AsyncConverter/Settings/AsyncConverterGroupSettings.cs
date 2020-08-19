using JetBrains.ReSharper.Feature.Services.Daemon;

namespace AsyncConverter.Settings
{
    [RegisterConfigurableHighlightingsGroup(Id, Name)]
    public static class AsyncConverterGroupSettings
    {
        public const string Id = "AsyncConverter";
        public const string Name = "Async converter plugin";
    }
}

