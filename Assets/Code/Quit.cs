using UnityEngine;
using UnityEngine.UI;

public class Quit : MonoBehaviour
{
    public Image quitButton; // Assign this in the Inspector

    public void QuitGameApplication()
    {
        // This will quit the application
        Application.Quit();

        #if UNITY_EDITOR
        // If you are running in the editor, stop playing the scene
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
