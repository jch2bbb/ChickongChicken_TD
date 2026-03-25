using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private int maxHealth = 5;

    [Header("References")]
    [SerializeField] private BaseHealthUI healthUI;

    [Header("Lose Popup")]
    [SerializeField] private GameObject losePopupPanel;
    [SerializeField] private GameObject blackBG;

    private int currentHealth;
    private bool isDefeated = false;

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
        if (isDefeated) return;

        currentHealth -= damage;

        if (healthUI != null)
            healthUI.UpdateHearts(currentHealth, maxHealth);

        UnityEngine.Debug.Log("Base Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            isDefeated = true;
            OpenLosePopup();
        }
    }

    private void OpenLosePopup()
    {
        if (losePopupPanel != null)
            losePopupPanel.SetActive(true);

        if (blackBG != null)
            blackBG.SetActive(true);

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClick);

        Time.timeScale = 0f; // Freeze everything in the game
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}