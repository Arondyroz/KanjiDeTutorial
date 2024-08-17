using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KanjiGame
{
    [CreateAssetMenu(fileName = "New Stat Data", menuName = "Stat Data", order = 51)]
    public class Stats : ScriptableObject
    {
        public float HP = 50;
        public float Attack = 10;
        public float Defense = 5;
    }
}
