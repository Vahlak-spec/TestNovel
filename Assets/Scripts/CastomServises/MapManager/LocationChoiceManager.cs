using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Naninovel
{
    [InitializeAtRuntime]
    public class LocationChoiceManager : ILocationChoiceManager
    {
        private LocationChoicesConfiguration _config;

        public LocationChoicesConfiguration Configuration => _config;

        public LocationChoiceManager(LocationChoicesConfiguration config)
        {
            _config = config;
        }

        public UniTask InitializeServiceAsync()
        {
            return UniTask.CompletedTask;
        }

        public void DestroyService() { }

        public void ResetService() { }
    }
}
