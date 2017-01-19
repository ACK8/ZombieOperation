using UnityEngine;
using System.Collections;

public class CardCaseKey : MonoBehaviour 
{
	[SerializeField]
	private CardCase CardCaseScript;
	[SerializeField]
	private int cardKeyID;
	private bool isTouched=false;

	void Start ()
	{
	
	}

	void Update ()
	{
	
	}

	void OnTriggerEnter(Collider hit)
	{
		if (hit.tag == "Key")
		{
			if (cardKeyID == hit.GetComponent<Key> ().cardID&&!isTouched) 
			{
				isTouched = true;
				CardCaseScript.MoveShutter();
			}
		}
	}

}
