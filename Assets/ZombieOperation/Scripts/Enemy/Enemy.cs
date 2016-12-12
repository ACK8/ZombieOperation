using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public float maxTimeToDiscover = 1.0f;
    public float[] waitTimes;
    public GameObject[] patrolObject;
    public Transform eyePosition;

    private bool isChase = false;
    private bool isPatrol = true;
    private bool isMove = true;
    private uint patrolNumber = 0;
    private float timeToDiscover = 0.0f;
    private float currntWaitTime = 0.0f;
    private NavMeshAgent navMeshAgent;
    private Transform playerTransform;

    void Start ()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.SetDestination(patrolObject[patrolNumber].transform.position);
    }
	
	void Update ()
    {
        if(isChase)
        {
            navMeshAgent.SetDestination(playerTransform.position);
        }

        //巡回処理
        if (isPatrol)
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

    void OnTriggerStay(Collider collider)
    {
        //プレイヤーを発見
        if (isChase) return;

        Debug.Log("target = " + collider.gameObject.tag);

        if (collider.gameObject.tag == "Player")
        {
            Debug.Log("Box発見");

            RaycastHit hit;
            Vector3 direction = collider.gameObject.transform.position - eyePosition.position;
            if (Physics.Raycast(eyePosition.position, direction.normalized, out hit))
            {
                Debug.DrawRay(eyePosition.position, direction.normalized, Color.red);

                if (hit.transform.tag == "Player")
                {
                    navMeshAgent.Stop();

                    if (timeToDiscover > maxTimeToDiscover)
                    {
                        isChase = true;
                        isMove = true;
                        isPatrol = false;
                        playerTransform = hit.transform;
                        timeToDiscover = 0.0f;
                        navMeshAgent.Resume();
                    }
                    else
                    {
                        timeToDiscover += Time.deltaTime;
                    }
                }
            }
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (!isChase)
        {
            if (collider.gameObject.tag == "Player")
            {
                timeToDiscover = 0.0f;
                navMeshAgent.Resume();
            }
        }
    }
}
