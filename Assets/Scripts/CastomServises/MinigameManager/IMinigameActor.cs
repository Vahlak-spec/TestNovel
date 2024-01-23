using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Naninovel
{
    public interface IMinigameActor : IActor
    {
        public MinigameState Minigame { get; }

        void SetupMinigame(MinigameState minigame);

        void RemoveMinigame(string id);
    }
}