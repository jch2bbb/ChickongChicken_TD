using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private int hitPoints = 2;
    [SerializeField] private int currencyWorth = 50;

    private bool isDestroyed = false;

    public void TakeDamage(int dmg)
    {
        hitPoints -= dmg;

        if (hitPoints <= 0 && !isDestroyed)
        {
            EnemySpawner.onEnemyDestroy.Invoke();
            LevelManager.main.IncreaseCurrency(currencyWorth);
            isDestroyed = true;

            if (AudioManager.Instance != null)
                AudioManager.Instance.PlaySFX(AudioManager.Instance.enemyDeath);

            Destroy(gameObject);
        }
        else
        {
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlaySFX(AudioManager.Instance.enemyHurt);
        }
    }
}