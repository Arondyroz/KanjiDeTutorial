using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

namespace KanjiGame
{
    public class QuizManager : MonoBehaviour
    {
        [SerializeField]
        QAContainer qAContainer;

        [Header("UnityEvents")]
        public UnityEvent onCorrectAnswer;
        public UnityEvent onIncorrectAnswer;

        [Header("UI Manager")]
        [SerializeField]
        private TMP_InputField inputAnswer;

        private Dictionary<string, string> quizDictionary = new Dictionary<string, string>();
        private string currentQuestion;
        // Start is called before the first frame update
        void Start()
        {
            
            for (int i = 0; i < qAContainer.questions.Count; i++)
            {
                if (!quizDictionary.ContainsKey(qAContainer.questions[i]))
                {
                    quizDictionary.Add(qAContainer.questions[i],qAContainer.answers[i]);
                }
                else
                {
                    Debug.LogWarning($"The question '{qAContainer.questions[i]}' already exists in the dictionary.");
                }
            }

            currentQuestion = qAContainer.questions[0];
            
        }

        // Update is called once per frame
        void Update()
        {
            CheckAnswer();
        }

        public void CheckAnswer()
        {
            if (!string.IsNullOrWhiteSpace(inputAnswer.text))
            {
                if(Input.GetKeyDown(KeyCode.Return))
                {
                    Debug.Log("Pressed");
                    string userAnswer = inputAnswer.text;
                    if (quizDictionary[currentQuestion] == userAnswer)
                    {
                        onCorrectAnswer?.Invoke();
                        Debug.Log("True");
                    }
                    else
                    {
                        onIncorrectAnswer?.Invoke();
                        Debug.Log("False");
                    }
                }
            }
        }

        public void ReturnInputText()
        {
            inputAnswer.text = string.Empty;
        }
    }
}
