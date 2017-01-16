using UnityEngine;
using System.Collections;

public class Bulkhead : MonoBehaviour
{
    public Transform pos;
	void Start ()
    {
	
	}
	
	void Update ()
    {
        print(transform.position);
    }
    public void OpenBulkhead(float speed)
    {
        transform.position=new Vector3(0, transform.position.y+speed, 0);
    }
}
