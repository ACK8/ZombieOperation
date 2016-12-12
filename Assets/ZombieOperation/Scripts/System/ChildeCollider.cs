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
        zombie.HitCollider(hit);
    }

    public GameObject GetGameObject()
    {
        return zombieObj;
    }
}
