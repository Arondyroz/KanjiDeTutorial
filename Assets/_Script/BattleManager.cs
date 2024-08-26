using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
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
                battleCoroutine = StartCoroutine(StartBattle());
            }
        }

        IEnumerator StartBattle()
        {
            yield return new WaitForSeconds(1f);
            Debug.Log("Battle started");

            while (player.CurrentHealth > 0f && enemy.CurrentHealth > 0f)
            {
                if (stateTurn == StateTurn.PlayerTurn)
                {
                    yield return StartCoroutine(PlayerTurn());
                }
                else
                {
                    yield return StartCoroutine(EnemyTurn());
                }
            }

            Debug.Log("Battle ended");
            SceneManager.LoadScene(0);
        }

        IEnumerator PlayerTurn()
        {
            float damage = Mathf.Max(player.GetAttackDamage() - enemy.GetDefense(), 1f);
            enemy.TakeDamage(damage);
            Debug.Log($"Player attacks: {damage} damage. Enemy health: {enemy.CurrentHealth}");

            yield return new WaitForSeconds(1.5f);
            //playerHealthBar.SetMaxHealth(player.CurrentHealth);

            stateTurn = StateTurn.EnemyTurn;
        }

        IEnumerator EnemyTurn()
        {
            float damage = Mathf.Max(enemy.GetAttackDamage() - player.GetDefense(), 1f);
            player.TakeDamage(damage);
            Debug.Log($"Enemy attacks: {damage} damage. Player health: {player.CurrentHealth}");

            yield return new WaitForSeconds(1.5f);

            stateTurn = StateTurn.PlayerTurn;
        }

        private void OnDisable()
        {
            if (battleCoroutine != null)
            {
                StopCoroutine(battleCoroutine);
            }
        }
        public void ChangeState(StateTurn state) => stateTurn = state;

        public void ChangePlayerHp()
        {
            //playerHP = GameManager.Instance.hpValue;
            //playerAttack = GameManager.Instance.attackValue;
            //playerDefense = GameManager.Instance.defenseValue;
            //currentPlayerHealth = playerHP;
            //playerHealthBar.SetMaxHealth(playerHP);
        }
    }
}
