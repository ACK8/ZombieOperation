using UnityEngine;
using System.Collections;

public class CardCase : MonoBehaviour
{
	[SerializeField]
	private float MoveSpeed;

	private bool isOpen=false;
	private bool isflag = false;
	public Vector3 Tagetpos;

	void Start ()
	{
		
	}
		
	public void MoveShutter()
	{
		isflag = true;
	}

	void Update ()
	{
		if(isflag)
		{
			if (!isOpen)
			{
				OpenShutter ();
			}	
		}
	}

	public void OpenShutter()
	{
		transform.position= new Vector3 (transform.position.x,transform.position.y-MoveSpeed*Time.deltaTime,transform.position.z);
		if (Tagetpos.y >= transform.position.y) 
		{
			transform.position =new Vector3(transform.position.x,Tagetpos.y,transform.position.z);
			isOpen = true;
			isflag = false;
		}
	}
}
