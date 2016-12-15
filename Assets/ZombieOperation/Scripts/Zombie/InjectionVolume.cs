using UnityEngine;
using UnityEngine.UI;

public class InjectionVolume : MonoBehaviour
{
    [SerializeField]
    private Slider slider;

    private GameObject vrCamEye;
	private GameObject leftController;
    private bool _isDisplayed = false;

    void Start()
    {
		vrCamEye = GameObject.Find("Camera (eye)");
		leftController = GameObject.Find("Controller (left)");
        slider.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!vrCamEye)
            Debug.LogError("Camera (eye)が見つかりません");

		if(!leftController)
			Debug.LogError("Controller (left)が見つかりません");
			

        if (slider.gameObject.activeSelf)
        {
            transform.LookAt(vrCamEye.transform);
        }
    }

    public void SwitchDisplay(bool f)
    {
        _isDisplayed = f;
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
