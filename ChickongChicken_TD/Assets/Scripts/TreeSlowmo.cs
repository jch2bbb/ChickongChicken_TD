using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class TreeSlowmo : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject upgradeUI;
    [SerializeField] private Button upgradeButton;

    [Header("Attribute")]
    [SerializeField] private float targetingRange = 5f;
    [SerializeField] private float aps = 4f;
    [SerializeField] private float freezeTime = 1f;
    [SerializeField] private int baseUpgradeCost = 150;
    [SerializeField] private int upgradeCostIncrement = 50;

    private float targetingRangeBase;
    private float apsBase;
    private float freezeTimeBase;
    private float timeUntilFire;
    private int level = 1;

    private void Start()
    {
        targetingRangeBase = targetingRange;
        apsBase = aps;
        freezeTimeBase = freezeTime;

        if (upgradeButton != null)
            upgradeButton.onClick.AddListener(Upgrade);
        else
            UnityEngine.Debug.LogWarning("Upgrade Button not assigned on TreeSlowmo!");
    }

    private void Update()
    {
        timeUntilFire += Time.deltaTime;

        if (timeUntilFire >= 1f / aps)
        {
            FreezeEnemies();
            timeUntilFire = 0f;
        }
    }

    private void FreezeEnemies()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange,
            Vector2.zero, 0f, enemyMask);

        if (hits.Length > 0)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit2D hit = hits[i];
                EnemyMovement em = hit.transform.GetComponent<EnemyMovement>();
                if (em != null)
                {
                    em.UpdateSpeed(0.5f);
                    StartCoroutine(ResetEnemySpeed(em));
                }
            }
        }
    }

    private IEnumerator ResetEnemySpeed(EnemyMovement em)
    {
        yield return new WaitForSeconds(freezeTime);
        if (em != null)
        {
            em.ResetSpeed();
        }
    }

    public void OpenUpgradeUI()
    {
        if (upgradeUI != null)
            upgradeUI.SetActive(true);
    }

    public void CloseUpgradeUI()
    {
        if (upgradeUI != null)
            upgradeUI.SetActive(false);
        UIManager.main.SetHoveringState(false);
    }

    public void Upgrade()
    {
        // Always play button click first
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClick);

        if (CalculateCost() > LevelManager.main.currency)
        {
            UpgradeUIHandler upgradeUIHandler = upgradeUI.GetComponent<UpgradeUIHandler>();
            if (upgradeUIHandler != null)
            {
                upgradeUIHandler.ShowCantAffordUpgrade();
            }
            return;
        }

        LevelManager.main.SpendCurrency(CalculateCost());
        level++;
        aps = CalculateAPS();
        targetingRange = CalculateRange();
        freezeTime = CalculateFreezeTime();
        CloseUpgradeUI();

        // Play upgrade sound after successful upgrade
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX(AudioManager.Instance.towerUpgrade);
    }

    public int GetUpgradeCost()
    {
        return CalculateCost();
    }

    private int CalculateCost()
    {
        return baseUpgradeCost + (level - 1) * upgradeCostIncrement;
    }

    private float CalculateAPS()
    {
        return apsBase * Mathf.Pow(level, 0.6f);
    }

    private float CalculateRange()
    {
        return targetingRangeBase * Mathf.Pow(level, 0.4f);
    }

    private float CalculateFreezeTime()
    {
        return freezeTimeBase + (level - 1) * 0.5f;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
    }
#endif
}