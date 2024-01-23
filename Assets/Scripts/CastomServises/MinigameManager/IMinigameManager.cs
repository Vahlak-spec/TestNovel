using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Naninovel
{
    public interface IMinigameManager : IActorManager<IMinigameActor, MinigameManagerState, MinigameManagerMetadata, MinigameManagerConfiguration>
    {
        IResourceLoader<Object> MinigameLoader { get; }
    }
}