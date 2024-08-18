using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KanjiGame
{
    [CreateAssetMenu(fileName = "New Stat Data", menuName = "Stat Data", order = 51)]
    public class Stats : ScriptableObject
    {
        [SerializeField]
        public float hp = 50;
        [SerializeField]
        public float attack = 1;
        [SerializeField]
        public float defense = 1;

        //public int HP { get; private set; }

        //public int Attack
        //{
        //    get { return attack; }
        //    set { attack = value; }
        //}

        //public int Defense
        //{
        //    get { return defense; }
        //    set { defense = value; }
        //}
    }
}
