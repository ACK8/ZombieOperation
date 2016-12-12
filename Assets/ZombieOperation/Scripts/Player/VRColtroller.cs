using UnityEngine;
using System.Collections;

public class VRColtroller : MonoBehaviour
{
    private SteamVR_TrackedObject trackedComponent;
    private LineRenderer line;
    private RaycastHit hit;
    private Ray ray;
    private bool isMenuDisplayed = false;

    void Start()
    {
        line = GetComponent<LineRenderer>();
        trackedComponent = gameObject.transform.parent.GetComponent<SteamVR_TrackedObject>();
    }

    void Update()
    {
        var device = SteamVR_Controller.Input((int)trackedComponent.index);
        /*
        //メニューの表示
        if (device.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
        {
            isMenuDisplayed = !isMenuDisplayed;
            Menu.Instance.SwitchDisplay();
        }

        //決定
        if (Physics.Raycast(ray, out hit, 100))
        {
            if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
            {
                Menu.Instance.SelectMenu(hit.collider.name);
            }
        }

        //メニュー表示中とレーザー表示をリンク       
        line.enabled = Menu.Instance.isDisplayed;

        ray.direction = transform.forward;
        ray.origin = transform.position;

        line.SetPosition(0, ray.origin);

        line.SetPosition(1, ray.GetPoint(100));
    */
    }
}
