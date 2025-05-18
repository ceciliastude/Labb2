using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomSceneManager : MonoBehaviour
{
    public GameObject ciaraSprite;  
    public GameObject vincentSprite; 

    public GameObject iconSprite1;
    public GameObject iconSprite2;
    public GameObject iconSprite3;
    public GameObject iconSprite4;

    void Start()
    {
        // Ensure that this is the Testing Environment scene
        if (SceneManager.GetActiveScene().name == "Testing Enviroment")
        {
            Debug.Log("Scene loaded");
            ActivateSelectedCharacter();
        }
    }

    public void ActivateSelectedCharacter()
    {
        // Retrieve the selected character from PlayerPrefs
        string selectedCharacter = PlayerPrefs.GetString("SelectedCharacter", "None");

        // Disable both sprites initially
        if (ciaraSprite) ciaraSprite.SetActive(false);
        if (vincentSprite) vincentSprite.SetActive(false);

        // Enable the sprite based on the selected character
        if (selectedCharacter == "Ciara")
        {
            if (ciaraSprite) ciaraSprite.SetActive(true);
            if (iconSprite2) iconSprite2.SetActive(true);
            if (iconSprite4) iconSprite4.SetActive(true);
        }
        else if (selectedCharacter == "Vincent")
        {
            if (vincentSprite) vincentSprite.SetActive(true);
            if (iconSprite1) iconSprite1.SetActive(true);
            if (iconSprite3) iconSprite3.SetActive(true);
        }
        else
        {
            Debug.LogError("Invalid character selected.");
        }
    }
}
