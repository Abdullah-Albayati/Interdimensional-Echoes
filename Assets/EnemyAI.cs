using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public enum EnemyState
{
    Idle,
    Chasing,
    Searching,
    Investigating,
    Attack
}

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public CreatureData creatureData;
    private NavMeshAgent agent;
    public EnemyState currentState;
    bool isPlayerDetected;
    private float wanderRadius = 20.0f;  // The range within which the enemy will pick a new random destination.
    private float wanderTimer = 5f;

    private float timer;

    public float detectionRange = 10.0f;  // Max distance for detection
    public float fieldOfViewAngle = 45.0f;  // Angle for the field of view (half total angle)
    public LayerMask detectionMask;  // Masks for detection (includes Player and Obstacles)
    public int numberOfRays = 10;  // Number of rays in field of view
    bool wasPlayerDetected = false;
    Vector3 playerLastSeenPos;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentState = EnemyState.Idle;
    }

    private void Update()
    {
        DetectPlayer();
        switch (currentState)
        {
                case EnemyState.Idle:
                Debug.Log($"Patrolling");
                timer += Time.deltaTime;

                if (timer >= wanderTimer) 
                {
                    Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
                    agent.SetDestination(newPos);
                    timer = 0;
                }

                if (isPlayerDetected)
                {
                    currentState = EnemyState.Chasing; // Transition to chasing if the player is detected
                }
                break;
                case EnemyState.Chasing:
                if (isPlayerDetected)
                {
                    agent.SetDestination(player.position);
                }
                else
                {
                    currentState = EnemyState.Searching;
                   
                }
                // Chasing behavior here
                break;

                case EnemyState.Searching:

                   agent.SetDestination(playerLastSeenPos);

                    if ( !agent.pathPending && agent.remainingDistance <= 1f) // "1f" can be a threshold value you decide.
                    {
                        currentState = EnemyState.Idle;
                        Debug.Log("Finished Searching. Player not found.");
                    }
                
                break;

                case EnemyState.Investigating:
                    // Investigating behavior here
                    break;
                case EnemyState.Attack:
                    // Attack behavior here
                    break;
        }
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float distance, int layermask)
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * distance;  // Pick a random point within the sphere.
        randomDirection += origin;  // Adjust this point based on the enemy's current position.

        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, distance, layermask);  // Get the nearest navigable point to our random point.

        return navHit.position;
    }
    private void DetectPlayer()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float angleBetweenEnemyAndPlayer = Vector3.Angle(transform.forward, directionToPlayer);
        bool previousDetection = isPlayerDetected;
        isPlayerDetected = false;
        // Check if player is within the field of view
        if (angleBetweenEnemyAndPlayer < fieldOfViewAngle / 2f)
        {
           
            float angleStep = fieldOfViewAngle / numberOfRays;
            for (int i = 0; i < numberOfRays; i++)
            {
                float currentRayAngle = (-fieldOfViewAngle / 2f) + (angleStep * i);
                Vector3 rayDirection = Quaternion.Euler(0, currentRayAngle, 0) * transform.forward;

                RaycastHit hit;
                if (Physics.Raycast(transform.position, rayDirection, out hit, detectionRange, detectionMask))
                {
                    RaycastHit lineHit;
                    if (Physics.Linecast(transform.position, player.position, out lineHit))
                    {
                        if (!lineHit.collider.CompareTag("Player"))
                        {
                            Debug.Log("there is an obstacle");
                           
                            continue; // skip the rest of this loop iteration and go to the next ray

                            
                        }
                        else
                        {
                            Debug.Log("Player Detected");
                            isPlayerDetected = true;
                            OnPlayerDetected();
                            break;
                        }
                    }
                }
            }
        }
        // Compare the previous detection state with the current one
        if (previousDetection && !isPlayerDetected)
        {
            Debug.Log("Player Lost!");
            // You can define this function to decide what the enemy should do when the player is lost.
        }

    }
    private void OnPlayerDetected()
    {
        // Implement any behavior you want the creature to take upon detecting the player
        // For instance, if you want the creature to chase the player:
        agent.SetDestination(player.position);
        playerLastSeenPos = player.position;
        currentState = EnemyState.Chasing;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        // Draw a line from enemy to player for debugging
        if (player)
            Gizmos.DrawLine(transform.position, player.position);

        // Draw the field of view
        float halfFOV = fieldOfViewAngle / 2;
        Quaternion leftRayRotation = Quaternion.AngleAxis(-halfFOV, Vector3.up);
        Quaternion rightRayRotation = Quaternion.AngleAxis(halfFOV, Vector3.up);

        Vector3 leftRayDirection = leftRayRotation * transform.forward;
        Vector3 rightRayDirection = rightRayRotation * transform.forward;

        Gizmos.DrawRay(transform.position, leftRayDirection * detectionRange);
        Gizmos.DrawRay(transform.position, rightRayDirection * detectionRange);
    }
}
