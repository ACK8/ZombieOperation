using UnityEngine;
using System.Collections;

public class Cardkey : MonoBehaviour
{
    [SerializeField]
    private Door doorScript;    //ドアについているスクリプト
    [SerializeField]
    private int cardKeyID;  //カードキーのID

    private float time;
    private bool isTouched = false;

    void Start()
    {

    }

    void Update()
    {
        //カードをタッチしたらカウント開始
        if (isTouched)
        {
            time += Time.deltaTime;
        }

        //時間が来たら再びタッチできるように
        if (1.5f < time)
        {
            time = 0f;
            isTouched = false;
        }
    }

    void OnTriggerEnter(Collider hit)
    {
        if (hit.tag == "Key")
        {
            //カードキーIDとカードIDが同じ、カードが触れていない時にドアを施錠
            if (cardKeyID == hit.GetComponent<Key>().cardID && !isTouched)
            {
                isTouched = true;
                //ドアを動かす
                doorScript.MoveDoor();
            }
        }
    }
}
