using UnityEngine;
using System.Collections;

public class Biometrics : MonoBehaviour
{
    public GameObject MovePoint;
    public Door doorScript;
    public int MachineID;

    private Zombie zombie;
    private bool isTouched = false;
    private bool isMove = false;

    void Start()
    {

    }

    void Update()
    {

    }

    void OnTriggerEnter(Collider hit)
    {
        if (hit.tag == "ZombieBody")
        {
            if (MachineID == hit.GetComponent<Zombie>().zombieID && !isTouched)
            {
                isTouched = true;
                doorScript.MoveDoor();
            }
        }
    }
}
