using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles the jump scare sequence triggered when an enemy catches the player.
/// This includes resetting other enemies, moving the attacker to the scare point,
/// playing an audio clip, shaking the camera, forcing the player to look at the attacker,
/// and transitioning to the lose screen.
/// </summary>
public class JumpScareManager : MonoBehaviour
{
    public static JumpScareManager Instance;

    [SerializeField] Transform jumpScarePoint;    // Where the attacker will teleport to for the jumpscare
    [SerializeField] Camera playerCamera;         // Reference to the player's camera
    [SerializeField] float shakeIntensity = 0.2f; // How strong the camera shake is
    [SerializeField] float shakeDuration = 1.5f;  // How long the camera shakes
    [SerializeField] AudioSource audioSource;     // Audio source to play the jumpscare sound
    [SerializeField] AudioClip jumpScareClip;     // The sound to play during the jumpscare

    private bool isScaring = false;               // Prevents multiple scares from overlapping
    private float shakeTimer;

    /// <summary>
    /// Sets the static instance for easy global access.
    /// </summary>
    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// Initiates the jumpscare sequence.
    /// Repels all enemies except the attacker, moves the attacker to the jumpscare point,
    /// plays sound, starts shaking the screen, and eventually loads the LoseScene.
    /// </summary>
    /// <param name="attacker">The enemy that caught the player.</param>
    /// <param name="allEnemies">All active enemies in the scene.</param>
    public void TriggerJumpScare(EnemyAI attacker, EnemyAI[] allEnemies)
    {
        if (isScaring) return; // Prevent multiple triggers

        isScaring = true;

        // Repel all other enemies back to their spawn points
        foreach (EnemyAI enemy in allEnemies)
        {
            if (enemy != attacker)
            {
                enemy.Repel();
            }
        }

        // Move attacker to the jumpscare point and face the player
        attacker.transform.position = jumpScarePoint.position;
        attacker.FacePlayer();

        // Play jumpscare sound
        if (audioSource && jumpScareClip)
        {
            audioSource.PlayOneShot(jumpScareClip);
        }

        // Start the screen shake and camera lock sequence
        shakeTimer = shakeDuration;
        StartCoroutine(JumpScareSequence());
    }

    /// <summary>
    /// Coroutine that handles camera shake, forces the player to look at the attacker,
    /// and transitions to the lose screen after the sequence finishes.
    /// </summary>
    private IEnumerator JumpScareSequence()
    {
        float elapsed = 0f;
        Vector3 originalPos = playerCamera.transform.localPosition;

        while (elapsed < shakeDuration)
        {
            elapsed += Time.deltaTime;

            // Shake the camera
            Vector3 randomOffset = Random.insideUnitSphere * shakeIntensity;
            playerCamera.transform.localPosition = originalPos + randomOffset;

            // Force camera to look at the jumpscare point
            Vector3 direction = jumpScarePoint.position - playerCamera.transform.position;
            playerCamera.transform.rotation = Quaternion.LookRotation(direction);

            yield return null;
        }

        // Reset camera position
        playerCamera.transform.localPosition = originalPos;

        // Make cursor visible and unlocked
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Load the lose screen
        SceneManager.LoadScene("LoseScene");
    }
}
