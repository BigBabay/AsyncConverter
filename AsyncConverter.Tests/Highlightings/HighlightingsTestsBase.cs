using JetBrains.Annotations;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.FeaturesTestFramework.Daemon;
using JetBrains.ReSharper.TestFramework;

namespace AsyncConverter.Tests.Highlightings
{
    [TestNetCore31]
    public abstract class HighlightingsTestsBase : CSharpHighlightingTestBase
    {
        protected override void DoTestSolution([NotNull] params string[] fileSet)
        {
            ExecuteWithinSettingsTransaction(settingsStore =>
                                             {
                                                 RunGuarded(() => MutateSettings(settingsStore));
                                                 base.DoTestSolution(fileSet);
                                             });
        }

        protected virtual void MutateSettings([NotNull] IContextBoundSettingsStore settingsStore)
        {
        }
    }
}