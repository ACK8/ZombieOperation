using UnityEngine;
using System.Collections;

public class InjectionCollision : MonoBehaviour
{
    void OnTriggerStay(Collider other)
    {
        Debug.Log("Hit");

        //ゾンビなら注射
        if (other.transform.tag == "Zombie")
        {

        }
    }

    void Start ()
    {
	
	}
	
	void Update ()
    {
	
	}
}
