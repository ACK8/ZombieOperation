using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class test : MonoBehaviour
{
    Dictionary<int, float> temp = new Dictionary<int, float>();

    void Start ()
    {
        temp.Add(1, 0.0f);
    }
	
	void Update ()
    {
        foreach (var i in temp)
        {
            Debug.Log("aaa");
        }
    }
}
