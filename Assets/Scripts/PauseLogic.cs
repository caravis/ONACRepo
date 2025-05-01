using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseLogic : MonoBehaviour
{
    private bool gamePaused;
    private bool settingsMenuOpen;
    public GameObject pauseMenuUI; // UI Reference to the pause menu
    public GameObject settingsMenuUI; // UI Reference to the pause menu

    public static bool IsGamePaused = false;

    private void Start()
    {
        gamePaused = false;
        settingsMenuOpen = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && settingsMenuOpen == false)
        {

            if (!gamePaused)
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        gamePaused = true;
        IsGamePaused = true;
        Time.timeScale = 0;
        pauseMenuUI.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        gamePaused = false;
        IsGamePaused = false;
        Time.timeScale = 1;
        pauseMenuUI.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    public void OpenSettings()
    {
        gamePaused = true;
        Time.timeScale = 0;
        settingsMenuUI.SetActive(true);
        pauseMenuUI.SetActive(false);
        settingsMenuOpen = true;
    }

    public void CloseSettings()
    {
        gamePaused = true;
        Time.timeScale = 0;
        settingsMenuUI.SetActive(false);
        pauseMenuUI.SetActive(true);
        settingsMenuOpen = false;
    }

    public void TitleScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadSceneAsync("TitleScene");
    }

    public void EndGame()
    {
        Application.Quit();
    }
}
