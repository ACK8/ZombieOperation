using UnityEngine;
using System.Collections;

public class BulletFire : MonoBehaviour
{
    public GameObject firePoint;
    	
	void Update ()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            float x = Random.Range(0.05f, -0.05f);
            float y = Random.Range(0.05f, -0.05f);
            float z = Random.Range(0.05f, -0.05f);

            Vector3 dir = new Quaternion(x, y, z, 1.0f) * firePoint.transform.forward;

            Vector3 start = firePoint.transform.position;
            Vector3 end = start + dir * 5;

            Debug.DrawLine(start, end);
        }
	}
}
