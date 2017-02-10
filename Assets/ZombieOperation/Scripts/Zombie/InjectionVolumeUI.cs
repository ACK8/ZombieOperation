using UnityEngine;
using UnityEngine.UI;

//薬の注入量のゲージ
public class InjectionVolumeUI : MonoBehaviour
{
    [SerializeField]
    private Slider slider;
    [SerializeField]
    private Image fillImage;

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
                //表示位置調整
                transform.LookAt(vrCamEye.transform);
            }
        }
    }

    //表示切り替え
    public void SwitchDisplay(bool f)
    {
        if (f)
        {
            //表示位置調整
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

    //カメラとコントローラーを検索
    void FindObject()
    {
        if (vrCamEye == null)
            vrCamEye = GameObject.Find("Camera (eye)");

        if (leftController == null)
            leftController = GameObject.Find("Controller (left)");
    }

    //注入量を設定
    public void SetVolume(float v, Color col)
    {
        slider.value = v;
        fillImage.color = col;
    }
    
    //スライダーの範囲設定
    public void SetValueRange(float min, float max)
    {
        slider.minValue = min;
        slider.maxValue = max;
    }
}
