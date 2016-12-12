using UnityEngine;
using System.Collections;

//注射器関係の処理をする
public class Syringe : MonoBehaviour
{
    public GameObject injectionColliderObject; //注射の当たり判定が入っているオブジェクト

    private CapsuleCollider injectionJudgment; //注射中を判定する用のコライダー

    void Awake()
    {
        injectionJudgment = injectionColliderObject.GetComponent<CapsuleCollider>();
        injectionJudgment.enabled = false;
    }

	void Update ()
    {
	
	}

    //注射をしている状態にする
    public void OnInjection()
    {
        injectionJudgment.enabled = true;
    }

    //注射をしていない状態にする
    public void OffInjection()
    {
        injectionJudgment.enabled = false;
    }
}
