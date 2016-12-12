using UnityEngine;
using System.Collections;

public class AutomaticDoorSensor : MonoBehaviour
{
    [SerializeField]
    private Door doorScript;

    private float intervalTime = 0f;
    private float closeintervalTime = 0f;
    private bool isStand = false;

    void Start()
    {

    }

    void Update()
    {
        if (isStand)
        {
            intervalTime += Time.deltaTime;
        }
        
        if (1.5f < intervalTime)
        {
            intervalTime = 0f;
            isStand = false;
        }
    }

    void OnTriggerEnter(Collider hit)
    {
        if (hit.tag == "Player" || hit.tag == "Zombie")
        {
            //ドアを施錠
            if (!isStand)
            {
                isStand = true;
                doorScript.MoveDoor();
            }
        }
    }
}
