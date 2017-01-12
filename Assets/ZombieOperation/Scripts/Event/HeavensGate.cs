using UnityEngine;
using System.Collections;

public class HeavensGate : MonoBehaviour
{
    [SerializeField]
    private VRTK.VRTK_Control[] gateLever;
    [SerializeField]
    private Light[] gateLights;

    void Start()
    {

    }

    void Update()
    {
        for (int i = 0; i < gateLights.Length; i++)
        {
            gateLights[i].enabled = gateLever[i].IsPowerOn();
        }

        if (gateLever[0].IsPowerOn() && gateLever[1].IsPowerOn() && gateLever[2].IsPowerOn())
        {
            //Heavens Gate Opens
        }
    }
}
