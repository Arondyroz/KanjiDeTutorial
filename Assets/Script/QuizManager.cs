using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

namespace KanjiGame
{
    public class QuizManager : Singleton<QuizManager>
    {
        [Header("UnityEvents")]
        public UnityEvent onCorrectAnswer;
        public UnityEvent onIncorrectAnswer;
        public UnityEvent onCycleEnds;

        [Header("UI Ref")]
        [SerializeField]
        private TMP_InputField inputAnswer;
        [SerializeField]
        private TMP_Text questionText;
        [SerializeField]
        private TMP_Text placeHolder;
        [SerializeField]
        private GameObject correctAnswerPanel;
        [SerializeField]
        private Image imageBackground;
        [SerializeField]
        private TMP_Text answerText;

        private Dictionary<string, List<string>> quizDictionary = new Dictionary<string, List<string>>();
        private List<string> questionsList;
        private List<List<string>> answersList;
        private List<Sprite> spriteData;
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

            answerText.text = answersList[currentIndex][0];

            CheckAnswer();
            CycleEndsTrigger();
        }
     
        public void CheckAnswer()
        {
            if (!string.IsNullOrWhiteSpace(inputAnswer.text))
            {
                if(Input.GetKeyDown(KeyCode.Return))
                {
                    string userAnswer = inputAnswer.text.ToLower();
                    List<string> correctAnswers = quizDictionary[currentQuestion];
                    bool isCorrect = correctAnswers.Exists(answer => answer.ToLower() == userAnswer);
      
                    Debug.Log($"Answer : {quizDictionary[currentQuestion]}, UserAnswer : {userAnswer}");
                    if (isCorrect)
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

                    StartCoroutine(ChangeQuestion(3f));
                }
            }
        }

        IEnumerator ChangeQuestion(float waitTime)
        {
            inputAnswer.enabled = false;
            correctAnswerPanel.SetActive(true);
            GameManager.Instance.ChangeState(GameState.ShowAnswer);
            yield return new WaitForSeconds(waitTime);

            correctAnswerPanel.SetActive(false);
            if (currentIndex < questionsList.Count - 1)
                currentIndex++;

            GameManager.Instance.ChangeState(GameState.Quiz);
            inputAnswer.enabled = true;
            currentQuestion = questionsList[currentIndex];
            placeHolder.gameObject.SetActive(true);
            questionText.text = currentQuestion;
            inputAnswer.ActivateInputField();
        }

        void InitializeQACollection()
        {
            JSONDataConvert();

            if (questionsList.Count > 0)
            {
                currentQuestion = questionsList[currentIndex];
                questionText.text = currentQuestion;
            }

            inputAnswer.ActivateInputField();
        }

        void JSONDataConvert()
        {
            TextAsset jsonFile = Resources.Load<TextAsset>("QAData");
            if (jsonFile != null)
            {
                QADataList qaData = JsonUtility.FromJson<QADataList>(jsonFile.text);

                if (qaData == null)
                {
                    Debug.LogError("QADataList is null. Check the JSON structure.");
                    return;
                }

                if (qaData.container == null)
                {
                    Debug.LogError("qaData.container is null. Check the JSON structure.");
                    return;
                }
                
                //System.Random rng = new System.Random();
                //int n = qaData.container.Count;
                //while (n > 1)
                //{
                //    n--;
                //    int k = rng.Next(n + 1);
                //    QAContainer value = qaData.container[k];
                //    qaData.container[k] = qaData.container[n];
                //    qaData.container[n] = value;
                //    foreach(var ch in qaData.container)
                //    {
                //        Debug.Log(ch);
                //    }
                //}

                foreach (QAContainer item in qaData.container)
                {
                    quizDictionary.Add(item.question, item.answers);
                    //LoadImageDataJSOn(item.data.image_path);
                }
                
                questionsList = new List<string>(quizDictionary.Keys);
                answersList = new List<List<string>>(quizDictionary.Values);
            }
            else
            {
                Debug.LogError("Could not find the JSON file.");
            }
        }


        public void LoadImageDataJSOn(string imagePath)
        {
            Sprite imageSprite = Resources.Load<Sprite>(imagePath);
            if (imageSprite != null)
            {
                spriteData.Add(imageSprite);
            }
            else
            {
                Debug.LogError("Image not found at path: " + imagePath);
            }
        }
        //public void AllocatePoint()
        public void CycleEndsTrigger()
        {
            if(currentIndex == 5)
            {
                onCycleEnds?.Invoke();
                GameManager.Instance.ChangeState(GameState.AllocatePoint);
                currentIndex = 0;
            }
        }

    }
}
