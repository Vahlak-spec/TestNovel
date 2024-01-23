using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Naninovel
{
    [InitializeAtRuntime]
    public class MinigameManager : ActorManager<IMinigameActor, MinigameManagerState, MinigameManagerMetadata, MinigameManagerConfiguration>, IMinigameManager
    {
        public IResourceLoader<Object> MinigameLoader => _minigameLoader;

        private ResourceLoader<Object> _minigameLoader;

        private readonly IResourceProviderManager _providerManager;
        private readonly ILocalizationManager _localizationManager;

        public MinigameManager(MinigameManagerConfiguration config, IResourceProviderManager providerManager, ILocalizationManager localizationManager)
            : base(config)
        {
            this._providerManager = providerManager;
            this._localizationManager = localizationManager;
        }
        public override async UniTask InitializeServiceAsync()
        {
            await base.InitializeServiceAsync();
            _minigameLoader = Configuration.MinigameLoader.CreateLocalizableFor<Object>(_providerManager, _localizationManager);
        }

        public override void DestroyService()
        {
            base.DestroyService();
            MinigameLoader?.ReleaseAll(this);
        }
    }
}