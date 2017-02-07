using UnityEngine;
using UnityEngine.Events;

public class TakeSyringe : MonoBehaviour
{
    [SerializeField]
    private HandController handCtrl;

    private SteamVR_TrackedObject trackedComponent;
    private SteamVR_Controller.Device device;
    //private bool isTakeSyringe = false;

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
                    //注射器をとっていないと呼ばれない
                    //if (isTakeSyringe)
                    {
                        //注射器をとるとステージに設置している注射器、自スクリプト、コライダー削除を削除、
                        Destroy(this);
                        Destroy(hit.gameObject);
                        Destroy(GetComponent<SphereCollider>());
                        TutorialManager.Instance.canMenuButtonInput = true;
                    }
                }

                break;
            case "ZombieOperation_Syringe":
                if (device.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
                {
                    //注射器をとるとステージに設置している注射器、自スクリプト、コライダー削除を削除、
                   // isTakeSyringe = true;
                    Destroy(this);
                    Destroy(hit.gameObject);
                    Destroy(GetComponent<SphereCollider>());
                    handCtrl.handType = HandType.ZombieOperating;
                }

                break;
        }
    }
}