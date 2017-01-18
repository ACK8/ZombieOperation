using UnityEngine;
using UnityEngine.Events;

public class PushButton : MonoBehaviour
{
    [SerializeField]
    UnityEvent pushEvent;

    private float count = 0f;
    private bool isTouched = false;

    void Start()
    {

    }

    void Update()
    {
        //カードをタッチしたらカウント開始
        if (isTouched)
        {
            count += Time.deltaTime;
        }

        //時間が来たら再びタッチできるように
        if (1.5f < count)
        {
            count = 0f;
            isTouched = false;
        }
    }

    private void OnTriggerEnter(Collider hit)
    {

        if (hit.tag == "Key")
        {
        }
    }
}
