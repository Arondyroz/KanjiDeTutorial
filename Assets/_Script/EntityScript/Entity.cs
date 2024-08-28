using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace KanjiGame
{
    public class Entity 
    {
        public readonly Stats stats;
        public readonly HealthBar healthBar;
        public readonly TMP_Text healthText;
        public float CurrentHealth { get; private set; }
        public float GetAttackDamage() => stats.attack;
        public float GetDefense() => stats.defense;

        public Entity(Stats stats, HealthBar healthBar, TMP_Text healthText)
        {
            this.stats = stats;
            this.healthBar = healthBar;
            this.healthText = healthText;
            CurrentHealth = stats.hp;
            healthBar.SetMaxHealth(CurrentHealth);
        }

        public void TakeDamage(float damage)
        {
            float statDamage = Mathf.Max(damage - stats.defense, 1f);
            float actualDamage = Mathf.Ceil(Random.Range(statDamage, statDamage * 0.7f));
            CurrentHealth = Mathf.Max(CurrentHealth - actualDamage, 0f);
            UpdateHealthDisplay(CurrentHealth);
        }

        private void UpdateHealthDisplay(float currHealth)
        {
            healthBar.SetHealth(currHealth);
            healthText.text = currHealth.ToString();
        }
    }
}
