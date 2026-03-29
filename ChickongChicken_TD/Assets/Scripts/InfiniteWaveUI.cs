using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfiniteWaveUI : MonoBehaviour
{
    public static InfiniteWaveUI main;

    [Header("References")]
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private TextMeshProUGUI enemiesKilledText;
    [SerializeField] private TextMeshProUGUI highScoreText;

    private int highScore = 0;

    private void Awake()
    {
        main = this;
    }

    private void Start()
    {
        highScore = PlayerPrefs.GetInt("InfiniteHighScore", 0);
        UpdateHighScoreText();
    }

    public void UpdateWaveText(int wave, int enemiesKilled, int enemiesToKill, bool isInfinite)
    {
        if (waveText != null)
            waveText.text = "Wave: " + wave;

        if (enemiesKilledText != null)
        {
            if (isInfinite)
                enemiesKilledText.text = "Defeat: " + enemiesKilled + "/\u221E";
            else
                enemiesKilledText.text = "Defeat: " + enemiesKilled + "/" + enemiesToKill;
        }

        if (enemiesKilled > highScore && isInfinite)
        {
            highScore = enemiesKilled;
            PlayerPrefs.SetInt("InfiniteHighScore", highScore);
            PlayerPrefs.Save();
            UpdateHighScoreText();
        }
    }

    private void UpdateHighScoreText()
    {
        if (highScoreText != null)
            highScoreText.text = "Best: " + highScore;
    }
}