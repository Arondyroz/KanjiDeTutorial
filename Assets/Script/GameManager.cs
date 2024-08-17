using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

namespace KanjiGame
{
    public enum GameState
    {
        Quiz,
        ShowAnswer,
        AllocatePoint,
        Battle
    }
    public class GameManager : Singleton<GameManager>
    {
        public UnityEvent onStatChanged;
        [Header("UI Ref")]
        [SerializeField]
        private TMP_Text scoreTextQuiz;
        [SerializeField]
        private TMP_Text scoreTextPoint;
        [SerializeField]
        private TMP_Text[] hpText;
        [SerializeField]
        private TMP_Text[] attackText;
        [SerializeField]
        private TMP_Text[] defenseText;

        [Header("Game Data")]
        [SerializeField]
        private GameState gameState;
        [SerializeField]
        private Stats playerData;
        [SerializeField]
        private List<Button> button = new List<Button>();

        public int Score = 0;

        int hpValue;
        int attackValue;
        int defenseValue;

        private void Start()
        {
            hpValue = playerData.hp;
            attackValue = playerData.attack;
            defenseValue = playerData.defense;
        }
        private void Update()
        {
            ChangeScoreUI();
        }

        public void ChangeState(GameState state) => gameState = state;

        public void AddScore(int val) => Score += val;

        public void ChangeScoreUI()
        {
            scoreTextQuiz.text = Score.ToString();
            scoreTextPoint.text = Score.ToString();
        }

        public void AllocateStats(string param)
        {
            Score -= 10;
            switch(param.ToLower())
            {
                case "hp":
                    hpValue += 10;
                    foreach(var text in hpText)
                    {
                        text.text = hpValue.ToString();
                    }
                    break;
                case "attack":
                    attackValue++;
                    foreach (var text in attackText)
                    {
                        text.text = attackValue.ToString();
                    }
                    break;
                case "defense":
                    defenseValue++;
                    foreach (var text in defenseText)
                    {
                        text.text = defenseValue.ToString();
                    }
                    break;
            }

            foreach(var btn in button)
            {
                if(Score <= 0)
                {
                    btn.interactable = false;
                }
            }
        }

    }
}
