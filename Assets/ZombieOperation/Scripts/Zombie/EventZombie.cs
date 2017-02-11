using UnityEngine;
using System.Collections;

public class EventZombie : MonoBehaviour
{
    public bool isZombie = false;
    public float zombieChangeTime = 2.5f;    //ゾンビに変化する時間

    [HideInInspector]
    public float injectionVolume = 0f;   //ゾンビ薬の注入量

    private InjectionVolumeUI injectionUI;

    void Start()
    {
        injectionUI = GameObject.Find("InjectionVolumeUI").GetComponent<InjectionVolumeUI>();
        injectionUI.SetValueRange(0f, zombieChangeTime);
    }

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
                injectionUI.SetVolume(injectionVolume, Color.green);
            }
        }
    }
    void OnTriggerEnter(Collider hit)
    {
        //注入量を表示
        if (hit.tag == "Injection")
        {
            injectionUI.SwitchDisplay(true);
        }
    }

    void OnTriggerExit(Collider hit)
    {
        //注入量UIを非表示
        injectionUI.SwitchDisplay(false);
    }
}
