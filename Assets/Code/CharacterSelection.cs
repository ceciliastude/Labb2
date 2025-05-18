using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour
{
    private string currentSelection = "";
    public LoadingScreen loadingScreen;

    public void Start(){
        loadingScreen = FindObjectOfType<LoadingScreen>();
    }
    public void SelectCharacter1()
    {
        PlayerPrefs.SetString("SelectedCharacter", "Ciara");
        currentSelection = "Ciara";
        Debug.Log("Selected Ciara, Testing: " + currentSelection);
    }

    public void SelectCharacter2()
    {
        PlayerPrefs.SetString("SelectedCharacter", "Vincent");
        currentSelection = "Vincent";
        Debug.Log("Selected Vincent, Testing: " + currentSelection);
    }

    public string GetSelectedCharacter()
    {
        return PlayerPrefs.GetString("SelectedCharacter", "None");
    }

    public void OnBackButtonPressed()
    {
        // Clear the current selection and re-enable hover effects
        PlayerPrefs.DeleteKey("SelectedCharacter");
        currentSelection = "";
        Debug.Log("Returning to previous screen and clearing selection. Testing: " + currentSelection);
    }
}
