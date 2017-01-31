using UnityEngine;
using System.Collections;

public class EventZombie : MonoBehaviour
{
    public bool isZombie = false;
    public float zombieChangeTime = 2.5f;    //ゾンビに変化する時間

    [HideInInspector]
    public float injectionVolume = 0f;   //ゾンビ薬の注入量

    void Update()
    {
        if (!isZombie)
        {
            if (zombieChangeTime <= injectionVolume)
            {
                isZombie = true;
            }
        }
    }

    void OnTriggerStay(Collider hit)
    {
        if (hit.tag == "Injection")
        {
            //注射
            if (!isZombie)
            {
                injectionVolume += Time.deltaTime;
            }
        }
    }
}
