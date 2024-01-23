using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Naninovel
{
    public interface IScenarioManager : IEngineService<ScenarioManagerConfiguration>
    {
        public void NextStage();
    }
}