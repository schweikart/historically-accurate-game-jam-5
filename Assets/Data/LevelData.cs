using System;
using UnityEngine;

namespace Arminius
{
    [CreateAssetMenu(fileName = "NewLevel", menuName = "Arminius/Level Data")]
    public class LevelData : ScriptableObject
    {
        public string LevelName;

        [Serializable]
        public struct ParametrizedGermane
        {
            public GermaneData Germane;
            public int Amount;
        }

        public ParametrizedGermane[] Germanes;
    }
}
