using UnityEngine;
using UnityEngine.UI;

public class InjectionVolumeUI : MonoBehaviour
{
    [SerializeField]
    private Slider slider;

    private GameObject vrCamEye;
    private GameObject leftController;

    void Start()
    {
        slider.gameObject.SetActive(false);
    }

    void Update()
    {
        FindObject();

        if (vrCamEye)
        {
            if (slider.gameObject.activeSelf)
            {
                transform.LookAt(vrCamEye.transform);
            }
        }
    }

    public void SwitchDisplay(bool f)
    {
        if (f)
        {
            Vector3 d = vrCamEye.transform.forward;
            d.y = 0.0f;
            d.Normalize();

            slider.gameObject.SetActive(true);
            
            transform.position = leftController.transform.position;
            transform.rotation = vrCamEye.transform.rotation;
            transform.LookAt(vrCamEye.transform);
        }
        else
        {
            slider.gameObject.SetActive(false);
        }
    }

    void FindObject()
    {
        if (vrCamEye == null)
            vrCamEye = GameObject.Find("Camera (eye)");

        if (leftController == null)
            leftController = GameObject.Find("Controller (left)");
    }

    public void SetVolume(float v)
    {
        slider.value = v;
    }

    public void SetValueRange(float min, float max)
    {
        slider.minValue = min;
        slider.maxValue = max;
    }
}
