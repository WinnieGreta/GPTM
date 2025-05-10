using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gameplay.Player
{
    [Serializable]
    internal class PlayerHandlingParameters : ISerializationCallbackReceiver
    {
        [Serializable]
        internal class AnchorGroup
        {
            [field: SerializeField] public GameObject GroupParent { get; private set; }
            [field: SerializeField] public List<SpriteRenderer> Sprites { get; private set; }

        }

        [SerializeField] private List<AnchorGroup> _anchorGroups;
        public Dictionary<int, AnchorGroup> AnchorGroups { get; private set; }

        public void OnBeforeSerialize()
        {

        }

        public void OnAfterDeserialize()
        {
            AnchorGroups = _anchorGroups.ToDictionary(x => x.Sprites.Count, x => x);
        }
    }
}