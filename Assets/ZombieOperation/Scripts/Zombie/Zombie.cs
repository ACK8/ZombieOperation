using UnityEngine;
using System.Collections;

public class Zombie : MonoBehaviour
{
    public bool isZombie = false;
    public bool isStrengthZombie = false;
    public float zombieChangeTime = 2.5f;    //注射時、ゾンビに変化する時間
    [HideInInspector]
    public float injectionVolume = 0f;   //ゾンビ薬の注入量
    [HideInInspector]
    public float strengthVolume = 0f;   //強化薬の注入量

    [SerializeField]
    private CapsuleCollider capsuleCol;
    [SerializeField]
    protected float attackAnimRate; //攻撃が有効になるアニメーション時間
    [SerializeField]
    protected float authenticationAnimRate; //認証が有効になるアニメーション時間
    [SerializeField]
    private int id = 0;

    protected NavMeshAgent navMesh;
    protected Animator anim;
    protected Transform seledtedTarget = null;
    protected OperatingType operatingType;
    protected int _hp = 100;
    protected float navSpeed = 0f;
    protected bool _isMove = false;
    protected bool _isAlive = true;
    protected bool isChange = false;
    protected bool destructionFlag = false;
    private InjectionVolumeUI injectionUI;
    private Transform authenticationMachinePos = null;
    private Transform BulkheadPos = null;
    private Vector3 lookatPosition;
    private GameObject destructionTarget = null;
    private GameObject moveTargetPos = null;
    private bool isAuthenticationComp = false;  //認証完了
    private bool isLift = false;

    void Start()
    {
        navMesh = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        navSpeed = navMesh.speed;
        capsuleCol.enabled = false;

        injectionUI = GameObject.Find("InjectionVolumeUI").GetComponent<InjectionVolumeUI>();
        injectionUI.SetValueRange(0f, zombieChangeTime);
    }

    void Update()
    {
        //注射処理
        Injection();

        if (isZombie)
        {
            if (seledtedTarget != null)
            {
                //目的地に着くと待機
                if (Vector3.Distance(transform.position,
                    new Vector3(seledtedTarget.position.x, transform.position.y, seledtedTarget.position.z)) <= 0.6f && _isMove)
                {
                    Wait();
                }

                if (operatingType == OperatingType.Following)
                {
                    navMesh.SetDestination(seledtedTarget.position);
                }
            }

            Animation();
            DestructionUpdate();
            AuthenticationUpdate();
            LiftBulkheadUpdate();
        }
        else
        {
            //倒れた状態
            anim.Play("GetUp", -1, 0.0f);
        }
    }

    //障害物破壊
    void DestructionUpdate()
    {
        if (moveTargetPos != null)
        {
            if (Vector3.Distance(transform.position, moveTargetPos.transform.position) <= 0.5f)
            {
                _isMove = false;
                transform.LookAt(new Vector3(destructionTarget.transform.position.x, transform.position.y, destructionTarget.transform.position.z));
                navMesh.Stop();
                navMesh.speed = 0f;
                anim.SetTrigger("Attack");
            }
            else
            {
                Move(moveTargetPos.transform);
            }
        }
        else
        {
            if (!destructionFlag)
            {
                Wait();
                destructionFlag = true;
            }
        }
    }

    //生体認証
    void AuthenticationUpdate()
    {
        if (authenticationMachinePos != null && !isAuthenticationComp)
        {
            if (Vector3.Distance(transform.position, authenticationMachinePos.transform.position) <= 0.4f)
            {
                _isMove = false;
                navMesh.Stop();
                navMesh.speed = 0f;
                navMesh.updateRotation = true;
                transform.LookAt(new Vector3(lookatPosition.x, transform.position.y, lookatPosition.z));
                anim.SetTrigger("Authentication");
                isAuthenticationComp = true;
            }
            else
            {
                Move(authenticationMachinePos);
            }
        }
        else
        {
            if (!isAuthenticationComp)
            {
                Wait();
                isAuthenticationComp = true;
            }
        }
    }

