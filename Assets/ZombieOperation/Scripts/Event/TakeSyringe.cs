using UnityEngine;
using UnityEngine.Events;

public class TakeSyringe : MonoBehaviour
{
    [SerializeField]
    private HandController handCtrl;

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

    private void OnTriggerStay(Collider hit)
    {
        if (hit.tag == "ZombieOperation")
        {
            if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
            {
                handCtrl.handType = HandType.ZombieOperating;
                Destroy(hit.gameObject);
            }
        }
    }
}