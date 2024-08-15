using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KanjiGame
{
    [CreateAssetMenu(fileName = "New Quiz Data", menuName = "Quiz Data", order = 51)]
    public class QAContainer : ScriptableObject
    {
        [SerializeField]
        private List<string> questions = new List<string>();

        [SerializeField]
        private List<string> answers = new List<string>();

        // Public read-only properties
        public IReadOnlyList<string> Questions => questions;
        public IReadOnlyList<string> Answers => answers;
    }
}
