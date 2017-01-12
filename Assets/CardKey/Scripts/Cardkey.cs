using UnityEngine;
using System.Collections;

public class Cardkey : MonoBehaviour
{
    [SerializeField]
    private Door doorScript;
    [SerializeField]
    private int cardKeyID;

    private float a;
    private bool isTouched = false;

    void Start()
    {

    }

    void Update()
    {
        if (isTouched)
        {
            a += Time.deltaTime;
        }

        if (1.5f < a)
        {
            a = 0f;
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
                doorScript.MoveDoor();
            }
        }
    }
}
