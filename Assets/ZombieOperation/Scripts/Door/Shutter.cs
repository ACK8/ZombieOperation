using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shutter : MonoBehaviour
{
    [SerializeField]
    private Animator anim;

    void Start()
    {

    }
    
    public void Open()
    {
        anim.SetBool("LeverOpen", true);
    }
}
