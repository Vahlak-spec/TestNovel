using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Naninovel
{
    [System.Serializable]
    public class MinigameManagerMetadata : ActorMetadata
    {
        [System.Serializable]
        public class Map : ActorMetadataMap<MinigameManagerMetadata> { }

        public MinigameManagerMetadata()
        {
            Implementation = typeof(MinigameUIHandler).AssemblyQualifiedName;
            Loader = new ResourceLoaderConfiguration { PathPrefix = MinigameManagerConfiguration.DefaultPathPrefix };
        }
        public override ActorPose<TState> GetPose<TState>(string poseName) => null;
    }
}
