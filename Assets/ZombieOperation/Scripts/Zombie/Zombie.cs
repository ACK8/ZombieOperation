using UnityEngine;
using System.Collections;

public class Zombie : MonoBehaviour
{
    public bool isZombie = false;
    public bool isStrengthZombie = false;
    public float zombieChangeTime = 100f;    //注射時、ゾンビに変化する時間
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
    [SerializeField]
    private GameObject[] particles;

    protected UnityEngine.AI.NavMeshAgent navMesh;
    protected Animator anim;
    protected Transform seledtedTarget = null;
    protected OperatingType operatingType;
    //protected int _hp = 100;
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
    private bool isLiftUpComp = false;  //隔壁持ち上げ完了

    void Start()
    {
        particles[0].gameObject.SetActive(false);
        particles[1].gameObject.SetActive(false);

        navMesh = GetComponent<UnityEngine.AI.NavMeshAgent>();
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

            //パーティクル切り替え
            if (isStrengthZombie)
            {
                //強化ゾンビ
                particles[0].gameObject.SetActive(false);
                particles[1].gameObject.SetActive(true);
            }
            else
            {
                //通常ゾンビ
                particles[0].gameObject.SetActive(true);
            }

            //アニメーションアップデート
            Animation();
            //破壊アップデート
            DestructionUpdate();
            //認証アップデート
            AuthenticationUpdate();
            //隔壁持ち上げアップデート
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
            //目的地に到着したら攻撃開始
            if (Vector3.Distance(transform.position, moveTargetPos.transform.position) <= 0.5f)
            {
                _isMove = false;
                //障害物の方に向く
                transform.LookAt(new Vector3(destructionTarget.transform.position.x, transform.position.y, destructionTarget.transform.position.z));
                navMesh.Stop();
                navMesh.speed = 0f;
                anim.SetTrigger("Attack");
            }
            //目的地に移動
            else
            {
                Move(moveTargetPos.transform);
            }
        }
        else
        {
            //破壊終了すると待機
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
        //認証ターゲットが指定されている＆認証未完了
        if (authenticationMachinePos != null && !isAuthenticationComp)
        {
            //目的地に到着したら認証開始
            if (Vector3.Distance(transform.position, authenticationMachinePos.transform.position) <= 0.4f)
            {
                _isMove = false;
                navMesh.Stop();
                navMesh.speed = 0f;
                navMesh.updateRotation = true;
                transform.LookAt(new Vector3(lookatPosition.x, transform.position.y, lookatPosition.z));
                anim.SetTrigger("Authentication");
                isAuthenticationComp = true;    //認証完了
            }
            else
            {
                //目的地に移動
                Move(authenticationMachinePos);
            }
        }
        else
        {
            //認証終了すると待機
            if (!isAuthenticationComp)
            {
                Wait();
                isAuthenticationComp = true;    //認証完了
            }
        }
    }

    //隔壁持ち上げ
    void LiftBulkheadUpdate()
    {
        //持ち上げターゲットが指定されている＆持ち上げ未完了
        if (BulkheadPos != null && !isLiftUpComp && isStrengthZombie)
        {
            //目的地に到着すると隔壁を持ち上げる
            if (Vector3.Distance(transform.position, BulkheadPos.transform.position) <= 0.4f)
            {
                _isMove = false;
                isLiftUpComp = true;
                navMesh.Stop();
                navMesh.speed = 0f;
                navMesh.updateRotation = true;
                transform.LookAt(new Vector3(lookatPosition.x, transform.position.y, lookatPosition.z));
                capsuleCol.enabled = true;
                anim.SetTrigger("Lift");
            }
            else
            {
                Move(BulkheadPos);
            }
        }
        else
        {
            if (!isLiftUpComp)
            {
                Wait();
                isLiftUpComp = true;
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
        navMesh.SetDestination(seledtedTarget.position);
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
        isLiftUpComp = false;
    }

    //命令前の初期化
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

        //認証アニメーション
        if (stateInfo.IsName("Base Layer.Authentication"))
        {
            //アニメーション再生時、指定フレーム範囲内でゾンビの前にあるコライダーON
            if (authenticationAnimRate <= stateInfo.normalizedTime && stateInfo.normalizedTime < (authenticationAnimRate + 0.2f))
            {
                capsuleCol.enabled = true;
            }
        }
        else
        {
            capsuleCol.enabled = false;
        }

        //攻撃アニメーション
        if (stateInfo.IsName("Base Layer.Attack"))
        {
            //アニメーション再生時、指定フレーム範囲内でゾンビの前にあるコライダーON
            if (attackAnimRate <= stateInfo.normalizedTime && stateInfo.normalizedTime < (attackAnimRate + 0.1f))
            {
                capsuleCol.enabled = true;
            }
            else
            {
                capsuleCol.enabled = false;
            }
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

    //HPを減らす
    public void DecrementHP(int val)
    {
        injectionVolume -= val;

        if (injectionVolume <= 0)
        {
            _isAlive = false;
            injectionVolume = 0;
        }
    }

    void OnTriggerEnter(Collider hit)
    {
        if (hit.tag == "Bullet")
        {
            DecrementHP(10);
        }

        //障害物破壊
        if (hit.tag == "Object" && hit.gameObject == destructionTarget)
        {
            hit.gameObject.GetComponent<DestructionObject>().DecreaseEnduranceValue(hit);
        }

        //注入量を表示
        if (hit.tag == "Injection")
        {
            injectionUI.SwitchDisplay(true);
        }
    }

    void OnTriggerStay(Collider hit)
    {
        if (hit.tag == "Injection")
        {
            InjectionCollision ic = hit.GetComponent<InjectionCollision>();
            MedicineType t = ic.GetMedicineType();

            //注射
            if (!isZombie && t == MedicineType.Zombie)
            {
                //蘇生薬を持っているとき注入量を増やす
                if (0 < ic.zombieMedicineNumber)
                {
                    injectionVolume += Time.deltaTime * 50f;
                    injectionUI.SetVolume(injectionVolume);
                }
            }

            //ゾンビ強化
            if (isZombie && !isStrengthZombie && t == MedicineType.Strength)
            {
                //強化薬を持っているとき注入量を増やす
                if (0 < ic.strengthMedicineNumber)
                {
                    strengthVolume += Time.deltaTime * 50f;
                    injectionUI.SetVolume(strengthVolume);
                }
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

    //HPの代わりに蘇生薬注入量
    public int hp
    {
        get { return (int)injectionVolume; }
    }

    //生体認証に使用するID
    public int zombieID
    {
        get { return id; }
    }
}
