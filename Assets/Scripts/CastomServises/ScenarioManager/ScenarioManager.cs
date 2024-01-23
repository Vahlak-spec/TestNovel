using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Naninovel
{
    [InitializeAtRuntime]
    public class ScenarioManager : IScenarioManager
    {
        private ScenarioManagerConfiguration _config;

        public ScenarioManagerConfiguration Configuration => _config;

        public ScenarioManager(ScenarioManagerConfiguration config)
        {
            _config = config;
        }

        public void NextStage() => _config.NextStage();

        public UniTask InitializeServiceAsync()
        {
            _config.InitValues();
            return UniTask.CompletedTask;
        }

        public void DestroyService() { }

        public void ResetService() { }
    }
}