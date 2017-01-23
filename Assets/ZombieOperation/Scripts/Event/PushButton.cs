using UnityEngine;
using UnityEngine.Events;

public class PushButton : MonoBehaviour
{
    [SerializeField]
    private float pushDistance = 100f;
    [SerializeField]
    private UnityEvent pushEvent;

    private float count = 0f;
    private bool isTouched = false;
    private Vector3 scale = new Vector3(100f, 100f, 100f);

    void Start()
    {

    }

    void Update()
    {
        if (isTouched)
        {
            count += Time.deltaTime;
            scale.z = pushDistance;
        }

        if (1f < count)
        {
            count = 0f;
            scale.z = 100f;
            isTouched = false;
        }

        transform.localScale = Vector3.Lerp(transform.localScale, scale, 0.3f);
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
