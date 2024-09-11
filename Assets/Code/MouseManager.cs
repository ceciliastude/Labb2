using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour
{
   public Vector2 okButtonPosition;

   public void MoveMouseToOkButton()
    {
        Cursor.lockState = CursorLockMode.Locked;  
        Cursor.lockState = CursorLockMode.None;    

        Cursor.SetCursor(null, okButtonPosition, CursorMode.Auto);
    }
}
