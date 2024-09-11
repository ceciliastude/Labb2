using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelection : MonoBehaviour
{
    public GameObject ciara;
    public GameObject vincent;
    private string selectedCharacter;

    public void SelectCiara(){
        selectedCharacter = "Ciara";
        MoveMouseToOkButton();
        Debug.Log("Ciara Selected");

    }

        public void SelectVincent(){
        selectedCharacter = "Vincent";
        MoveMouseToOkButton();
        Debug.Log("Vincent Selected");
    }

    public string GetSelectedCharacter(){
        return selectedCharacter;
    }

    public void MoveMouseToOkButton()
    {
        Vector2 okButtonPosition = new Vector2(100, 100); // Placeholder, use actual screen position
        Cursor.lockState = CursorLockMode.Locked;  // Freeze cursor
        Cursor.lockState = CursorLockMode.None;    // Unlock to move
        Cursor.SetCursor(null, okButtonPosition, CursorMode.Auto);
    }
}
