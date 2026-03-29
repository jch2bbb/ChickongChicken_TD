using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Tutorial : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject[] enemyPrefabs;

    [Header("Attributes")]
    [SerializeField] private int baseEnemies = 4;
    [SerializeField] private float enemiesPerSecond = 0.5f;
    [SerializeField] private float timeBetweenWaves = 5f;
    [SerializeField] private float difficultyScalingFactor = 0.75f;
    [SerializeField] private float enemiesPerSecondCap = 15f;

    [Header("Victory")]
    [SerializeField] private int enemiesToKill = 4;

    [Header("Victory Popup")]
    [SerializeField] private GameObject victoryPopupPanel;
    [SerializeField] private GameObject blackBG;

    [Header("Wave Popup")]
    [SerializeField] private GameObject wavePopupPanel;

    [Header("Tutorial Popup (Step 1)")]
    [SerializeField] private GameObject tutorialPopupPanel;

    [Header("Health Popups (Step 2)")]
    [SerializeField] private GameObject healthBlackBG;
    [SerializeField] private GameObject healthDescriptionPopup;
    [SerializeField] private GameObject healthDescriptionPopup1;

    [Header("Shop Popups (Step 3)")]
    [SerializeField] private GameObject shopBlackBG;
    [SerializeField] private GameObject shopPopupPanel;

    [Header("Upgrade Popup (Step 4 - closed by button)")]
    [SerializeField] private GameObject upgradePopupPanel;

    private int currentWave = 1;
    private float timeSinceLastSpawn;
    private int enemiesAlive;
    private int enemiesLeftToSpawn;
    private float eps;
    private bool isSpawning = false;
    private int enemiesKilled = 0;
    private bool gameOver = false;
    private bool upgradePopupClosed = false;
    private bool waveEndHandled = false;

    private void Awake()
    {
        // Listen to the same event EnemySpawner uses so enemy scripts work unchanged
        EnemySpawner.onEnemyDestroy.RemoveAllListeners();
        EnemySpawner.onEnemyDestroy.AddListener(EnemyDestroyed);
    }

    private void Start()
    {
        Time.timeScale = 1f;
        StartCoroutine(RunTutorialSequence());
    }

    // ---------------------------------------------------------------
    // TUTORIAL SEQUENCE
    // ---------------------------------------------------------------
    private IEnumerator RunTutorialSequence()
    {
        // Step 1: Tutorial intro (3 sec)
        if (blackBG != null) blackBG.SetActive(true);
        if (tutorialPopupPanel != null) tutorialPopupPanel.SetActive(true);
        yield return new WaitForSeconds(3f);
        if (blackBG != null) blackBG.SetActive(false);
        if (tutorialPopupPanel != null) tutorialPopupPanel.SetActive(false);
        yield return new WaitForSeconds(0.5f);

        // Step 2: Health popups (5 sec)
        if (healthBlackBG != null) healthBlackBG.SetActive(true);
        if (healthDescriptionPopup != null) healthDescriptionPopup.SetActive(true);
        if (healthDescriptionPopup1 != null) healthDescriptionPopup1.SetActive(true);
        yield return new WaitForSeconds(5f);
        if (healthBlackBG != null) healthBlackBG.SetActive(false);
        if (healthDescriptionPopup != null) healthDescriptionPopup.SetActive(false);
        if (healthDescriptionPopup1 != null) healthDescriptionPopup1.SetActive(false);
        yield return new WaitForSeconds(0.5f);

        // Step 3: Shop popup (5 sec)
        if (shopBlackBG != null) shopBlackBG.SetActive(true);
        if (shopPopupPanel != null) shopPopupPanel.SetActive(true);
        yield return new WaitForSeconds(5f);
        if (shopBlackBG != null) shopBlackBG.SetActive(false);
        if (shopPopupPanel != null) shopPopupPanel.SetActive(false);
        yield return new WaitForSeconds(0.5f);

        // Step 4: Upgrade popup (wait for close button)
        upgradePopupClosed = false;
        if (blackBG != null) blackBG.SetActive(true);
        if (upgradePopupPanel != null) upgradePopupPanel.SetActive(true);
        yield return new WaitUntil(() => upgradePopupClosed);
        if (blackBG != null) blackBG.SetActive(false);
        if (upgradePopupPanel != null) upgradePopupPanel.SetActive(false);
        yield return new WaitForSeconds(0.5f);

        // Step 5: Wave popup (3 sec) then 5 sec countdown — only shown once here
        if (blackBG != null) blackBG.SetActive(true);
        if (wavePopupPanel != null) wavePopupPanel.SetActive(true);
        yield return new WaitForSeconds(3f);
        if (blackBG != null) blackBG.SetActive(false);
        if (wavePopupPanel != null) wavePopupPanel.SetActive(false);
        yield return new WaitForSeconds(5f);

        // Begin first wave directly — skip StartWave's wave 1 popup logic
        BeginWave();
    }

    public void CloseUpgradePopup()
    {
        upgradePopupClosed = true;

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClick);
    }

    // ---------------------------------------------------------------
    // UPDATE — spawning loop
    // ---------------------------------------------------------------
    private void Update()
    {
        if (!isSpawning || gameOver) return;

        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= (1f / eps) && enemiesLeftToSpawn > 0)
        {
            SpawnEnemy();
            enemiesLeftToSpawn--;
            enemiesAlive++;
            timeSinceLastSpawn = 0f;
        }

        // Only trigger end of wave once all spawned enemies are killed
        if (enemiesLeftToSpawn == 0 && enemiesAlive <= 0 && !waveEndHandled)
        {
            waveEndHandled = true;
            EndWave();
        }
    }

    // ---------------------------------------------------------------
    // ENEMY DESTROYED
    // ---------------------------------------------------------------
    private void EnemyDestroyed()
    {
        if (gameOver) return;

        enemiesAlive--;
        if (enemiesAlive < 0) enemiesAlive = 0;

        enemiesKilled++;
        UnityEngine.Debug.Log("Enemy Killed: " + enemiesKilled + " / " + enemiesToKill + " | Alive: " + enemiesAlive + " | Left to spawn: " + enemiesLeftToSpawn);

        if (enemiesKilled >= enemiesToKill)
        {
            gameOver = true;
            UnityEngine.Debug.Log("Victory!");
            OpenVictoryPopup();
        }
    }

    private void OpenVictoryPopup()
    {
        if (victoryPopupPanel != null)
            victoryPopupPanel.SetActive(true);

        if (blackBG != null)
            blackBG.SetActive(true);

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX(AudioManager.Instance.gameWin);

        Time.timeScale = 0f;
    }

    // ---------------------------------------------------------------
    // WAVE MANAGEMENT
    // ---------------------------------------------------------------

    // Called directly for wave 1 (skips the popup — already shown in sequence)
    private void BeginWave()
    {
        if (gameOver) return;

        waveEndHandled = false;
        enemiesAlive = 0;
        enemiesLeftToSpawn = EnemiesPerWave();
        eps = EnemiesPerSecond();
        timeSinceLastSpawn = 0f;
        isSpawning = true;

        UnityEngine.Debug.Log("Wave " + currentWave + " started. Enemies to spawn: " + enemiesLeftToSpawn);

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX(AudioManager.Instance.waveStart);
    }

    // Called for wave 2+ (handles the between-wave delay)
    private IEnumerator StartWave()
    {
        isSpawning = false;
        waveEndHandled = false; // Reset so the next wave can end properly

        yield return new WaitForSeconds(timeBetweenWaves);

        if (gameOver) yield break;

        enemiesAlive = 0;
        enemiesLeftToSpawn = EnemiesPerWave();
        eps = EnemiesPerSecond();
        timeSinceLastSpawn = 0f;
        isSpawning = true;

        UnityEngine.Debug.Log("Wave " + currentWave + " started. Enemies to spawn: " + enemiesLeftToSpawn);

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX(AudioManager.Instance.waveStart);
    }

    private void EndWave()
    {
        if (gameOver) return;

        isSpawning = false;
        timeSinceLastSpawn = 0f;
        currentWave++;

        UnityEngine.Debug.Log("Wave ended. Starting wave " + currentWave);
        StartCoroutine(StartWave());
    }

    private void SpawnEnemy()
    {
        int index = Random.Range(0, enemyPrefabs.Length);
        GameObject prefabToSpawn = enemyPrefabs[index];
        Instantiate(prefabToSpawn, LevelManager.main.startPoint.position, Quaternion.identity);
    }

    private int EnemiesPerWave()
    {
        return Mathf.RoundToInt(baseEnemies * Mathf.Pow(currentWave, difficultyScalingFactor));
    }

    private float EnemiesPerSecond()
    {
        return Mathf.Clamp(enemiesPerSecond * Mathf.Pow(currentWave, difficultyScalingFactor),
            0f, enemiesPerSecondCap);
    }
}