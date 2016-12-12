using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public float[] waitTimes;
    public Vector3[] patrolPositions;
    public GameObject[] patrolObject;

    private uint patrolNumber = 0;
    private float currntWaitTime = 0;
    private NavMeshAgent navMeshAgent;

    void Start ()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.SetDestination(patrolPositions[patrolNumber]);
        currntWaitTime = waitTimes[patrolNumber];
    }
	
	void Update ()
    {
        if(patrolNumber >= patrolPositions.Length)
        {
            patrolNumber = 0;
        }

        if(currntWaitTime >= waitTimes[patrolNumber])
        {
            navMeshAgent.SetDestination(patrolPositions[patrolNumber]);
            currntWaitTime = 0.0f;
        }
        else
        {
            currntWaitTime += Time.deltaTime;   
        }
    }
}
