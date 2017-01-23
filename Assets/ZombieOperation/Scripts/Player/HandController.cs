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
    public GameObject controllerModel;

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
                controllerModel.SetActive(true);

                break;

            case HandType.ZombieOperating:
                zombieOperatingObject.SetActive(true);
                zombieOperatingComponent.enabled = true;
                controllerModel.SetActive(false);

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

                if (!vrControllerObject.activeSelf)
                {
                    controllerModel.SetActive(true);
                    vrControllerObject.SetActive(true);
                    zombieOperatingObject.SetActive(false);
                }
                break;

            case HandType.ZombieOperating:

                if (!zombieOperatingObject.activeSelf)
                {
                    vrControllerObject.SetActive(false);
                    controllerModel.SetActive(false);
                    zombieOperatingObject.SetActive(true);
                }

                //トリガーの入力可否
                if (!TutorialManager.Instance.canTriggerInput) return;

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
            //メニューボタンの入力可否
            if (!TutorialManager.Instance.canMenuButtonInput) return;

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
