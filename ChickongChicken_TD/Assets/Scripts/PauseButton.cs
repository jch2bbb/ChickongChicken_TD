using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PauseButton : MonoBehaviour
{
    [Header("Pause Popup")]
    [SerializeField] private GameObject pausePopupPanel;
    [SerializeField] private GameObject blackBG;

    private bool isPaused = false;
    private bool isProcessing = false;

    private void Update()
    {
        if (isPaused && Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            ResumeGame();
        }
    }

    public void OnPauseButtonClicked()
    {
        if (isProcessing) return;

        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    private void PauseGame()
    {
        isProcessing = true;
        isPaused = true;
        Time.timeScale = 0f;

        if (pausePopupPanel != null)
            pausePopupPanel.SetActive(true);

        if (blackBG != null)
            blackBG.SetActive(true);

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClick);

        EventSystem.current.SetSelectedGameObject(null);
        isProcessing = false;
    }

    public void ResumeGame()
    {
        isProcessing = true;
        isPaused = false;
        Time.timeScale = 1f;

        if (pausePopupPanel != null)
            pausePopupPanel.SetActive(false);

        if (blackBG != null)
            blackBG.SetActive(false);

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClick);

        EventSystem.current.SetSelectedGameObject(null);
        isProcessing = false;
    }
}