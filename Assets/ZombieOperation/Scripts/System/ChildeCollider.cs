using UnityEngine;
using System.Collections;

public class ChildeCollider : MonoBehaviour
{
    [SerializeField]
    private Zombie zombie;
    [SerializeField]
    private GameObject zombieObj;
    
    void OnCollisionEnter(Collision hit)
    {
        print("OnCollisionEnter");
        zombie.CollisionEnter(hit);
    }

    void OnTriggerEnter(Collider hit)
    {
        print("OnTriggerEnter");
        zombie.TriggerEnter(hit);
    }

    public GameObject GetGameObject()
    {
        return zombieObj;
    }
}
