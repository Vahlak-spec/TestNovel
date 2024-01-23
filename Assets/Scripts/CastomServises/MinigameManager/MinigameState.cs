using System;
using System.Collections.Generic;
using UnityEngine;

namespace Naninovel
{
    [Serializable]
    public struct MinigameState : IEquatable<MinigameState>
    {
        public string Id => _id;
        public string DataPath => _dataPath;
        public string OnComplete => _onComplete;

        [SerializeField] private string _id;
        [SerializeField] private string _dataPath;
        [SerializeField] private string _onComplete;

        public MinigameState(string dataPath, string onComplete)
        {
            _id = Guid.NewGuid().ToString();
            _dataPath = dataPath;
            _onComplete = onComplete;
        }

        public bool Equals(MinigameState other)
        {
            return other.Id == _id;
        }

    }
}