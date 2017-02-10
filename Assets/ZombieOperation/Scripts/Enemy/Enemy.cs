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
    private LampColor lamp;     //ランプ
    [SerializeField]
    private GameObject bullet;  //弾
    [SerializeField]
    private GameObject explosion;   //爆発パーティクル
    [SerializeField]
    private float attackDistance = 10f; //攻撃を開始するプレイヤーとの距離
    [SerializeField]
    private float shotWait = 0.5f;
    [SerializeField]
    private float memoryTime = 10f; //プレイヤーの位置を記憶する時間。これ以上になるとパトロール状態に

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
    private float memoryTimeCount = 0f;

    void Start()
    {
        anim = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.SetDestination(patrolObject[patrolNumber].transform.position);

        lamp.ColorChange(Color.green);

        if (patrolObject == null) print("PatrolObject is null");
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

                    animBlend = 0f;
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
                    navMeshAgent.Resume();
                    lamp.ColorChange(Color.green);
                    animBlend = 1f;
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
                        else
                        {
                            memoryTimeCount += Time.deltaTime;

                            //プレイヤーを見失って、一定時間以上になるとパトロール状態に
                            if (memoryTime <= memoryTimeCount)
                            {
                                memoryTimeCount = 0f;
                                navMeshAgent.SetDestination(patrolObject[patrolNumber].transform.position);
                                enemyState = EEnemyState.Patrol;
                            }
                        }
                    }
                }
                break;

            case EEnemyState.Attack:
                {
                    lamp.ColorChange(Color.red);
                    memoryTimeCount = 0f;

                    //ターゲットに向かせる
                    Vector3 targetPos = playerTransform.position;
                    targetPos.y = transform.position.y;
                    transform.LookAt(targetPos);
                    animBlend = 0f;

                    RaycastHit hit;
                    Vector3 targetDir = (playerTransform.position - eyePosition.position).normalized;

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
                    RaycastHit hitfire;
                    if (Physics.Raycast(eyePosition.position, direction.normalized, out hitfire))
                    {
                        if (hitfire.collider.tag == "Player")
                        {
                            BulletShot();
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
        anim.SetFloat("Blend", Mathf.Lerp(anim.GetFloat("Blend"), animBlend, 0.3f));
    }

    //弾を発射
    void BulletShot()
    {
        for (int i = 0; i < firePoint.Length; i++)
        {
            float x = Random.Range(0.03f, -0.03f);
            float y = Random.Range(0.03f, -0.03f);
            float z = Random.Range(0.03f, -0.03f);
            Quaternion rand = new Quaternion(x, y, z, 1.0f);

            shotwaitCount += Time.deltaTime;

            if (shotWait < shotwaitCount)
            {
                shotwaitCount = 0f;
                Instantiate(bullet, firePoint[i].position, firePoint[i].rotation * rand);
            }
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

            //爆発エフェクト
            GameObject p = Instantiate(explosion, transform.position + new Vector3(0f, 1f, 0f), explosion.transform.rotation) as GameObject;
            Destroy(p, 4f);

            //自身を削除
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider hit)
    {
        //ゾンビからのダメージ
        if (hit.tag == "ZombieBody")
        {
            AddDamage(5);
        }

        if (!(enemyState == EEnemyState.Patrol)) return;

        //目的地に着いた
        if (hit.gameObject.name == patrolObject[patrolNumber].name)
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
