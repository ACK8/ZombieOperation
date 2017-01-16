﻿using UnityEngine;
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

    private InjectionVolumeUI injectionUI;
    private Transform authenticationMachinePos = null;
    private Vector3 Biometrics1MachinePos;
    protected NavMeshAgent navMesh;
    protected Animator anim;
    protected Transform seledtedTarget = null;
    protected GameObject destructionTarget = null;
    protected GameObject destructionPos = null;
    protected OperatingType operatingType;
    protected int _hp = 100;
    protected float navSpeed = 0f;
    protected bool _isMove = false;
    protected bool _isAlive = true;
    protected bool isChange = false;
    protected bool destructionFlag = false;
    private bool isAuthenticationComp = false;  //認証完了

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
        if (destructionPos != null)
        {
            if (Vector3.Distance(transform.position, destructionPos.transform.position) <= 0.7f)
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
                transform.LookAt(new Vector3(Biometrics1MachinePos.x, transform.position.y, Biometrics1MachinePos.z));
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
        operatingType = OperatingType.Attack;
        destructionPos = null;
        destructionPos = target.GetComponent<DestructionObject>().destructionPosition;
        destructionTarget = target;
        _isMove = true;
        destructionFlag = false;
    }

    //生体認証
    public void Authentication(Transform pos, Vector3 lookatPos)
    {
        authenticationMachinePos = pos;
        Biometrics1MachinePos = lookatPos;
        _isMove = true;
        isAuthenticationComp = false;
        print("authenticationMachinePos  " + authenticationMachinePos.position);
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
            if (attackAnimRate <= stateInfo.normalizedTime && stateInfo.normalizedTime < (attackAnimRate + 0.1f))
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
            if (!isZombie && t == MedicineType.Zombie && !isZombie)
            {
                injectionVolume += Time.deltaTime;
                injectionUI.SetVolume(injectionVolume);
            }

            //ゾンビ強化
            if (isZombie && t == MedicineType.Strength && isZombie)
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
