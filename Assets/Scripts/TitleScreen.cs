using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreen : MonoBehaviour
{
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        PauseLogic.IsGamePaused = false; // Just in case
        Time.timeScale = 1f; // Reset time
    }
}
