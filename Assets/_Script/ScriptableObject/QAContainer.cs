using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KanjiGame
{
    //[CreateAssetMenu(fileName = "New Quiz Data", menuName = "Quiz Data", order = 51)]
    [System.Serializable]
    public struct QAContainer
    {
        public string question;
        public List<string> answers;
        public Data data;
    }

    [System.Serializable]
    public struct Data
    {
        public string dalle_prompt;
        public string scene_text_en;
        public string scene_text_jp;
        public string image_path;
        public string word;
    }

    [System.Serializable]
    public class QADataList
    {
        public List<QAContainer> container;
    }
}
