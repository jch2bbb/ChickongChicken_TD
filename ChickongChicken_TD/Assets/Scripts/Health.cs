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

        UnityEngine.Debug.Log("TakeDamage called. HP left: " + hitPoints);

        if (hitPoints <= 0 && !isDestroyed)
        {
            isDestroyed = true;

            UnityEngine.Debug.Log("Enemy dead. EnemySpawner.main is: " + 
                (EnemySpawner.main != null ? "Found" : "NULL"));

            UnityEngine.Debug.Log("InfiniteWaveUI.main is: " + 
                (InfiniteWaveUI.main != null ? "Found" : "NULL"));

            LevelManager.main.IncreaseCurrency(currencyWorth);

            if (EnemySpawner.main != null)
            {
                EnemySpawner.main.EnemyKilledByTower();
                UnityEngine.Debug.Log("EnemyKilledByTower called successfully");
            }
            else
            {
                UnityEngine.Debug.LogWarning("EnemySpawner.main is NULL — cannot count kill!");
            }

            EnemySpawner.onEnemyDestroy.Invoke();
            Destroy(gameObject);
        }
    }
}