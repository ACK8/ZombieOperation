using UnityEngine;
using UnityEngine.Events;

public class TakeSyringe : MonoBehaviour
{
    [SerializeField]
    private GameObject zombieOperating;
    [SerializeField]
    private GameObject ControlModel;

    private SteamVR_TrackedObject trackedComponent;
    private SteamVR_Controller.Device device;

    void Start()
    {
        zombieOperating.SetActive(false);
        ControlModel.SetActive(true);
        trackedComponent = gameObject.transform.parent.GetComponent<SteamVR_TrackedObject>();
    }

    void Update()
    {
        device = SteamVR_Controller.Input((int)trackedComponent.index);
    }

    private void OnTriggerEnter(Collider hit)
    {
        if (hit.tag == "Syringe")
        {
            if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
            {
                zombieOperating.SetActive(true);
                ControlModel.SetActive(false);

                Destroy(hit.gameObject);
                Destroy(GetComponent<CapsuleCollider>());
                Destroy(this);
            }
        }
    }
}