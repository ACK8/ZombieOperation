using UnityEngine;
using System.Collections;

public class Bulkhead : MonoBehaviour
{
    public GameObject MovePoint;
	public Animator anime;

	void Start ()
    {
		
	}
	
	void Update ()
    {
		
    }

	void OnTriggerEnter(Collider hit)
	{
		if(hit.tag=="ZombieBody")
		{
			anime.SetBool ("Wallflag", true);
		}
	}
}
