using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventDead : MonoBehaviour 
{
	public float 時間;
	// Use this for initialization
	void Start () {
		Destroy (this.gameObject, 時間);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
