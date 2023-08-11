using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public CreatureData creatureData;
    private NavMeshAgent agent;

    public float detectionRange = 10.0f;  // Max distance for detection
    public float fieldOfViewAngle = 45.0f;  // Angle for the field of view (half total angle)
    public LayerMask detectionMask;  // Masks for detection (includes Player and Obstacles)
    public int numberOfRays = 10;  // Number of rays in field of view

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        DetectPlayer();
    }

    private void DetectPlayer()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float angleBetweenEnemyAndPlayer = Vector3.Angle(transform.forward, directionToPlayer);

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
                            OnPlayerDetected();
                            break;
                        }
                    }
                }
            }
        }
    }

    private void OnPlayerDetected()
    {
        // Implement any behavior you want the creature to take upon detecting the player
        // For instance, if you want the creature to chase the player:
        agent.SetDestination(player.position);
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
