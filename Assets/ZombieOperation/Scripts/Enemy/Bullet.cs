using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private float destroyTime = 4f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, destroyTime);
    }

    void Update()
    {
        rb.velocity = transform.forward.normalized * speed;
    }

    private void OnTriggerEnter(Collider hit)
    {
        if (hit.tag == "Wall" || hit.tag == "Floor" || hit.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}
