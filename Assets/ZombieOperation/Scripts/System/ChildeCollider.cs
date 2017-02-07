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
        zombie.CollisionEnter(hit);
    }

    void OnTriggerEnter(Collider hit)
    {
        zombie.TriggerEnter(hit);
    }

    public GameObject GetGameObject()
    {
        return zombieObj;
    }

    public Zombie getZombie
    {
        get { return zombie; }
    }
}
