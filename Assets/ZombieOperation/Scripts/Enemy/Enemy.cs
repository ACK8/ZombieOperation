using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public float[] waitTimes;
    public GameObject[] patrolObject;

    private bool isPatrol = true;
    private bool isMove = true;
    private uint patrolNumber = 0;
    private float currntWaitTime = 0.0f;
    private NavMeshAgent navMeshAgent;

    void Start ()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.SetDestination(patrolObject[patrolNumber].transform.position);
    }
	
	void Update ()
    {
        //巡回処理
        if(isPatrol)
        {
            if (!isMove)
            {
                //巡回位置切り替え
                if (currntWaitTime > waitTimes[patrolNumber])
                {
                    patrolNumber += 1;

                    if (patrolNumber >= patrolObject.Length)
                    {
                        patrolNumber = 0;
                    }

                    navMeshAgent.SetDestination(patrolObject[patrolNumber].transform.position);
                    currntWaitTime = 0.0f;
                    isMove = true;
                }
                else
                {
                    currntWaitTime += Time.deltaTime;
                }
            }
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        //目的地に着いた
        if (collider.gameObject.name == patrolObject[patrolNumber].name)
        {
            isMove = false;
        }
    }
}
