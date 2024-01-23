using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Naninovel
{
    [EditInProjectSettings]
    public class ScenarioManagerConfiguration : Configuration
    {
        [SerializeField] private string stageValueName;
        [SerializeField] private string questValueName;
        [Space]
        [SerializeField] private ScenarioData[] _datas;
        [SerializeField] private string[] _quests;
        [Space]
        [SerializeField] private string _category;

        private ICustomVariableManager _CVmanager;
        private ITextManager _textManager;

        private int Stage
        {
            get
            {
                int.TryParse(Engine.GetService<ICustomVariableManager>().GetVariableValue(stageValueName), out int res);
                Debug.Log("Stage-" + res);
                return res;
            }
        }

        public void InitValues()
        {
            _textManager = Engine.GetService<ITextManager>();
            _CVmanager = Engine.GetService<ICustomVariableManager>();
            _CVmanager.SetVariableValue(questValueName, _quests[Stage]);
        }
        public void NextStage()
        {
            _CVmanager.SetVariableValue(stageValueName, (Stage + 1).ToString());
            _CVmanager.SetVariableValue(questValueName, _quests[Stage]);
        }

        public bool TryGetPath(LocationType type, out string fileName, out string keyName)
        {
            var item = Array.Find(_datas[Stage].Paths, item => item.Type == type);

            fileName = item.FileName;
            keyName = item.KeyName;

            return item.IsOpen;
        }


        [System.Serializable]
        private class ScenarioData
        {
            public Path[] Paths => _paths;

            [SerializeField] private Path[] _paths;

            [System.Serializable]
            public class Path
            {
                public LocationType Type => _type;
                public bool IsOpen => _isOpen;
                public string FileName => _fileName;
                public string KeyName => _keyName;

                [SerializeField] private LocationType _type;
                [SerializeField] private bool _isOpen;
                [SerializeField] private string _fileName;
                [SerializeField] private string _keyName;

            }
        }
    }

    public enum LocationType
    {
        CAPSULE,
        POST,
        STORAGE
    }
}