using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject[] enemyPrefabs;

    [Header("Attributes")]
    [SerializeField] private int baseEnemies = 8;
    [SerializeField] private float enemiesPerSecond = 0.5f;
    [SerializeField] private float timeBetweenWaves = 5f;
    [SerializeField] private float difficultyScalingFactor = 0.75f;
    [SerializeField] private float enemiesPerSecondCap = 15f;

    [Header("Victory Popup")]
    [SerializeField] private GameObject victoryPopupPanel;
    [SerializeField] private GameObject blackBG;

    [Header("Wave Popup")]
    [SerializeField] private GameObject wavePopupPanel;

    [Header("Victory")]
    [SerializeField] private int enemiesToKill = 20;

    [Header("Infinite Wave")]
    [SerializeField] private bool isInfiniteMode = false;
    [SerializeField] private int maxDifficultyWave = 20;

    [Header("Events")]
    public static UnityEvent onEnemyDestroy = new UnityEvent();

    private int currentWave = 1;
    private float timeSinceLastSpawn;
    private int enemiesAlive;
    private int enemiesLeftToSpawn;
    private float eps;
    private bool isSpawning = false;
    private int enemiesKilled = 0;
    private bool victoryTriggered = false;

    private void Awake()
    {
        onEnemyDestroy.RemoveAllListeners();
        onEnemyDestroy.AddListener(EnemyDestroyed);
    }

    private void Start()
    {
        StartCoroutine(StartWave());
        UpdateKillUI();
    }

    private void Update()
    {
        if (!isSpawning) return;

        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= (1f / eps) && enemiesLeftToSpawn > 0)
        {
            SpawnEnemy();
            enemiesLeftToSpawn--;
            enemiesAlive++;
            timeSinceLastSpawn = 0f;
        }

        if (enemiesAlive <= 0 && enemiesLeftToSpawn == 0)
        {
            EndWave();
        }
    }

    private void EnemyDestroyed()
    {
        enemiesAlive--;

        if (enemiesAlive < 0) enemiesAlive = 0;

        enemiesKilled++;

        UpdateKillUI();

        UnityEngine.Debug.Log("Enemy Killed: " + enemiesKilled + " / " +
            (isInfiniteMode ? "\u221E" : enemiesToKill.ToString()) +
            " | Wave: " + currentWave +
            " | Alive: " + enemiesAlive);

        if (!isInfiniteMode && enemiesKilled >= enemiesToKill && !victoryTriggered)
        {
            victoryTriggered = true;
            UnityEngine.Debug.Log("Victory! Loading Victory Scene...");
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

    private void UpdateKillUI()
    {
        if (InfiniteWaveUI.main != null)
        {
            InfiniteWaveUI.main.UpdateWaveText(currentWave, enemiesKilled, enemiesToKill, isInfiniteMode);
        }
    }

    private IEnumerator StartWave()
    {
        isSpawning = false;

        if (currentWave > 1)
        {
            yield return new WaitForSeconds(timeBetweenWaves);
        }
        else
        {
            yield return null;
        }

        enemiesAlive = 0;
        isSpawning = true;
        enemiesLeftToSpawn = EnemiesPerWave();
        eps = EnemiesPerSecond();

        UpdateKillUI();

        UnityEngine.Debug.Log("Wave " + currentWave + " started. Enemies: " + enemiesLeftToSpawn);
    }

    private void EndWave()
    {
        isSpawning = false;
        timeSinceLastSpawn = 0f;
        currentWave++;
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
        int cappedWave = isInfiniteMode ? Mathf.Min(currentWave, maxDifficultyWave) : currentWave;
        return Mathf.RoundToInt(baseEnemies * Mathf.Pow(cappedWave, difficultyScalingFactor));
    }

    private float EnemiesPerSecond()
    {
        int cappedWave = isInfiniteMode ? Mathf.Min(currentWave, maxDifficultyWave) : currentWave;
        return Mathf.Clamp(enemiesPerSecond * Mathf.Pow(cappedWave, difficultyScalingFactor),
            0f, enemiesPerSecondCap);
    }

    public int GetCurrentWave()
    {
        return currentWave;
    }

    public int GetEnemiesKilled()
    {
        return enemiesKilled;
    }
}