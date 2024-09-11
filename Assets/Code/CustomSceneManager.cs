using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomSceneManager : MonoBehaviour
{
    public CharacterSelection characterSelection;
    public GameObject ciara;
    public GameObject vincent;

    void Start()
    {
        // Subscribe to the sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        // Unsubscribe from the sceneLoaded event
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void LoadFighterScene(){
        SceneManager.LoadScene("Testing Enviroment");
        Debug.Log("Loading Next scene...");
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Call ActivateSelectedCharacter after the scene has loaded
        ActivateSelectedCharacter();
    }

    public void ActivateSelectedCharacter(){
        ciara.SetActive(false);
        vincent.SetActive(false);

        string selectedCharacter = characterSelection.GetSelectedCharacter();

        if (selectedCharacter == "Ciara"){
            ciara.SetActive(true);
            vincent.SetActive(false);
        }
        else if (selectedCharacter == "Vincent"){
            ciara.SetActive(false);
            vincent.SetActive(true);
        }
        else
        {
            Debug.LogError("Invalid character selected.");
        }
    }
}
