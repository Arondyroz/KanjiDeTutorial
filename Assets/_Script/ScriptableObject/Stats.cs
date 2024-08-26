using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KanjiGame
{
    [CreateAssetMenu(fileName = "New Stat Data", menuName = "Stat Data", order = 51)]
    public class Stats : ScriptableObject
    {
        public float hp = 50;
        public float attack = 1;
        public float defense = 1;
    }
}
