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
        print(isTouched);
        if (isTouched)
        {
            count += Time.deltaTime;
        }

        if (1.5f < count)
        {
            count = 0f;
            isTouched = false;
        }
    }

    private void OnTriggerEnter(Collider hit)
    {

        if (hit.tag == "VRController_R" && !isTouched)
        {
            isTouched = true;
            pushEvent.Invoke();
        }
    }
}
