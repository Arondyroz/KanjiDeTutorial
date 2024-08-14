using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KanjiGame
{
    [CreateAssetMenu(fileName = "New Quiz Data", menuName = "Quiz Data", order = 51)]
    public class QAContainer : ScriptableObject
    {
        
        public List<string> questions = new List<string>();
        public List<string> answers = new List<string>();
    }
}
