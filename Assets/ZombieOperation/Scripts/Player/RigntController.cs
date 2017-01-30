using UnityEngine;

public class RigntController : MonoBehaviour
{
    private SteamVR_TrackedObject trackedComponent;
    private SteamVR_Controller.Device device;
    private LineRenderer line;
    private RaycastHit hit;
    private Ray ray;

    void Start()
    {
        trackedComponent = gameObject.transform.parent.GetComponent<SteamVR_TrackedObject>();
        line = GetComponent<LineRenderer>();
    }

    void Update()
    {
        device = SteamVR_Controller.Input((int)trackedComponent.index);

        ray.direction = transform.forward;
        ray.origin = transform.position;

        line.SetPosition(0, ray.origin);
        line.SetPosition(1, ray.GetPoint(200));


        if (device.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
        {
            Menu.Instance.SwitchDisplay();
        }

        if (Physics.Raycast(ray, out hit, 200.0f))
        {
            if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
            {
                Menu.Instance.SelectMenu(hit.collider.name);
            }
        }

        line.enabled = Menu.Instance.isDisplayed;
    }
}
