using UnityEngine;
using System.Collections;

public class Zombie : MonoBehaviour
{
    public bool isZombie = false;

    [SerializeField]
    private CapsuleCollider capsuleCol;
    [SerializeField]
    private float zombieChangeTime = 2.5f;    //注射時、ゾンビに変化する時間
    [SerializeField]
    private float attackAnimRate; //攻撃が有効になるアニメーション時間

    private NavMeshAgent navMesh;
    private Animator anim;
    private Vector3 targetPos = Vector3.zero;
    private Transform seledtedTarget = null;
    private GameObject destructionTarget = null;
    private GameObject destructionPos = null;
    private OperatingType operatingType;
    private float injectionVolume = 0f;   //ゾンビ薬の注入量
    private float navSpeed = 0f;
    private bool _isMove = false;
    private bool isChange = false;
    private bool isDestruction = false;
    private bool destructionFlag = false;

    void Start()
    {
        navMesh = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        navSpeed = navMesh.speed;
        capsuleCol.enabled = false;
    }

    void Update()
    {
        if (isZombie)
        {
            if (seledtedTarget != null)
            {
                //目的地に着くと待機
                if (Vector3.Distance(transform.position,
                    new Vector3(seledtedTarget.position.x, transform.position.y, seledtedTarget.position.z)) <= 0.5f && _isMove)
                {
                    Wait();
                }

                if (operatingType == OperatingType.Following)
                {
                    navMesh.SetDestination(seledtedTarget.position);
                }

                Animation();
            }

            DestructionUpdate();
        }
        else
        {
            //注射処理
            Injection();

            //倒れた状態
            anim.Play("GetUp", -1, 0.0f);
        }
    }

    //障害物破壊
    void DestructionUpdate()
    {
        if (destructionPos != null)
        {
            if (Vector3.Distance(transform.position, destructionPos.transform.position) <= 1.0f)
            {
                _isMove = false;
                transform.LookAt(new Vector3(destructionPos.transform.position.x, transform.position.y, destructionPos.transform.position.z));
                navMesh.Stop();
                navMesh.speed = 0f;
                anim.SetTrigger("Attack");
            }
            else
            {
                Move(destructionPos.transform);
            }
        }
        else
        {
            if (!destructionFlag)
            {
                Wait();
                isDestruction = false;
                destructionFlag = true;
            }
        }
    }

    //待機
    public void Wait()
    {
        operatingType = OperatingType.Wait;
        _isMove = false;
        navMesh.speed = 0f;
        navMesh.Stop();

        anim.SetFloat("Blend", 0.0f);
        navMesh.SetDestination(transform.position);
    }

    //ゾンビ誘導処理
    public void Move(Transform target)
    {
        operatingType = OperatingType.Move;
        _isMove = true;
        navMesh.Resume();
        navMesh.speed = navSpeed;
        navMesh.SetDestination(target.position);

        anim.SetFloat("Blend", 1.0f);

        seledtedTarget = target;
        navMesh.SetDestination(target.position);
    }

    //プレイヤーに追従
    public void Following(Transform playerPos)
    {
        operatingType = OperatingType.Following;
        _isMove = true;
        navMesh.Resume();
        navMesh.speed = navSpeed;
        anim.SetFloat("Blend", 1.0f);

        seledtedTarget = playerPos;
    }

    //攻撃
    public void Destruction(GameObject target)
    {
        operatingType = OperatingType.Attack;
        destructionPos = null;
        destructionPos = target.GetComponent<DestructionObject>().destructionPosition;
        destructionTarget = target;
        isDestruction = true;
        _isMove = true;
        destructionFlag = false;
    }

    //アニメーション
    void Animation()
    {
        //起き上がる
        if (!isChange)
        {
            isChange = true;
            anim.SetBool("GetUp", false);
        }

        anim.Update(0);
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("Base Layer.Attack"))
        {
            if (attackAnimRate <= stateInfo.normalizedTime && stateInfo.normalizedTime < (attackAnimRate + 0.1f))
            {
                capsuleCol.enabled = true;
            }
            else
            {
                capsuleCol.enabled = false;
            }
        }
    }

    //注射処理
    void Injection()
    {
        if (zombieChangeTime <= injectionVolume)
        {
            isZombie = true;
            anim.SetBool("GetUp", false);
        }
    }

    void OnTriggerEnter(Collider hit)
    {
        //障害物破壊
        if (hit.tag == "Object" && hit.gameObject == destructionTarget)
        {
            hit.gameObject.GetComponent<DestructionObject>().DecreaseEnduranceValue();
        }
    }

    void OnTriggerStay(Collider hit)
    {
        //注射
        if (hit.tag == "Injection" && !isZombie)
        {
            injectionVolume += Time.deltaTime;
        }
    }

    //ボーンについているスクリプトから呼ばれる
    public void CollisionEnter(Collision hit)
    {

    }

    //ボーンについているスクリプトから呼ばれる
    public void TriggerEnter(Collider hit)
    {

    }

    //移動しているか(誘導用)
    public bool isMove
    {
        get { return _isMove; }
        set { _isMove = value; }
    }
}
