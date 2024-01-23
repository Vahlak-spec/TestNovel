using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Naninovel
{
    [EditInProjectSettings]
    public class MinigameManagerConfiguration : ActorManagerConfiguration<MinigameManagerMetadata>
    {
        public const string DefaultPathPrefix = "MinigameHandlers";


        [Tooltip("ID of the choice handler to use by default.")]
        public string DefaultHandlerId = "MathMinigame";
        [Tooltip("Configuration of the resource loader used for loading custom choice buttons.")]
        public ResourceLoaderConfiguration MinigameLoader = new ResourceLoaderConfiguration();

        public MinigameManagerMetadata DefaultMetadata = new MinigameManagerMetadata();

        public MinigameManagerMetadata.Map Metadata = new MinigameManagerMetadata.Map
        {
            ["MathMinigame"] = CreateBuiltinMeta()
        };

        public override MinigameManagerMetadata DefaultActorMetadata => DefaultMetadata;

        public override ActorMetadataMap<MinigameManagerMetadata> ActorMetadataMap => Metadata;

        protected override ActorPose<TState> GetSharedPose<TState>(string poseName) => null;

        private static MinigameManagerMetadata CreateBuiltinMeta() => new MinigameManagerMetadata
        {
            Implementation = typeof(MinigameUIHandler).AssemblyQualifiedName
        };

    }
}