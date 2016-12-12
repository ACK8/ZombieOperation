using UnityEngine;
using System.Collections;

public class Zombie : MonoBehaviour
{
    [SerializeField]
    private CapsuleCollider capsuleCol;
    [SerializeField]
    private float zombieChangeTime = 2.5f;    //注射時、ゾンビに変化する時間
    [SerializeField]
    private float attackAnimRate; //攻撃が有効になるアニメーション時間
    [SerializeField]
    private bool _isZombie = false;

    private NavMeshAgent navMesh;
    private Animator anim;
    private Vector3 targetPos = Vector3.zero;
    private Vector3 seledtedTarget = Vector3.zero;
    private GameObject destructionTarget = null;
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
        if (_isZombie)
        {
            //目的地に着くと待機
            if (Vector3.Distance(transform.position, seledtedTarget) < 0.6f && _isMove)
            {
                Wait();
            }

            if(operatingType == OperatingType.Following)
            {
                navMesh.SetDestination(seledtedTarget);
            }

            Animation();

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
        if (destructionTarget)
        {
            if (Vector3.Distance(transform.position, destructionTarget.transform.position) <= 2.0f && _isMove)
            {
                _isMove = false;
                navMesh.Stop();
                navMesh.speed = 0f;
                anim.SetTrigger("Attack");
            }
            else
            {
                Move(destructionTarget.transform.position);
            }
        }
        else
        {
            if (!destructionFlag)
            {
                isDestruction = false;
                Wait();
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
        navMesh.SetDestination(transform.position);
        anim.SetFloat("Blend", 0.0f);

    }

    //ゾンビ誘導処理
    public void Move(Vector3 target)
    {
        operatingType = OperatingType.Move;
        _isMove = true;
        navMesh.Resume();
        navMesh.speed = navSpeed;
        navMesh.SetDestination(target);

        anim.SetFloat("Blend", 1.0f);

        seledtedTarget = target;
    }

    //プレイヤーに追従
    public void Following(ref Transform playerPos)
    {
        operatingType = OperatingType.Following;
        _isMove = true;
        navMesh.Resume();
        navMesh.speed = navSpeed;
        anim.SetFloat("Blend", 1.0f);

        seledtedTarget = playerPos.position;
    }

    //攻撃
    public void Destruction(GameObject target)
    {
        operatingType = OperatingType.Attack;
        destructionTarget = null;
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
            _isZombie = true;
            anim.SetBool("GetUp", true);
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
        if (hit.tag == "Injection" && !_isZombie)
        {
            injectionVolume += Time.deltaTime;
        }
    }

    //ボーンについているスクリプトから呼ばれる
    public void HitCollider(Collision hit)
    {
        print(hit.collider.name);
    }

    //ボーンについているスクリプトから呼ばれる
    public void HitTrigger(Collider hit)
    {
        print(hit.name);
    }

    //移動しているか(誘導用)
    public bool isMove
    {
        get { return _isMove; }
        set { _isMove = value; }
    }

    //死体かゾンビかの状態
    public bool isZombie
    {
        get { return _isZombie; }
        set { _isZombie = value; }
    }
}
