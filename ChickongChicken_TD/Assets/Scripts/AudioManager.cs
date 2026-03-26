using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource bgmSource;

    [Header("Background Music")]
    public AudioClip backgroundMusic;
    [Range(0f, 1f)] public float bgmVolume = 1f;

    [Header("SFX Clips & Volume")]
    public AudioClip buttonClick;
    [Range(0f, 1f)] public float buttonClickVolume = 1f;

    public AudioClip buttonStart;
    [Range(0f, 1f)] public float buttonStartVolume = 1f;

    public AudioClip gameOver;
    [Range(0f, 1f)] public float gameOverVolume = 1f;

    public AudioClip gameWin;
    [Range(0f, 1f)] public float gameWinVolume = 1f;

    public AudioClip waveStart;
    [Range(0f, 1f)] public float waveStartVolume = 1f;

    public AudioClip enemyDeath;
    [Range(0f, 1f)] public float enemyDeathVolume = 1f;

    public AudioClip enemyHurt;
    [Range(0f, 1f)] public float enemyHurtVolume = 1f;

    public AudioClip farmDamage;
    [Range(0f, 1f)] public float farmDamageVolume = 1f;

    public AudioClip farmDestroyed;
    [Range(0f, 1f)] public float farmDestroyedVolume = 1f;

    public AudioClip towerShooting;
    [Range(0f, 1f)] public float towerShootingVolume = 1f;

    public AudioClip towerUpgrade;
    [Range(0f, 1f)] public float towerUpgradeVolume = 1f;



    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayBackgroundMusic();
    }

    private void PlayBackgroundMusic()
    {
        if (backgroundMusic == null) return;

        bgmSource.clip = backgroundMusic;
        bgmSource.volume = bgmVolume;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;

        float volume = GetVolumeForClip(clip);
        sfxSource.PlayOneShot(clip, volume);
    }

    private float GetVolumeForClip(AudioClip clip)
    {
        if (clip == buttonClick) return buttonClickVolume;
        if (clip == enemyDeath) return enemyDeathVolume;
        if (clip == enemyHurt) return enemyHurtVolume;
        if (clip == farmDamage) return farmDamageVolume;
        if (clip == farmDestroyed) return farmDestroyedVolume;
        if (clip == gameOver) return gameOverVolume;
        if (clip == gameSong) return gameSongVolume;
        if (clip == gameWin) return gameWinVolume;
        if (clip == buttonStart) return buttonStartVolume;
        if (clip == towerShooting) return towerShootingVolume;
        if (clip == towerUpgrade) return towerUpgradeVolume;
        if (clip == waveStart) return waveStartVolume;

        return 1f;
    }
}