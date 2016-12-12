﻿using UnityEngine;
using System.Collections;

public class ElevatorCardkey : MonoBehaviour
{
    [SerializeField]
    private ElevatorDoor[] doorScript = new ElevatorDoor[2];
    [SerializeField]
    private int cardKeyID;

    private float timeCount;
    private bool isTouched = false;
    private bool _isAuthenticated = false;

    void Start()
    {

    }

    void Update()
    {
        if (isTouched)
        {
            timeCount += Time.deltaTime;
        }

        if (1.5f < timeCount)
        {
            timeCount = 0f;
            isTouched = false;
        }
    }

    void OnTriggerEnter(Collider hit)
    {
        if (hit.tag == "Key")
        {
            //カードキーIDとカードIDが同じ、カードが触れていない時にドアを施錠
            if (cardKeyID == hit.GetComponent<Key>().cardID && !isTouched)
            {
                isTouched = true;

                for (int i = 0; i < doorScript.Length; i++)
                {
                    doorScript[i].MoveDoor();
                    _isAuthenticated = true;
                }
            }
        }
    }

    public bool isAuthenticated
    {
        get { return _isAuthenticated; }
    }
}

