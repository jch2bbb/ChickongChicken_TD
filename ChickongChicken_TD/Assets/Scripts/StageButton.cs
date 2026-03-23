using TMPro;
using UnityEngine;
using static UnityEngine.Audio.GeneratorInstance;
using static UnityEngine.Rendering.DebugUI;

public class StageButton : MonoBehaviour
{
    [Header("Popup Panel")]
    [SerializeField] private GameObject popupPanel;
    [SerializeField] private TextMeshProUGUI popupTitle_1;
    [SerializeField] private TextMeshProUGUI popupTitle_2;
    [SerializeField] private TextMeshProUGUI popupDescription;
    [SerializeField] private GameObject blackBG;


    [Header("This Stage's Data")]
    [SerializeField] private string stageName_1;
    [SerializeField] private string stageName_2;
    [SerializeField] private string stageDescription;

    public void OpenPopup()
    {
        if (popupPanel != null)
        {
            popupTitle_1.text = stageName_1;
            popupTitle_2.text = stageName_2;
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