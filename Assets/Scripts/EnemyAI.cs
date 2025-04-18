using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Basic enemy AI that either follows a path or advances toward the player
public class EnemyAI : MonoBehaviour
{
    public Transform player; // Player to chase

    public float speed = 2f;             // Movement speed
    public float attackDistance = 1f;    // Distance at which the enemy can attack

    public Transform[] pathWaypoints;    //path points for pre-chase movement

    [Range(0f, 1f)]
    public float chanceToAdvance = 0.25f; // Probability to advance when timer expires

    public float minWaitTime = 3f;       // Minimum time to wait before next advance attempt
    public float maxWaitTime = 7f;       // Maximum time to wait before next advance attempt

    private float currentTimer;          // Timer counting down to potential advance
    private bool isAdvancing = false;    // Whether the enemy is currently advancing
    private bool isFollowingPath = true; // Whether the enemy is still on its path
    private int currentWaypointIndex = 0;// Current waypoint in path

    private Vector3 startPosition;       // Initial position to reset to
    private Quaternion startRotation;    // Initial rotation to reset to

    void Start()
    {
        // Save starting transform for reset
        startPosition = transform.position;
        startRotation = transform.rotation;

        // Start the timer with a random value
        ResetTimer();
    }

    void Update()
    {
        if (!isAdvancing)
        {
            // Countdown until next potential advance
            currentTimer -= Time.deltaTime;
            if (currentTimer <= 0f)
            {
                TryAdvance(); // Random roll to decide if it advances
            }
        }
        else
        {
            // Follow path if still on one, otherwise go straight to player
            if (isFollowingPath && pathWaypoints.Length > 0)
            {
                FollowPath();
            }
            else
            {
                AdvanceTowardsPlayer();
            }
        }
    }

    /// <summary>
    /// Determines whether the enemy should begin advancing
    /// </summary>
    void TryAdvance()
    {
        float roll = Random.value; // Random number between 0 and 1
        if (roll <= chanceToAdvance)
        {
            isAdvancing = true;
            Debug.Log($"{gameObject.name} begins advancing! (Roll: {roll:F2})");
        }
        else
        {
            Debug.Log($"{gameObject.name} rolled {roll:F2} and stays put.");
            ResetTimer(); // Retry again later
        }
    }

    /// <summary>
    /// Moves enemy along its waypoint path
    /// </summary>
    void FollowPath()
    {
        FacePlayer(); // Always face the player for tension

        Transform targetWaypoint = pathWaypoints[currentWaypointIndex];
        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, speed * Time.deltaTime);

        // Check if close enough to next waypoint
        float distance = Vector3.Distance(transform.position, targetWaypoint.position);
        if (distance < 0.1f)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= pathWaypoints.Length)
            {
                isFollowingPath = false; // Switch to direct approach
            }
        }
    }

    /// <summary>
    /// Moves enemy directly toward the player
    /// </summary>
    void AdvanceTowardsPlayer()
    {
        FacePlayer();

        transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

        // Check if within attack range
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= attackDistance)
        {
            Debug.Log("Player loses!");
            // TODO: Add actual lose condition logic here
        }
    }

    /// <summary>
    /// Called by PlayerInteraction to stop enemy and reset
    /// </summary>
    public void Repel()
    {
        Debug.Log($"{gameObject.name} was repelled!");
        ResetEnemy(); // Go back to start
    }

    /// <summary>
    /// Resets enemy to start state
    /// </summary>
    void ResetEnemy()
    {
        isAdvancing = false;
        isFollowingPath = true;
        currentWaypointIndex = 0;
        transform.position = startPosition;
        transform.rotation = startRotation;
        ResetTimer();
    }

    /// <summary>
    /// Sets a new wait time between advances
    /// </summary>
    void ResetTimer()
    {
        currentTimer = Random.Range(minWaitTime, maxWaitTime);
    }

    /// <summary>
    /// Rotates the enemy to face the player smoothly
    /// </summary>
    void FacePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
}
