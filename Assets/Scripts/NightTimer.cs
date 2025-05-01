using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Add at the top if using UI.Text
using TMPro;           // Add at the top if using TextMeshProUGUI

public class NightTimer : MonoBehaviour
{
    public static NightTimer Instance;

    public float hourDuration = 45f; // Seconds per in-game hour
    private float timer = 0f;
    private int currentHour = 0; // 0 = 12 AM, 1 = 1 AM, ..., 6 = 6 AM

    public EnemyAI[] allEnemies; // Assign in Inspector

    public TextMeshProUGUI hourText; // Drag and drop from the Canvas in Inspector


    /// <summary>
    /// Awake method (you know what an awake is) 
    /// </summary>
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        timer = hourDuration;
        currentHour = 0;
        UpdateTimeDisplay();
    }

    /// <summary>
    /// The Update Method (you know what an update method is)
    /// </summary>
    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f && currentHour < 6)
        {
            currentHour++;
            timer = hourDuration;

            UpdateEnemyChances();
            UpdateTimeDisplay(); 
        }

        if(currentHour == 6)
        {
            // Make cursor visible and unlocked
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            // Load the Win screen
            SceneManager.LoadScene("WinScene");
        }
    }

    /// <summary>
    /// Updates the enemy chances to advance and increases speed every "hour" 
    /// </summary>
    private void UpdateEnemyChances()
    {
        foreach (EnemyAI enemy in allEnemies)
        {
            enemy.chanceToAdvance += 0.10f;
            enemy.chanceToAdvance = Mathf.Clamp01(enemy.chanceToAdvance); // Prevent >100%
            enemy.speed += 0.5f;
        }
    }

    /// <summary>
    /// Updates UI Display
    /// </summary>
    private void UpdateTimeDisplay()
    {
        string hourString = GetCurrentHourText();
        Debug.Log($"Hour: {hourString}");

        if (hourText != null)
            hourText.text = hourString;
    }


    /// <summary>
    /// Gets the current hour text
    /// </summary>
    /// <returns></returns>
    public string GetCurrentHourText()
    {
        int hour = 12 + currentHour;
        return (hour > 12 ? hour - 12 : hour) + " : 00 AM";
    }
}
