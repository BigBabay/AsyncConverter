using System.Collections.Generic;
using AsyncConverter.Highlightings;
using JetBrains.ReSharper.Feature.Services.Bulbs;
using JetBrains.ReSharper.Feature.Services.Intentions;
using JetBrains.ReSharper.Feature.Services.QuickFixes;
using JetBrains.Util;

namespace AsyncConverter.QuickFixes
{
    [QuickFix]
    public class ConfigureAwaitQuickFix : IQuickFix
    {
        private readonly ConfigureAwaitHighlighting configureAwaitHighlighting;

        public ConfigureAwaitQuickFix(ConfigureAwaitHighlighting configureAwaitHighlighting)
        {
            this.configureAwaitHighlighting = configureAwaitHighlighting;
        }


        public bool IsAvailable(IUserDataHolder cache)
        {
            return configureAwaitHighlighting.IsValid();
        }

        public IEnumerable<IntentionAction> CreateBulbItems()
        {
            return new []
                   {
                       new ConfigureAwaitAction(configureAwaitHighlighting, false),
                       new ConfigureAwaitAction(configureAwaitHighlighting, false),
                   }.ToQuickFixIntentions();
        }
    }
}