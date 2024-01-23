using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Naninovel.Commands
{

    [CommandAlias("NextStage")]
    public class NextStage : Command
    {
        public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
        {
            Engine.GetService<IScenarioManager>().NextStage();
            return UniTask.CompletedTask;
        }
    }
}