using UnityEngine;
using UnityEngine.Events;

public class TakeSyringe : MonoBehaviour
{
    [SerializeField]
    private GameObject syringe;
    [SerializeField]
    private UnityEvent OnTouch;

    private SteamVR_TrackedObject trackedComponent;
    private SteamVR_Controller.Device device;

    void Start()
    {
        trackedComponent = gameObject.transform.parent.GetComponent<SteamVR_TrackedObject>();
    }

    void Update()
    {
        device = SteamVR_Controller.Input((int)trackedComponent.index);
    }

    void OnTriggerStay(Collider hit)
    {
        /*
        if (hit.tag == "Syringe")
        {
            if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
            {
                OnTouch.Invoke();
                Destroy(hit.gameObject);
                Destroy(GetComponent<CapsuleCollider>());
                Destroy(this);
            }
        }
        */
    }
}