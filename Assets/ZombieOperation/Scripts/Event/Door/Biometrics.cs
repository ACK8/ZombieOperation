using UnityEngine;
using System.Collections;

public class Biometrics : MonoBehaviour
{
    public Door doorScript;
    private Zombie zombie;
    public int MachineID;
    private bool isTouched=false;
    private bool isMove=false;
	void Start ()
    {
	
	}	

	void Update ()
    {
        if(zombie)
        {
            if(zombie.isAuthenticationComplete&&!isMove)
            {
                isMove = true;
                doorScript.MoveDoor();
            }
        }
	}
    void OnTriggerEnter(Collider hit)
    {
        if (hit.tag=="Zombie")
        {
            if (MachineID == hit.GetComponent<Zombie>().zombieID && !isTouched)
            {
                isTouched = true;
                hit.GetComponent<Zombie>().StartBiometrics();
                zombie = hit.GetComponent<Zombie>();
                print("Opresadas");

            }
        }
    }
}
