using UnityEngine;
using System.Collections;

public enum EEnemyState
{
    None,
    Wait,
    Patrol,
    Chase,
    Attack,
}

public class Enemy : MonoBehaviour
{
    public float maxTimeToDiscover = 1.0f; //ターゲットを発見して認識する最大時間
    public float[] waitTimes; //待機の最大時間
    public string[] targetTags; //ターゲットのタグ名
    public GameObject[] patrolObject; //巡回位置
    public Transform eyePosition; //視界の位置

    //銃関係
    public float bulletDistanceMax = 5.0f; //銃弾の最大射程
    public Transform firePoint; //銃口の位置

    private uint patrolNumber = 0;
    private float timeToDiscover = 0.0f;
    private float currntWaitTime = 0.0f;
    private NavMeshAgent navMeshAgent;
    private Transform playerTransform;
    private EEnemyState enemyState = EEnemyState.Patrol;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.SetDestination(patrolObject[patrolNumber].transform.position);
    }

    void Update()
    {
        Debug.Log(enemyState);

        switch (enemyState)
        {
            case EEnemyState.None:
                break;

            case EEnemyState.Wait:
                //巡回位置切り替え
                if (currntWaitTime > waitTimes[patrolNumber])
                {
                    patrolNumber += 1;
                    if (patrolNumber >= patrolObject.Length) patrolNumber = 0;

                    navMeshAgent.SetDestination(patrolObject[patrolNumber].transform.position);
                    currntWaitTime = 0.0f;
                    enemyState = EEnemyState.Patrol;
                }
                else
                {
                    currntWaitTime += Time.deltaTime;
                }

                break;

            case EEnemyState.Patrol:

                break;

            case EEnemyState.Chase:
                navMeshAgent.SetDestination(playerTransform.position);
                navMeshAgent.Resume();

                //攻撃状態に移行
                if (Vector3.Distance(playerTransform.position, firePoint.position) < bulletDistanceMax * 0.5f)
                {
                    //enemyState = EEnemyState.Attack;

                    RaycastHit hit;
                    Vector3 targetDir = (playerTransform.position - eyePosition.position).normalized;
                    if (Physics.Raycast(eyePosition.position, targetDir, out hit))
                    {
                        Debug.Log("hit" + hit.transform.GetInstanceID());
                        Debug.Log("playerTransform" + playerTransform.GetInstanceID());

                        if (hit.transform.GetInstanceID() == playerTransform.GetInstanceID())
                        {
                            enemyState = EEnemyState.Attack;
                        }
                    }
                }

                break;

            case EEnemyState.Attack:
                {
                    navMeshAgent.SetDestination(playerTransform.position);

                    NavMeshHit navMeshHit2;
                    if (!navMeshAgent.Raycast(playerTransform.position, out navMeshHit2))
                    {
                        enemyState = EEnemyState.Chase;
                        break;
                    }

                    navMeshAgent.Stop();



                    if (Vector3.Distance(playerTransform.position, firePoint.position) > bulletDistanceMax)
                    {
                        RaycastHit hit;
                        Vector3 targetDir = (playerTransform.position - eyePosition.position).normalized;

                        Debug.DrawLine(eyePosition.position, (eyePosition.position + targetDir));

                        if (Physics.Raycast(eyePosition.position, targetDir, out hit))
                        {
                            if (hit.transform.tag != playerTransform.tag)
                            {
                                //enemyState = EEnemyState.Chase;
                            }
                        }
                    }
                }


                /*
                Vector3 direction = playerTransform.position - eyePosition.position;

                if (Vector3.Distance(playerTransform.position, eyePosition.position) < 5.0f)
                {
                    float x = Random.Range(0.05f, -0.05f);
                    float y = Random.Range(0.05f, -0.05f);
                    float z = Random.Range(0.05f, -0.05f);

                    Vector3 targetDir = (playerTransform.position - firePoint.position).normalized;
                    Vector3 dir = new Quaternion(x, y, z, 1.0f) * targetDir;

                    Vector3 start = firePoint.position;
                    Vector3 end = start + dir * bulletDistanceMax;

                    Debug.DrawLine(start, end);

                    RaycastHit hitfire;
                    if (Physics.Raycast(eyePosition.position, direction.normalized, out hitfire))
                    {
                    }
                }
                else
                {
                    RaycastHit hit;
                    if (Physics.Raycast(eyePosition.position, direction.normalized, out hit))
                    {
                    }
                }
                */
                break;
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (!(enemyState == EEnemyState.Patrol)) return;

        //目的地に着いた
        if (collider.gameObject.name == patrolObject[patrolNumber].name)
        {
            enemyState = EEnemyState.Wait;
        }
    }

    void OnTriggerStay(Collider collider)
    {
        //視界処理
        if ((enemyState == EEnemyState.Patrol) || (enemyState == EEnemyState.Wait))
        {
            for (int i = 0; i < targetTags.Length; i++)
            {
                if (collider.gameObject.tag == targetTags[i])
                {
                    RaycastHit hit;
                    Vector3 direction = collider.gameObject.transform.position - eyePosition.position;
                    if (Physics.Raycast(eyePosition.position, direction.normalized, out hit))
                    {
                        //Debug.DrawRay(eyePosition.position, direction.normalized, Color.red);

                        if (hit.transform.tag == targetTags[i])
                        {
                            navMeshAgent.Stop();

                            if (timeToDiscover > maxTimeToDiscover)
                            {
                                enemyState = EEnemyState.Chase;
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
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (!(enemyState == EEnemyState.Chase))
        {
            for (int i = 0; i < targetTags.Length; i++)
            {
                if (collider.gameObject.tag == targetTags[i])
                {
                    timeToDiscover = 0.0f;
                    navMeshAgent.Resume();
                }
            }
        }
    }
}
