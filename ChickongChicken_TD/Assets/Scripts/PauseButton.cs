using UnityEngine;

public class PauseButton : MonoBehaviour
{
    [Header("Pause Popup")]
    [SerializeField] private GameObject pausePopupPanel;
    [SerializeField] private GameObject blackBG;

    private bool isPaused = false;

    public void OnPauseButtonClicked()
    {
        if (isPaused)
            ResumeGame();
        else
            PauseGame();
    }

    private void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;

        if (pausePopupPanel != null)
            pausePopupPanel.SetActive(true);

        if (blackBG != null)
            blackBG.SetActive(true);

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClick);
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;

        if (pausePopupPanel != null)
            pausePopupPanel.SetActive(false);

        if (blackBG != null)
            blackBG.SetActive(false);

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClick);
    }
}