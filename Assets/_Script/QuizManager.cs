using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;
using System.ComponentModel;
using System.Xml.Linq;

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

        private List<string> questionsList;
        private readonly List<List<string>> answersList = new();
        private string currentQuestion;
        private int currentIndex = 0;
        // Start is called before the first frame update

        private Dictionary<string, QAContainer> newDict = new();
        [SerializeField]
        private List<Sprite> image;

        private const string API_URL = "https://arondyroz.github.io/KanjiDeTutorial/Assets/Resources/QAData.Json";
        private async void Start()
        {
            await InitializeJson2();

            answerText.text = answersList[currentIndex][0];
            imageBackground.sprite = image[currentIndex];
        }


        // Update is called once per frame
        void Update()
        {
            if (inputAnswer.isFocused && Input.GetKeyDown(KeyCode.Return))
                placeHolder.gameObject.SetActive(false);

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
                    List<string> correctAnswers = newDict[currentQuestion].answers;
                    bool isCorrect = correctAnswers.Exists(answer => answer.ToLower() == userAnswer);
      
                    Debug.Log($"Answer : {newDict[currentQuestion]}, UserAnswer : {userAnswer}");
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

        private IEnumerator ChangeQuestion(float waitTime)
        {
            InputSetEnable(false);
            CorrectAnswerPanelSetActive(true);

            GameManager.Instance.ChangeState(GameState.ShowAnswer);
            yield return new WaitForSeconds(waitTime);

            CorrectAnswerPanelSetActive(false);
            UpdateQuestionIndex();
            UpdateUI();
            InputSetEnable(true);

            inputAnswer.ActivateInputField();
        }

        private void InputSetEnable(bool set) => inputAnswer.enabled = set;

        private void CorrectAnswerPanelSetActive(bool set) => correctAnswerPanel.SetActive(set); 
      
        private void UpdateQuestionIndex()
        {
            if (currentIndex < questionsList.Count - 1)
            {
                currentIndex++;
            }
        }

        private void UpdateUI()
        {
            answerText.text = answersList[currentIndex][0];
            imageBackground.sprite = image[currentIndex];
            currentQuestion = questionsList[currentIndex];
            placeHolder.gameObject.SetActive(true);
            questionText.text = currentQuestion;
            GameManager.Instance.ChangeState(GameState.Quiz);
        }

        private async UniTask InitializeJson2()
        {
            QADataList qaData = await FetchJsonData();
            if (qaData != null)
            {
                foreach(QAContainer qa in qaData.container)
                {
                    Sprite imageSprite = Resources.Load<Sprite>(qa.data.image_path);

                    newDict[qa.question] = qa;
                    image.Add(imageSprite);
                    if (qa.answers != null)
                    {
                        // Convert qa.answers to List<string> and add to answersList
                        List<string> answerList = new(qa.answers);
                       
                        answersList.Add(answerList);
                    }

                }
                Debug.Log(answersList.Count);
                questionsList = new List<string>(newDict.Keys);
            }
            else
            {
                Debug.LogError("Could not find the JSON file.");
            }

            if (questionsList.Count > 0)
            {
                currentQuestion = questionsList[currentIndex];
                questionText.text = currentQuestion;
            }

            inputAnswer.ActivateInputField();
        }


        private async UniTask<QADataList> FetchJsonData()
        {
            try
            {
                using (UnityWebRequest request = UnityWebRequest.Get(API_URL))
                {
                    // Await the completion of the web request
                    var operation = await request.SendWebRequest().ToUniTask();

                    // Check if the request was successful
                    if (request.result == UnityWebRequest.Result.Success)
                    {
                        // Get the JSON response and parse it
                        string json = request.downloadHandler.text;
                        QADataList qaData = JsonUtility.FromJson<QADataList>(json);
                        return qaData;
                    }
                    else
                    {
                        // Log the error and return null
                        Debug.LogError($"Error fetching data: {request.error}");
                        return null;
                    }
                }
            }
            catch (System.Exception ex)
            {
                // Log any exceptions that occur during the request or parsing
                Debug.LogError($"Exception caught while fetching data: {ex.Message}");
                return null;
            }
        }

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
