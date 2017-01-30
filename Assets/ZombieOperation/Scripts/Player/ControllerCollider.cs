using UnityEngine;
using System.Collections;

public class ControllerCollider : MonoBehaviour
{
    private SteamVR_TrackedObject trackedComponent;
    private SteamVR_Controller.Device device;

    private SphereCollider col;

    void Start()
    {
        trackedComponent = gameObject.transform.parent.GetComponent<SteamVR_TrackedObject>();
        col = GetComponent<SphereCollider>();
        col.enabled = false;
    }

    void Update()
    {
        device = SteamVR_Controller.Input((int)trackedComponent.index);

        if (col == null)
        {
            trackedComponent = gameObject.transform.parent.GetComponent<SteamVR_TrackedObject>();
        }
        else
        {
            col.enabled = device.GetPressDown(SteamVR_Controller.ButtonMask.Grip);
        }
    }
}
