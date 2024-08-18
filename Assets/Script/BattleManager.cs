using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace KanjiGame
{
    public enum StateTurn
    {
        PlayerTurn,
        EnemyTurn
    }
    public class BattleManager : MonoBehaviour
    {
        public Stats enemyStat;
        [Header("Player Health")]
        public float playerHP;
        public float playerAttack;
        public float playerDefense;
        public HealthBar playerHealthBar;
        public float currentPlayerHealth;
        public TMP_Text playerHealthText;

        [Header("Enemy Health")]
        public float enemyHP;
        public float enemyAttack;
        public float enemyDefense;
        public HealthBar enemyHealthBar;
        public float currentEnemyHealth;
        public TMP_Text enemyHealthText;


        public StateTurn stateTurn;

        private bool isPlayerTurn = true;
        private bool isBattleStarted = false;
        private Coroutine battleCoroutine;

        private void Start()
        {
            stateTurn = StateTurn.PlayerTurn;

            enemyHP = enemyStat.hp;
            enemyAttack = enemyStat.attack;
            enemyDefense = enemyStat.defense;

            currentPlayerHealth = playerHP;
            currentEnemyHealth = enemyHP;
            enemyHealthBar.SetMaxHealth(enemyHP);

            isPlayerTurn = true;
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

            while (currentPlayerHealth > 0f && currentEnemyHealth > 0f)
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
            float damage = Mathf.Max(playerAttack - enemyDefense, 0.3f);
            currentEnemyHealth -= damage;
            enemyHealthText.text = damage.ToString();
            enemyHealthBar.SetHealth(currentEnemyHealth);
            Debug.Log($"Player attacks: {damage} damage. Enemy health: {currentEnemyHealth}");

            yield return new WaitForSeconds(1.5f);

            stateTurn = StateTurn.EnemyTurn;
        }

        IEnumerator EnemyTurn()
        {
            float damage = Mathf.Max(enemyAttack - playerDefense, 0.3f);
            currentPlayerHealth -= damage;
            playerHealthText.text = damage.ToString();
            playerHealthBar.SetHealth(currentPlayerHealth);
            Debug.Log($"Enemy attacks: {damage} damage. Player health: {currentPlayerHealth}");

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
            playerHP = GameManager.Instance.hpValue;
            playerAttack = GameManager.Instance.attackValue;
            playerDefense = GameManager.Instance.defenseValue;
            currentPlayerHealth = playerHP;
            playerHealthBar.SetMaxHealth(playerHP);
        }
    }
}
