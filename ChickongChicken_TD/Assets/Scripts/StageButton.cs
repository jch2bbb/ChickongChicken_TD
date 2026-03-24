using TMPro;
using UnityEngine;

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

    [Header("Scene To Load")]
    [SerializeField] private string sceneName; // e.g. "Level1"

    public void OpenPopup()
    {
        if (popupPanel != null)
        {
            // Set popup text
            popupTitle_1.text = stageName_1;
            popupTitle_2.text = stageName_2;
            popupDescription.text = stageDescription;

            // Tell the Play Button which scene to load
            NextSceneButton playButton = popupPanel.GetComponentInChildren<NextSceneButton>();
            if (playButton != null)
                playButton.destinationScene = sceneName;

            popupPanel.SetActive(true);
            blackBG.SetActive(true);

            if (AudioManager.Instance != null)
                AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClick);
        }
    }

    public void ClosePopup()
    {
        if (popupPanel != null)
            popupPanel.SetActive(false);
            blackBG.SetActive(false);

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClick);
    }
}