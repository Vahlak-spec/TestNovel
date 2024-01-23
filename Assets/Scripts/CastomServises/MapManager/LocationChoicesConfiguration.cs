using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Naninovel
{
    [EditInProjectSettings]
    public class LocationChoicesConfiguration : Configuration
    {
        public const string DefaultPathPrefix = "ChoiceLocationHandlers";

        [SerializeField]
        private MapData[] _datas;

        public string GetBack(int i) => _datas[i].MapBackground;

        public string GetHandlerId(int i) => _datas[i].HandlerId;
        public int GetLenth(int i) => _datas[i].Buttons.Length;
        public Vector3 GetPos(int i1, int i2) => _datas[i1].Buttons[i2].Pos;
        public string ButtonPath(int i1, int i2) => _datas[i1].Buttons[i2].ButtonId;
        public string TriggerId(int i1, int i2) => _datas[i1].Buttons[i2].TriggerId;
        public LocationType Location(int i1, int i2) => _datas[i1].Buttons[i2].Location;

        [System.Serializable]
        private class MapData
        {
            public string MapBackground => _mapBackground;
            public string HandlerId => _handlerId == "Default" ? Engine.GetService<IChoiceHandlerManager>().Configuration.DefaultHandlerId : _handlerId;
            public MapButton[] Buttons => _buttons;

            [SerializeField] private string _handlerId = "Default";
            [SerializeField] private string _mapBackground;
            [SerializeField] private MapButton[] _buttons;

        }

        [System.Serializable]
        private class MapButton
        {
            public Vector3 Pos => _pos;
            public string ButtonId => _buttonId;
            public string TriggerId => _triggerId;
            public LocationType Location => _location;

            [SerializeField] private Vector3 _pos;
            [SerializeField] private string _buttonId;
            [SerializeField] private string _triggerId;
            [SerializeField] private LocationType _location;
        }
    }
}