    //隔壁持ち上げ
    void LiftBulkheadUpdate()
    {
        if (BulkheadPos != null)
        {
            if (Vector3.Distance(transform.position, BulkheadPos.transform.position) <= 0.4f)
            {
                if (!isLift)
                {
                    _isMove = false;
                    isLift = true;
                    navMesh.Stop();
                    navMesh.speed = 0f;
                    navMesh.updateRotation = true;
                    transform.LookAt(new Vector3(lookatPosition.x, transform.position.y, lookatPosition.z));
                    capsuleCol.enabled = true;
                    anim.SetTrigger("Lift");
                }
            }
            else
            {
                Move(BulkheadPos);
            }
        }
        else
        {
            if (!isAuthenticationComp)
            {
                //倒れた状態
                //anim.Play("GetUp", -1, 0.0f);
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
    public void Attack(GameObject target)
    {
        Init();

        operatingType = OperatingType.Attack;
        moveTargetPos = target.GetComponent<DestructionObject>().destructionPosition;
        destructionTarget = target;
        _isMove = true;
        destructionFlag = false;
    }

    //生体認証
    public void Authentication(GameObject pos, Vector3 lookatPos)
    {
        Init();

        authenticationMachinePos = pos.transform;
        lookatPosition = lookatPos;
        _isMove = true;
        isAuthenticationComp = false;
    }

    //隔壁持ち上げ
    public void LiftBulkhead(GameObject pos, Vector3 lookatPos)
    {
        Init();

        BulkheadPos = pos.transform;
        lookatPosition = lookatPos;
        _isMove = true;
        isLift = false;
    }

    void Init()
    {
        BulkheadPos = null;
        moveTargetPos = null;
        destructionTarget = null;
        authenticationMachinePos = null;
        lookatPosition = Vector3.zero;
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

        //攻撃アニメーション
        if (stateInfo.IsName("Base Layer.Attack"))
        {
            if (attackAnimRate <= stateInfo.normalizedTime && stateInfo.normalizedTime < (attackAnimRate + 0.15f))
            {
                capsuleCol.enabled = true;
            }
            else
            {
                capsuleCol.enabled = false;
            }
        }

        //認証アニメーション
        if (stateInfo.IsName("Base Layer.Authentication"))
        {
            if (authenticationAnimRate <= stateInfo.normalizedTime && stateInfo.normalizedTime < (authenticationAnimRate + 0.2f))
            {
                capsuleCol.enabled = true;
            }
        }
        else
        {
            capsuleCol.enabled = false;
        }

        //倒れる
        if (!_isAlive)
        {
            anim.SetTrigger("Collapse");
        }
    }

    //注射処理
    void Injection()
    {
        //ゾンビの時強化
        if (isZombie)
        {
            if (zombieChangeTime <= strengthVolume)
            {
                isStrengthZombie = true;

            }
        }
        //死体の時ゾンビ化
        else
        {
            if (zombieChangeTime <= injectionVolume)
            {
                isZombie = true;
                anim.SetBool("GetUp", false);
            }
        }
    }

    public void DecrementHP(int val)
    {
        _hp -= val;

        if (_hp <= 0)
        {
            _isAlive = false;
            _hp = 0;
        }
    }

    void OnTriggerEnter(Collider hit)
    {
        //障害物破壊
        if (hit.tag == "Object" && hit.gameObject == destructionTarget)
        {
            hit.gameObject.GetComponent<DestructionObject>().DecreaseEnduranceValue();
        }

        if (hit.tag == "Injection")
        {
            injectionUI.SwitchDisplay(true);
        }
    }

    void OnTriggerStay(Collider hit)
    {
        if (hit.tag == "Injection")
        {
            MedicineType t = hit.GetComponent<InjectionCollision>().GetMedicineType();

            //注射
            if (!isZombie && t == MedicineType.Zombie)
            {
                injectionVolume += Time.deltaTime;
                injectionUI.SetVolume(injectionVolume);
            }

            //ゾンビ強化
            if (isZombie && t == MedicineType.Strength)
            {
                strengthVolume += Time.deltaTime;
                injectionUI.SetVolume(strengthVolume);
            }
        }
    }

    void OnTriggerExit(Collider hit)
    {
        injectionUI.SwitchDisplay(false);
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

    public int hp
    {
        get { return _hp; }
    }

    public int zombieID
    {
        get { return id; }
    }
}
