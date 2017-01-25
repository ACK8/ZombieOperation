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
        switch (hit.tag)
        {
            case "ZombieOperation":
                if (device.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
                {
                    handCtrl.handType = HandType.ZombieOperating;
                    Destroy(hit.gameObject);
                }

                break;
            case "ZombieOperation_Syringe":
                if (device.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
                {
                    Destroy(hit.gameObject);
                    TutorialManager.Instance.canMenuButtonInput = true;
                }

                break;
        }
    }
}