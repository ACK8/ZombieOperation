using UnityEngine;
using System.Collections;

public enum VRInputTriggerType
{
    TouchDown,
    TouchUp,
    Touch,
    PressDown,
    Press
}

public enum VRInputTouchpadType
{
    TouchDown,
    TouchUp,
    Touch,
    PressDown,
    PressUp,
    Press
}

public class VRInputLeft : MonoBehaviour
{
    private bool isApplicationMenu = false;
    private bool isGrip = false;
    private bool[] isTrigger;
    private bool[] isTouchpad;
    private SteamVR_TrackedObject trackedComponent;
    
    void Awake()
    {
        isTrigger = new bool[5];
        isTouchpad = new bool[6];

        for (int i = 0; i < isTrigger.Length; i++) isTrigger[i] = false;
        for (int i = 0; i < isTouchpad.Length; i++) isTouchpad[i] = false;
    }

    void Start ()
    {
        trackedComponent = GetComponent<SteamVR_TrackedObject>();
    }

    void Update ()
    {
        var device = SteamVR_Controller.Input((int)trackedComponent.index);

        //トリガー関係
        {
            //トリガーを浅く引いた
            if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
            {
                isTrigger[(int)VRInputTriggerType.TouchDown] = true;
            }
            else
            {
                isTrigger[(int)VRInputTriggerType.TouchDown] = false;
            }

            //トリガーを深く引いた
            if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
            {
                isTrigger[(int)VRInputTriggerType.PressDown] = true;
            }
            else
            {
                isTrigger[(int)VRInputTriggerType.PressDown] = false;
            }

            //トリガーを離した
            if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
            {
                isTrigger[(int)VRInputTriggerType.TouchUp] = true;
            }
            else
            {
                isTrigger[(int)VRInputTriggerType.TouchUp] = false;
            }

            //トリガーを浅く引いている
            if (device.GetTouch(SteamVR_Controller.ButtonMask.Trigger))
            {
                isTrigger[(int)VRInputTriggerType.Touch] = true;
            }
            else
            {
                isTrigger[(int)VRInputTriggerType.Touch] = false;
            }

            //トリガーを深く引いている
            if (device.GetPress(SteamVR_Controller.ButtonMask.Trigger))
            {
                isTrigger[(int)VRInputTriggerType.Press] = true;
            }
            else
            {
                isTrigger[(int)VRInputTriggerType.Press] = false;
            }
        }

        //タッチパット
        {
            //タッチパッドをクリックした
            if (device.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
            {
                isTouchpad[(int)VRInputTouchpadType.PressDown] = true;
            }
            else
            {
                isTouchpad[(int)VRInputTouchpadType.PressDown] = false;
            }

            //タッチパッドをクリックしている
            if (device.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
            {
                isTouchpad[(int)VRInputTouchpadType.Press] = true;
            }
            else
            {
                isTouchpad[(int)VRInputTouchpadType.Press] = false;
            }

            //タッチパッドをクリックして離した
            if (device.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad))
            {
                isTouchpad[(int)VRInputTouchpadType.PressUp] = true;
            }
            else
            {
                isTouchpad[(int)VRInputTouchpadType.PressUp] = false;
            }

            //タッチパッドに触った
            if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Touchpad))
            {
                isTouchpad[(int)VRInputTouchpadType.TouchDown] = true;
            }
            else
            {
                isTouchpad[(int)VRInputTouchpadType.TouchDown] = false;
            }

            //タッチパッドを離した
            if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Touchpad))
            {
                isTouchpad[(int)VRInputTouchpadType.TouchUp] = true;
            }
            else
            {
                isTouchpad[(int)VRInputTouchpadType.TouchUp] = false;
            }

            //タッチパッドに触っている
            if (device.GetTouch(SteamVR_Controller.ButtonMask.Touchpad))
            {
                isTouchpad[(int)VRInputTouchpadType.Touch] = true;
            }
            else
            {
                isTouchpad[(int)VRInputTouchpadType.Touch] = false;
            }
        }

        //メニューボタンをクリックした
        if (device.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
        {
            isApplicationMenu = true;
        }
        else
        {
            isApplicationMenu = false;
        }

        //グリップボタンをクリックした
        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
        {
            isGrip = true;
        }
        else
        {
            isGrip = false;
        }
    }

    public bool GetTrigger(VRInputTriggerType triggerType)
    {
        return isTrigger[(int)triggerType];
    }

    public bool GetTouchpad(VRInputTouchpadType touchpadType)
    {
        return isTouchpad[(int)touchpadType];
    }

    public bool GetApplicationMenu()
    {
        return isApplicationMenu;
    }

    public bool GetGrip()
    {
        return isGrip;
    }
}
