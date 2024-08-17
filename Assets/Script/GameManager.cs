using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        [SerializeField]
        private GameState gameState;

        public void ChangeState(GameState state)
        {
            gameState = state;
        }
    }
}
