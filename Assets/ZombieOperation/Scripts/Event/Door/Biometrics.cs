using UnityEngine;
using System.Collections;

public class Biometrics : MonoBehaviour
{
    public Door doorScript;
    private Zombie zombie;
    public int MachineID;
    private bool isTouched=false;
    private bool isMove=false;
    public Transform MovePoint;
	void Start ()
    {
	
	}	

	void Update ()
    {
	}
    void OnTriggerEnter(Collider hit)
    {
        if (hit.tag=="Zombie")
        {
            if (MachineID == hit.GetComponent<Zombie>().zombieID && !isTouched)
            {
                isTouched = true;
                doorScript.MoveDoor();
            }
        }
    }
}
