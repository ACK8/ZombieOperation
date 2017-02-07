using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct discoveryInfo
{
    public GameObject targetObject;
    public float timeToDiscover;

    public discoveryInfo(GameObject target, float time)
    {
        targetObject = target;
        timeToDiscover = time;
    }

    public void SetTimeToDiscover(float time)
    {
        timeToDiscover = time;
    }

    public void AddTimeToDiscover(float time)
    {
        timeToDiscover += time;
    }
}

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
    private UnityEngine.AI.NavMeshAgent navMeshAgent;
    private Transform playerTransform;
    private EEnemyState enemyState = EEnemyState.Patrol;
    private List<discoveryInfo> discoveryList = new List<discoveryInfo>();
    private Dictionary<int, float> temp = new Dictionary<int, float>();
    private Animator anim;
    private float animBlend = 0f;


    void Start()
    {
        anim = GetComponent<Animator>();
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        navMeshAgent.SetDestination(patrolObject[patrolNumber].transform.position);
    }

    void Update()
    {
        switch (enemyState)
        {
            case EEnemyState.None:
                {

                }
                break;

            case EEnemyState.Wait:
                {
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
                }
                break;

            case EEnemyState.Patrol:
                {
                    animBlend = 1f;
                }
                break;

            case EEnemyState.Chase:
                {
                    navMeshAgent.SetDestination(playerTransform.position);
                    navMeshAgent.Resume();

                    //攻撃状態に移行
                    RaycastHit hit;
                    Vector3 targetDir = (playerTransform.position - eyePosition.position).normalized;
                    if (Physics.Raycast(eyePosition.position, targetDir, out hit))
                    {
                        if (hit.transform.GetInstanceID() == playerTransform.GetInstanceID())
                        {
                            navMeshAgent.Stop();
                            enemyState = EEnemyState.Attack;
                        }
                    }
                }
                break;

            case EEnemyState.Attack:
                {
                    //ターゲットに向かせる
                    Vector3 targetPos = playerTransform.position;
                    targetPos.y = transform.position.y;
                    transform.LookAt(targetPos);

                    RaycastHit hit;
                    Vector3 targetDir = (playerTransform.position - eyePosition.position).normalized;

                    Debug.DrawLine(eyePosition.position, (eyePosition.position + targetDir));

                    if (Physics.Raycast(eyePosition.position, targetDir, out hit))
                    {
                        if (hit.transform.GetInstanceID() != playerTransform.GetInstanceID())
                        {
                            enemyState = EEnemyState.Chase;
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

        anim.SetFloat("Blend", Mathf.Lerp(anim.GetFloat("Blend"), animBlend, 0.5f));
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
                    //レイで視界判断
                    RaycastHit hit;
                    Vector3 direction = collider.gameObject.transform.position - eyePosition.position;
                    if (Physics.Raycast(eyePosition.position, direction.normalized, out hit))
                    {
                        if (hit.transform.tag == targetTags[i])
                        {
                            bool isTarget = false;
                            bool isAdd = true;
                            int id = collider.gameObject.GetInstanceID();

                            //前の当たり判定でターゲットが視界に入っているのを確認
                            foreach (int key in temp.Keys)
                            {
                                if (key == id)
                                {
                                    isTarget = true;
                                    isAdd = false;
                                    navMeshAgent.Stop();
                                    animBlend = 0f;
                                }
                            }

                            //ターゲットが視界に入り続けているか確認
                            if (isTarget)
                            {
                                if (temp[id] > maxTimeToDiscover)
                                {
                                    enemyState = EEnemyState.Chase;
                                    playerTransform = hit.transform;
                                    navMeshAgent.Resume();
                                    temp.Clear();

                                    Debug.Log(temp.Count);
                                }
                                else
                                {
                                    temp[id] += Time.deltaTime;
                                }
                            }

                            //新たに対象を発見
                            if (isAdd)
                            {
                                temp.Add(id, 0.0f);
                            }
                        }
                    }
                }
            }
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (enemyState == EEnemyState.Patrol)
        {
            int id = collider.gameObject.GetInstanceID();
            temp.Remove(id);
        }
    }
}
