using System.Text;
using UnityEngine;

namespace Naninovel.Commands
{

    [CommandAlias("SummonMinigame")]
    public class SummonMinigame : Command
    {
        [ParameterAlias("handler"), ActorContext(ChoiceHandlersConfiguration.DefaultPathPrefix)]
        public StringParameter HandlerId;

        [ParameterAlias("DataPath")]
        public StringParameter DataPath;

        [ParameterAlias("OnComplete")]
        public StringListParameter OnComplete;

        [ParameterAlias("time"), ParameterDefaultValue("0.35")]
        public DecimalParameter Duration;

        [ParameterAlias("goto"), ResourceContext(ScriptsConfiguration.DefaultPathPrefix, 0)]
        public NamedStringParameter GotoPath;

        private IMinigameManager MinigameManager => Engine.GetService<IMinigameManager>();

        public async UniTask PreloadResourcesAsync()
        {
            if (Assigned(HandlerId) && !HandlerId.DynamicValue)
            {
                var handlerId = Assigned(HandlerId) ? HandlerId.Value : MinigameManager.Configuration.DefaultHandlerId;
                var handler = await MinigameManager.GetOrAddActorAsync(handlerId);
                await handler.HoldResourcesAsync(null, this);
            }

            if (Assigned(DataPath) && !DataPath.DynamicValue)
                await MinigameManager.MinigameLoader.LoadAndHoldAsync(DataPath, this);
        }
        public void ReleasePreloadedResources()
        {
            if (Assigned(HandlerId) && !HandlerId.DynamicValue)
            {
                var handlerId = Assigned(HandlerId) ? HandlerId.Value : MinigameManager.Configuration.DefaultHandlerId;
                if (MinigameManager.ActorExists(handlerId)) MinigameManager.GetActor(handlerId).ReleaseResources(null, this);
            }

            if (Assigned(DataPath) && !DataPath.DynamicValue)
                MinigameManager.MinigameLoader.Release(DataPath, this);
        }

        public override async UniTask ExecuteAsync(AsyncToken asyncToken = default)
        {
            var handlerId = Assigned(HandlerId) ? HandlerId.Value : MinigameManager.Configuration.DefaultHandlerId;
            Debug.Log("handlerId - " + handlerId);
            var minigameActor = await MinigameManager.GetOrAddActorAsync(handlerId);

            Debug.Log("actor - " + minigameActor);

            if (!minigameActor.Visible)
            {
                var duration = Assigned(Duration) ? Duration.Value : MinigameManager.Configuration.DefaultDuration;
                minigameActor.ChangeVisibilityAsync(true, duration, asyncToken: asyncToken).Forget();
            }

            StringBuilder builder = new StringBuilder();

            if (Assigned(GotoPath))
                builder.AppendLine($"{Parsing.Identifiers.CommandLine}{nameof(Goto)} {GotoPath.Name ?? string.Empty}{(GotoPath.NamedValue.HasValue ? $".{GotoPath.NamedValue.Value}" : string.Empty)}");

            if (Assigned(OnComplete))
                foreach (var line in OnComplete)
                    builder.Append(line?.Value?.Trim() ?? string.Empty).Append('\n');

            string onComplete = builder.ToString().TrimFull();

            MinigameState minigame = new MinigameState(DataPath, onComplete);
            minigameActor.SetupMinigame(minigame);
        }
    }
}