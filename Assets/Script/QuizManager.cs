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

        [Header("UI Ref")]
        [SerializeField]
        private TMP_InputField inputAnswer;
        [SerializeField]
        private TMP_Text questionText;
        [SerializeField]
        private TMP_Text placeHolder;

        private Dictionary<string, string> quizDictionary = new Dictionary<string, string>();
        private string currentQuestion;
        private int currentIndex = 0;
        // Start is called before the first frame update
        void Start()
        {
            InitializeQACollection();
        }

        // Update is called once per frame
        void Update()
        {
            if (inputAnswer.isFocused && Input.GetKeyDown(KeyCode.Return))
                placeHolder.gameObject.SetActive(false);

            CheckAnswer();
        }

        void InitializeQACollection()
        {
            for (int i = 0; i < qAContainer.Questions.Count; i++)
            {
                if (!quizDictionary.ContainsKey(qAContainer.Questions[i]))
                {
                    quizDictionary.Add(qAContainer.Questions[i], qAContainer.Answers[i]);
                }
                else
                {
                    Debug.LogWarning($"The question '{qAContainer.Questions[i]}' already exists in the dictionary.");
                }
            }

            if (qAContainer.Questions.Count > 0)
            {
                currentQuestion = qAContainer.Questions[currentIndex];
                questionText.text = currentQuestion;
            }

            inputAnswer.ActivateInputField();
        }

        public void CheckAnswer()
        {
            if (!string.IsNullOrWhiteSpace(inputAnswer.text))
            {
                if(Input.GetKeyDown(KeyCode.Return))
                {
                    string userAnswer = inputAnswer.text;
                    Debug.Log($"Dictionary : {quizDictionary[currentQuestion]}, Answer : {userAnswer}");
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

                    inputAnswer.text = string.Empty;

                    ChangeQuestion();
                }
            }
        }

        public void ChangeQuestion()
        {
            if (currentIndex < qAContainer.Questions.Count - 1)
                currentIndex++;

            currentQuestion = qAContainer.Questions[currentIndex];
            placeHolder.gameObject.SetActive(true);
            questionText.text = currentQuestion;
            inputAnswer.ActivateInputField();
        }
    }
}
