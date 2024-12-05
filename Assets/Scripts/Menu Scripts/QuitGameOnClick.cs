using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitOnClick : MonoBehaviour
{
    private void OnMouseDown()
    {
        // Check if we are in the editor or build
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;  // Stop play mode in the editor
        #else
            Application.Quit();  // Quit the game in a built version
        #endif
    }
}
