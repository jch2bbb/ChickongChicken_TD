using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System.Collections;

public class PauseButton : MonoBehaviour
{
    [Header("Pause Popup")]
    [SerializeField] private GameObject pausePopupPanel;
    [SerializeField] private GameObject pauseBlackBG;

    private bool isPaused = false;
    private bool isProcessing = false;

    private void Start()
    {
        Time.timeScale = 1f;
        isPaused = false;

        // Directly hide pause panel only — don't touch blackBG
        // since other systems (wave popup, tutorial) may use it
        if (pausePopupPanel != null)
            pausePopupPanel.SetActive(false);
    }

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

        if (pauseBlackBG != null)
            pauseBlackBG.SetActive(true);

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

        if (pauseBlackBG != null)
            pauseBlackBG.SetActive(false);

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClick);

        EventSystem.current.SetSelectedGameObject(null);
        isProcessing = false;
    }
}


