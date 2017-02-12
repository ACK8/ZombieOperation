using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavAI : MonoBehaviour 
{
	public Transform ターゲット;
	public float スピード = 0.0f;
	public float 最大スピード = 0.5f;
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (ターゲット != null) 
		{
			スピード += 1.0f * Time.deltaTime;
			if (スピード> 最大スピード) 
			{
				スピード = 最大スピード;
			}
			this.GetComponent<NavMeshAgent> ().speed = スピード;
			this.GetComponent<NavMeshAgent> ().SetDestination (ターゲット.position);
		}
		else 
		{
			スピード -= 1.0f * Time.deltaTime;
			if (スピード< 最大スピード) 
			{
				スピード = 0.0f;
			}
			this.GetComponent<NavMeshAgent> ().speed = スピード;
		}
	}
}
