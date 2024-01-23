using System.Text;
using UnityEngine;


namespace Naninovel.Commands
{

    [CommandAlias("summonmap")]
    public class SummonMap : Command
    {
        [ParameterAlias("MapID")]
        public IntegerParameter MapId;

        private ILocationChoiceManager MapManager => Engine.GetService<ILocationChoiceManager>();

        private IChoiceHandlerManager ChoiceManager => Engine.GetService<IChoiceHandlerManager>();
        private IScenarioManager ScenarioManager => Engine.GetService<IScenarioManager>();

        public override async UniTask ExecuteAsync(AsyncToken asyncToken = default)
        {
            int mapId = Assigned(MapId) ? MapId : 0;

            int buttonsLenth = MapManager.Configuration.GetLenth(mapId);

            string triggerId;
            string buttPath;
            Vector2 pos;
            LocationType type;
            IChoiceHandlerActor choiceActor;
            StringBuilder builder = new StringBuilder();
            string onSelectScript;


            for (int i = 0; i < buttonsLenth; i++)
            {
                buttPath = MapManager.Configuration.ButtonPath(mapId, i);
                triggerId = MapManager.Configuration.TriggerId(mapId, i);
                pos = MapManager.Configuration.GetPos(mapId, i);
                type = MapManager.Configuration.Location(mapId, i);

                choiceActor = await ChoiceManager.GetOrAddActorAsync(MapManager.Configuration.GetHandlerId(mapId));

                choiceActor.ChangeVisibilityAsync(true, 0.3f, asyncToken: asyncToken).Forget();

                builder.Clear();

                Engine.GetService<IUnlockableManager>().SetItemUnlocked(triggerId,
                    ScenarioManager.Configuration.TryGetPath(type, out string fileName, out string keyName)
                    );

                builder.AppendLine($"{Parsing.Identifiers.CommandLine}{nameof(Goto)} {fileName ?? string.Empty}{(keyName.Length > 0 ? $".{keyName}" : string.Empty)}");


                onSelectScript = builder.ToString().TrimFull();

                var choice = new ChoiceState("", buttPath, pos, onSelectScript, false);
                choiceActor.AddChoice(choice);
            }

        }
    }
}