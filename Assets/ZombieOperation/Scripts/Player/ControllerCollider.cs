using UnityEngine;
using System.Collections;

public class ControllerCollider : MonoBehaviour
{
    private SteamVR_TrackedObject trackedComponent;
    private SteamVR_Controller.Device device;

    private CapsuleCollider col;

    void Start()
    {
        trackedComponent = gameObject.transform.parent.GetComponent<SteamVR_TrackedObject>();
    }

    void Update()
    {
        device = SteamVR_Controller.Input((int)trackedComponent.index);


        col.enabled = device.GetPressDown(SteamVR_Controller.ButtonMask.Grip);
    }
}
