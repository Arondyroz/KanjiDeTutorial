using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Cysharp.Threading.Tasks;
using UnityEngine.TextCore.Text;

namespace KanjiGame
{
    public enum StateTurn
    {
        PlayerTurn,
        EnemyTurn
    }
    public class BattleManager : MonoBehaviour
    {
        [SerializeField] private Stats playerStatsSO;
        [SerializeField] private Stats enemyStatsSO;
        [SerializeField] private HealthBar playerHealthBar;
        [SerializeField] private HealthBar enemyHealthBar;
        [SerializeField] private TMP_Text playerHealthText;
        [SerializeField] private TMP_Text enemyHealthText;

        private Entity player;
        private Entity enemy;

        public StateTurn stateTurn;

        private bool isBattleStarted = false;
        private Coroutine battleCoroutine;

        private void Start()
        {
            stateTurn = StateTurn.PlayerTurn;

            player = new Entity(playerStatsSO, playerHealthBar, playerHealthText);
            enemy = new Entity(enemyStatsSO, enemyHealthBar, enemyHealthText);

        }

        private void Update()
        {
            if (GameManager.Instance.gameState == GameState.Battle && !isBattleStarted)
            {
                isBattleStarted = true;
                StartBattle().Forget();
            }
        }

        private async UniTaskVoid StartBattle()
        {
            await UniTask.Delay(1000); // 1 second delay
            Debug.Log("Battle started");

            while (player.CurrentHealth > 0f && enemy.CurrentHealth > 0f)
            {
                if (stateTurn == StateTurn.PlayerTurn)
                {
                    await PlayerTurn();
                }
                else
                {
                    await EnemyTurn();
                }
            }

            Debug.Log("Battle ended");
            await SceneManager.LoadSceneAsync(0);
        }

        private async UniTask PlayerTurn()
        {
            float damage = Mathf.Max(player.GetAttackDamage() - enemy.GetDefense(), 1f);
            enemy.TakeDamage(damage);
            Debug.Log($"Player attacks: {damage} damage. Enemy health: {enemy.CurrentHealth}");

            await UniTask.Delay(1500); // 1.5 seconds delay

            stateTurn = StateTurn.EnemyTurn;
        }

        private async UniTask EnemyTurn()
        {
            float damage = Mathf.Max(enemy.GetAttackDamage() - player.GetDefense(), 1f);
            player.TakeDamage(damage);
            Debug.Log($"Enemy attacks: {damage} damage. Player health: {player.CurrentHealth}");

            await UniTask.Delay(1500); // 1.5 seconds delay

            stateTurn = StateTurn.PlayerTurn;
        }
        public void ChangeState(StateTurn state) => stateTurn = state;
    }
}
