using UnityEngine;
using UnityEngine.SceneManagement;

public class NextSceneButton : MonoBehaviour
{
    [Header("Scene Settings")]
    [Tooltip("Leave empty to reload the previously visited scene.")]
    public string destinationScene;

    public void OnNextButtonClicked()
    {
        if (string.IsNullOrEmpty(destinationScene))
        {
            // No scene set — go back to the previous scene
            int previousSceneIndex = SceneManager.GetActiveScene().buildIndex - 1;
            SceneManager.LoadScene(previousSceneIndex);
        }
        else
        {
            SceneManager.LoadScene(destinationScene);
        }
    }
}