using UnityEngine;
using System.Collections;

//手に装備する種類
public enum HandType
{
    VRController,
    ZombieOperating,
}

//手関係の処理をコントロールするクラス
public class HandController : MonoBehaviour
{
    public HandType handType;
    public GameObject vrControllerObject;
    public GameObject zombieOperatingObject;

    private SteamVR_TrackedObject trackedComponent;
    private ZombieOperating zombieOperatingComponent;

    void Start()
    {
        trackedComponent = GetComponent<SteamVR_TrackedObject>();

        vrControllerObject.SetActive(false);

        zombieOperatingObject.SetActive(false);
        zombieOperatingComponent = zombieOperatingObject.GetComponent<ZombieOperating>();
        zombieOperatingComponent.enabled = false;

        switch (handType)
        {
            case HandType.VRController:
                vrControllerObject.SetActive(true);

                break;

            case HandType.ZombieOperating:
                zombieOperatingObject.SetActive(true);
                zombieOperatingComponent.enabled = true;

                break;
        }
        //SyringeEnabled(false);
    }

    void Update()
    {
        var device = SteamVR_Controller.Input((int)trackedComponent.index);

        //手に持ってるごとの処理
        switch (handType)
        {
            case HandType.VRController:
                break;

            case HandType.ZombieOperating:
                switch (zombieOperatingComponent.operatingType)
                {
                    case ZombieOperatingType.Operating:
                        //命令を決定
                        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
                        {
                            zombieOperatingComponent.OperatingEvent();
                        }
                        break;

                    case ZombieOperatingType.Syringe:
                        //注射を打つ
                        if (device.GetPress(SteamVR_Controller.ButtonMask.Trigger))
                        {
                            zombieOperatingComponent.SyringeEvent(true);
                        }
                        else
                        {
                            zombieOperatingComponent.SyringeEvent(false);
                        }
                        break;
                }
                break;
        }

        //注射器と操作器の入れ替え
        if (device.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
        {
            switch (zombieOperatingComponent.operatingType)
            {
                case ZombieOperatingType.Operating:
                    zombieOperatingComponent.ChangeMightiness(ZombieOperatingType.Syringe);
                    break;

                case ZombieOperatingType.Syringe:
                    zombieOperatingComponent.ChangeMightiness(ZombieOperatingType.Operating);
                    break;
            }
        }
    }
}
