using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Arminius
{
    [CreateAssetMenu(fileName = "NewGermane", menuName = "Arminius/Germane Data")]
    public class GermaneData : ScriptableObject
    {
        public string TypeName;
        public Sprite CardSprite;
        public GameObject FigurePrefab;
    }
}
