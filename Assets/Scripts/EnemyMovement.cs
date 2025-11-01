using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{

    public Transform player;
    private NavMeshAgent navMeshAgent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        // ADD || player isInvulnerable = true LATER AND MAKE IT WORK
        {
            navMeshAgent.SetDestination(player.position);
        }
        else
        {
            navMeshAgent.isStopped = true;
        }
    }
}
