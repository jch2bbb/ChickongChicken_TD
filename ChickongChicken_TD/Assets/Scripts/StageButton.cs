using TMPro;
using UnityEngine;
using static UnityEngine.Audio.GeneratorInstance;
using static UnityEngine.Rendering.DebugUI;

public class StageButton : MonoBehaviour
{
    [Header("Popup Panel")]
    [SerializeField] private GameObject popupPanel;
    [SerializeField] private TextMeshProUGUI popupTitle;
    [SerializeField] private TextMeshProUGUI popupDescription;
    [SerializeField] private GameObject blackBG;


    [Header("This Stage's Data")]
    [SerializeField] private string stageName;
    [SerializeField] private string stageDescription;

    public void OpenPopup()
    {
        if (popupPanel != null)
        {
            popupTitle.text = stageName;
            popupDescription.text = stageDescription;
            popupPanel.SetActive(true);
            blackBG.SetActive(true);

            // Null check before calling
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClick);
        }
    }

    public void ClosePopup()
    {
        if (popupPanel != null)
            popupPanel.SetActive(false);
            blackBG.SetActive(false);

        // Null check before calling
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClick);
    }
}