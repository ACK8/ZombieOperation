using UnityEngine;
using UnityEngine.AI;
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
    [SerializeField]
    private LampColor lamp;
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private GameObject explosion;
    [SerializeField]
    private float attackDistance = 10f;
    [SerializeField]
    private float shotWait = 0.5f;

    public float maxTimeToDiscover = 1.0f; //ターゲットを発見して認識する最大時間
    public Transform eyePosition; //視界の位置
    public int hp = 100;
    public bool isAlive = true;
    public float[] waitTimes; //待機の最大時間
    public string[] targetTags; //ターゲットのタグ名
    public GameObject[] patrolObject; //巡回位置

    //銃関係
    public Transform[] firePoint; //銃口の位置
    public float bulletDistanceMax = 5.0f; //銃弾の最大射程

    private uint patrolNumber = 0;
    private float timeToDiscover = 0.0f;
    private float currntWaitTime = 0.0f;
    private NavMeshAgent navMeshAgent;
    private Transform playerTransform;
    private EEnemyState enemyState = EEnemyState.Patrol;
    private List<discoveryInfo> discoveryList = new List<discoveryInfo>();
    private Dictionary<int, float> temp = new Dictionary<int, float>();
    private Animator anim;
    private float animBlend = 0f;
    private float shotwaitCount = 0f;

    void Start()
    {
        anim = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.SetDestination(patrolObject[patrolNumber].transform.position);

        lamp.ColorChange(Color.green);

        if (patrolObject == null) print("patrolObject is null");
    }

    void Update()
    {
        //print(enemyState);
        switch (enemyState)
        {
            case EEnemyState.None:
                {
                    lamp.ColorChange(Color.green);
                }
                break;

            case EEnemyState.Wait:
                {
                    lamp.ColorChange(Color.green);

                    //animBlend = 0f;
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
                    navMeshAgent.Resume();
                    lamp.ColorChange(Color.green);
                }
                break;

            case EEnemyState.Chase:
                {
                    lamp.ColorChange(Color.yellow);

                    navMeshAgent.SetDestination(playerTransform.position);
                    navMeshAgent.Resume();
                    animBlend = 1f;

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
                    lamp.ColorChange(Color.red);

                    //ターゲットに向かせる
                    Vector3 targetPos = playerTransform.position;
                    targetPos.y = transform.position.y;
                    transform.LookAt(targetPos);
                    animBlend = 0f;

                    RaycastHit hit;
                    Vector3 targetDir = (playerTransform.position - eyePosition.position).normalized;

                    //Debug.DrawLine(eyePosition.position, (eyePosition.position + targetDir));

                    if (Physics.Raycast(eyePosition.position, targetDir, out hit))
                    {
                        if (hit.transform.GetInstanceID() != playerTransform.GetInstanceID())
                        {
                            enemyState = EEnemyState.Chase;
                        }
                    }
                }

                Vector3 direction = playerTransform.position - eyePosition.position;

                if (Vector3.Distance(playerTransform.position, eyePosition.position) < attackDistance)
                {
                    float x = Random.Range(0.03f, -0.03f);
                    float y = Random.Range(0.03f, -0.03f);
                    float z = Random.Range(0.03f, -0.03f);
                    Quaternion rand = new Quaternion(x, y, z, 1.0f);

                    for (int i = 0; i < firePoint.Length; i++)
                    {
                        //Vector3 targetDir = (playerTransform.position - firePoint[i].position).normalized;
                        //Vector3 dir = rand * targetDir;

                        //Vector3 start = firePoint[i].position;
                        //Vector3 end = start + dir * bulletDistanceMax;
                        //Debug.DrawLine(start, end);

                        RaycastHit hitfire;
                        if (Physics.Raycast(eyePosition.position, direction.normalized, out hitfire))
                        {
                            if (hitfire.collider.tag == "Player")
                            {
                                BulletShot(i, rand);
                            }
                        }
                    }
                }
                else
                {
                    RaycastHit hit;
                    if (Physics.Raycast(eyePosition.position, direction.normalized, out hit))
                    {

                    }
                }
                break;
        }

        //アニメーションブレンド
        var fwdSpeed = Vector3.Dot(navMeshAgent.velocity.normalized, transform.forward);
        anim.SetFloat("Blend", fwdSpeed);
    }

    void BulletShot(int index, Quaternion rand)
    {
        shotwaitCount += Time.deltaTime;

        if (shotWait < shotwaitCount)
        {
            shotwaitCount = 0f;
            Instantiate(bullet, firePoint[index].position, firePoint[index].rotation * rand);
        }
    }

    //ダメージ
    void AddDamage(int val)
    {
        hp -= val;

        if (hp <= 0)
        {
            isAlive = false;
            hp = 0;

            GameObject p = Instantiate(explosion, transform.position + new Vector3(0f, 1f, 0f), explosion.transform.rotation) as GameObject;
            Destroy(p, 4f);

            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider hit)
    {
        if (!(enemyState == EEnemyState.Patrol)) return;

        //目的地に着いた
        if (hit.gameObject.name == patrolObject[patrolNumber].name)
        {
            enemyState = EEnemyState.Wait;
        }

        if (hit.tag == "ZombieBody")
        {
            AddDamage(10);
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
