using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Tutorial : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject[] enemyPrefabs;

    [Header("Attributes")]
    [SerializeField] private int baseEnemies = 8;
    [SerializeField] private float enemiesPerSecond = 0.5f;
    [SerializeField] private float timeBetweenWaves = 5f;
    [SerializeField] private float difficultyScalingFactor = 0.75f;
    [SerializeField] private float enemiesPerSecondCap = 15f;

    [Header("Victory")]
    [SerializeField] private int enemiesToKill = 20;

    [Header("Victory Popup")]
    [SerializeField] private GameObject victoryPopupPanel;
    [SerializeField] private GameObject blackBG;

    [Header("Wave Popup")]
    [SerializeField] private GameObject wavePopupPanel;

    [Header("Events")]
    public static UnityEvent onEnemyDestroy = new UnityEvent();

    private int currentWave = 1;
    private float timeSinceLastSpawn;
    private int enemiesAlive;
    private int enemiesLeftToSpawn;
    private float eps;
    private bool isSpawning = false;
    private int enemiesKilled = 0;
    private bool gameOver = false;

    private void Awake()
    {
        onEnemyDestroy.RemoveAllListeners();
        onEnemyDestroy.AddListener(EnemyDestroyed);
    }

    private void Start()
    {
        Time.timeScale = 1f;
        StartCoroutine(StartWave());
    }

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

        if (enemiesAlive <= 0 && enemiesLeftToSpawn == 0)
        {
            EndWave();
        }
    }

    private void EnemyDestroyed()
    {
        if (gameOver) return;

        enemiesAlive--;
        if (enemiesAlive < 0) enemiesAlive = 0;

        enemiesKilled++;

        UnityEngine.Debug.Log("Enemy Killed: " + enemiesKilled + " / " + enemiesToKill + " | Alive: " + enemiesAlive);

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

        if (gameOver) yield break;

        // Show wave popup on first wave only
        if (currentWave == 1)
        {
            if (blackBG != null) blackBG.SetActive(true);
            if (wavePopupPanel != null) wavePopupPanel.SetActive(true);

            yield return new WaitForSeconds(5f);

            if (blackBG != null) blackBG.SetActive(false);
            if (wavePopupPanel != null) wavePopupPanel.SetActive(false);

            yield return new WaitForSeconds(3f);

            if (gameOver) yield break;
        }

        enemiesAlive = 0;
        isSpawning = true;
        enemiesLeftToSpawn = EnemiesPerWave();
        eps = EnemiesPerSecond();

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