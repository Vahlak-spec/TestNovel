using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Naninovel
{
    public class MinigameManagerState : ActorState<IMinigameActor>
    {
        public List<MinigameState> Minigames => new List<MinigameState>(minigames);

        [SerializeField] private List<MinigameState> minigames = new List<MinigameState>();

    }
}