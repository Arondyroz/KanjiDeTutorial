using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KanjiGame
{
    //[CreateAssetMenu(fileName = "New Quiz Data", menuName = "Quiz Data", order = 51)]
    [System.Serializable]
    public class QAContainer
    {
        public string question;
        public List<string> answers;

        //// Public read-only properties
        //public IReadOnlyList<string> Questions => questions;
        //public IReadOnlyList<string> Answers => answers;
    }

    [System.Serializable]
    public class QADataList
    {
        public List<QAContainer> container;
    }
}
