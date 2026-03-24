using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Base : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private int maxHealth = 5;

    [Header("References")]
    [SerializeField] private BaseHealthUI healthUI;

    private int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;

        if (healthUI != null)
            healthUI.UpdateHearts(currentHealth, maxHealth);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            TakeDamage(1);

            EnemyMovement em = other.GetComponent<EnemyMovement>();
            if (em != null)
            {
                em.ReachedBase();
            }
            else
            {
                Destroy(other.gameObject);
            }
        }
    }

    private void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (healthUI != null)
            healthUI.UpdateHearts(currentHealth, maxHealth);

        UnityEngine.Debug.Log("Base Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            SceneManager.LoadScene("Lose_Scene");
        }
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}