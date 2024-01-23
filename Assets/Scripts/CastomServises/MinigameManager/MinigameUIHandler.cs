using System.Threading;
using Naninovel.UI;
using UnityEngine;
using DTT.MinigameMemory;

namespace Naninovel
{
    [ActorResources(typeof(MinigameHandlerPanel), false)]
    public class MinigameUIHandler : MonoBehaviourActor<MinigameManagerMetadata>, IMinigameActor
    {
        public override GameObject GameObject => HandlerPanel.gameObject;
        public override string Appearance { get; set; }
        public override bool Visible { get => HandlerPanel.Visible; set => HandlerPanel.Visible = value; }

        private readonly IStateManager stateManager;
        private readonly IUIManager uiManager;
        private readonly IResourceLoader<Object> custopDataLoader;

        private MinigameState curMinigame;

        public MinigameState Minigame
        {
            get
            {
                return curMinigame;
            }
        }
        protected MinigameHandlerPanel HandlerPanel { get; private set; }
        public MinigameUIHandler(string id, MinigameManagerMetadata metadata) : base(id, metadata)
        {
            stateManager = Engine.GetService<IStateManager>();
            uiManager = Engine.GetService<IUIManager>();
            custopDataLoader = Engine.GetService<IMinigameManager>().MinigameLoader;
        }

        public override async UniTask InitializeAsync()
        {
            await base.InitializeAsync();
            GameObject prefab = await LoadUIPrefabAsync();
            Debug.Log("Prefab - " + prefab.name);
            HandlerPanel = await uiManager.AddUIAsync(prefab, group: BuildActorCategory()) as MinigameHandlerPanel;
            //HandlerPanel = uiManager.GetUI(Id) as MinigameHandlerPanel;
            HandlerPanel.AllCardsMatched += CompleteMinigame;
            Debug.Log("Handler - " + HandlerPanel);
            Visible = false;
        }
        protected async UniTask<GameObject> LoadUIPrefabAsync()
        {
            var providerManager = Engine.GetService<IResourceProviderManager>();
            var localizationManager = Engine.GetService<ILocalizationManager>();
            Debug.Log("Id - " + Id);
            //var resource = await ActorMetadata.Loader.CreateLocalizableFor<GameObject>(providerManager, localizationManager).LoadAsync(Id);
            //var resource = await ActorMetadata.Loader.CreateFor<GameObject>(providerManager).LoadAsync(Id);

            var resource = Resources.Load(Id) as GameObject;
            Debug.Log("resource - " + resource);

            return resource;
        }

        private async void CompleteMinigame()
        {
            Visible = false;

            using (var cts = CancellationTokenSource.CreateLinkedTokenSource(GetDestroyCancellationToken()))
            {
                stateManager.OnRollbackStarted += cts.Cancel;
                try { await PlayOnSelectScriptAsync(curMinigame.OnComplete, cts.Token); }
                catch (System.OperationCanceledException) { return; }
                finally
                {
                    if (stateManager != null)
                        stateManager.OnRollbackStarted -= cts.Cancel;
                    cts.Dispose();
                }
            }

            var player = Engine.GetService<IScriptPlayer>();

            var nextIndex = player.PlayedIndex + 1;
            player.Play(player.Playlist, nextIndex);
        }
        protected async UniTask PlayOnSelectScriptAsync(string scriptText, CancellationToken token)
        {
            var script = Script.FromScriptText($"`{Id}` minigame complete", scriptText);
            var playlist = new ScriptPlaylist(script);
            await playlist.ExecuteAsync(token);
            token.ThrowIfCancellationRequested();
        }

        public void SetupMinigame(MinigameState minigame)
        {
            curMinigame = minigame;
            Visible = true;

            var r1 = custopDataLoader.GetLoadedOrNull(minigame.DataPath) ?? Resource<Object>.Invalid;

            if (r1.Valid)
            {
                HandlerPanel.SetupGame((MemoryGameSettings)r1);
            }
            else if (Resources.Load<MemoryGameSettings>(minigame.DataPath) is MemoryGameSettings r2 && r2)
            {
                HandlerPanel.SetupGame((MemoryGameSettings)r2);
            }
        }

        public override UniTask ChangeAppearanceAsync(string appearance, float duration, EasingType easingType = EasingType.Linear, Transition? transition = null, AsyncToken asyncToken = default)
        {
            return UniTask.CompletedTask;
        }

        public override async UniTask ChangeVisibilityAsync(bool isVisible, float duration, EasingType easingType = EasingType.Linear, AsyncToken asyncToken = default)
        {
            if (HandlerPanel)
                await HandlerPanel.ChangeVisibilityAsync(isVisible, duration);
        }

        public void RemoveMinigame(string id)
        {

        }

        protected override Color GetBehaviourTintColor()
        {
            return Color.white;
        }

        protected override void SetBehaviourTintColor(Color tintColor)
        {

        }
    }
}