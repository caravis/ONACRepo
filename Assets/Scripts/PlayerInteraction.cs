using UnityEngine;

// Handles player's interaction with enemies, specifically the repel mechanic
public class PlayerInteraction : MonoBehaviour
{
    public EnemyAI[] enemies;                  // List of enemies the player can interact with
    public Camera playerCamera;                // Reference to the player's camera
    public float repelAngleThreshold = 30f;    // Angle threshold for determining if player is looking at an enemy

    void Update()
    {
        // When space key is pressed, attempt to repel all enemies in view
        if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach (EnemyAI enemy in enemies)
            {
                // Check if player is looking at this enemy within threshold
                if (IsLookingAtEnemy(enemy.transform))
                {
                    enemy.Repel(); // Repel enemy
                }
                else
                {
                    Debug.Log($"Not looking at {enemy.name}, repel failed."); // Log failed attempt
                }
            }
        }
    }

    /// <summary>
    /// Determines if the player is looking directly enough at the enemy
    /// </summary>
    /// <param name="enemyTransform"></param>
    /// <returns></returns>
    bool IsLookingAtEnemy(Transform enemyTransform)
    {
        // Direction from camera to enemy
        Vector3 directionToEnemy = (enemyTransform.position - playerCamera.transform.position).normalized;

        // Angle between camera's forward vector and direction to enemy
        float angle = Vector3.Angle(playerCamera.transform.forward, directionToEnemy);

        // Return true if within allowable angle
        return angle <= repelAngleThreshold;
    }
}
