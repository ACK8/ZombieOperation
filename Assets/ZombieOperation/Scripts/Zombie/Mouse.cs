using UnityEngine;
using System.Collections;

public class Mouse : Zombie
{
    void Start()
    {
        navMesh = GetComponent<UnityEngine.AI.NavMeshAgent>();
        anim = GetComponent<Animator>();
        navSpeed = navMesh.speed;
    }

    void Update()
    {

    }

    void OnTriggerEnter(Collider hit)
    {
        //障害物破壊
        if (hit.tag == "Object")
        {
            hit.gameObject.GetComponent<DestructionObject>().DecreaseEnduranceValue();
        }
    }
}
