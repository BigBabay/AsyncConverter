using System;
using AsyncConverter.Highlightings;
using JetBrains.Application.Progress;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Bulbs;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.TextControl;

namespace AsyncConverter.QuickFixes
{
    public class ConfigureAwaitAction : BulbActionBase
    {
        private readonly ConfigureAwaitHighlighting configureAwaitHighlighting;
        private readonly bool configureAwaitValue;

        public ConfigureAwaitAction(ConfigureAwaitHighlighting configureAwaitHighlighting, bool configureAwaitValue)
        {
            this.configureAwaitHighlighting = configureAwaitHighlighting;
            this.configureAwaitValue = configureAwaitValue;
        }
        protected override Action<ITextControl> ExecutePsiTransaction(ISolution solution, IProgressIndicator progress)
        {
            var awaitExpression = configureAwaitHighlighting.AwaitExpression;
            var factory = CSharpElementFactory.GetInstance(awaitExpression);
            var taskWithConfiguring = factory.CreateExpression($"$0.ConfigureAwait({GetConfigureAwaitValueText()})", awaitExpression.Task);
            awaitExpression.Task.ReplaceBy(taskWithConfiguring);
            return null;
        }

        public override string Text => $"Add ConfigureAwait({GetConfigureAwaitValueText()})";

        private string GetConfigureAwaitValueText() => configureAwaitValue ? "true" : "false";
    }
}