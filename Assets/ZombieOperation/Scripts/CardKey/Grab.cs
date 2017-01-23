using UnityEngine;
using System.Collections;

public class Grab : MonoBehaviour
{
    public GameObject key;

    private SteamVR_TrackedObject trackedObject;

    void Start()
    {
        trackedObject = GetComponent<SteamVR_TrackedObject>();

    }

    void Update()
    {
        var device = SteamVR_Controller.Input((int)trackedObject.index);

        if (device.GetPress(SteamVR_Controller.ButtonMask.Grip))
        {
            key.transform.position = this.transform.position;
            key.transform.rotation = this.transform.rotation;
        }
    }
}
