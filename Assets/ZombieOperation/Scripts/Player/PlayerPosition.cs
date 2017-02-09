using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPosition : MonoBehaviour
{

    [SerializeField]
    private Transform head;

    void Start()
    {

    }
    
    void LateUpdate()
    {
        transform.position = head.position;
    }
}
