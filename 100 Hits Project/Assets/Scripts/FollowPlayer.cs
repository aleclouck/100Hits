using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{

    UnityEngine.AI.NavMeshAgent agent;
    Transform target;
    public int radius = 5;
    private Animator gAnim; // for the waiting animation
    private bool dead;
    Collider m_Collider;

    // Use this for initialization
    void Start ()
    {
        dead = false;
        gAnim = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        m_Collider = GetComponent<Collider>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        dead = GetComponent<aiHealth>().EnemyDeath;
        if (!dead) //death cheak
        {
            if (Vector3.Distance(transform.position, target.position) > radius)
            {
                agent.isStopped = false;
                agent.SetDestination(target.position);
                gAnim.SetBool("IsWaiting", false);
            }

            else
            {
                agent.isStopped = true;
                gAnim.SetBool("IsWaiting", true);
            }
        }
        else
        {
            agent.isStopped = true;
            m_Collider.enabled = false;
        }
    }
       
}
