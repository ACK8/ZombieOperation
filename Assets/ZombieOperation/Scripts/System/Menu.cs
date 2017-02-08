using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Menu : SingletonMonoBehaviour<Menu>
{
    [SerializeField]
    private GameObject[] nemuPanel;
    [SerializeField]
    private float offset = 2.0f;
    [SerializeField]
    private string titleSceneName = null;

    private GameObject vrCamEye;
    private bool _isDisplayed = false;

    void Start()
    {
        vrCamEye = GameObject.Find("Camera (eye)");
        MenuSetActiveAll(false);
    }

    void Update()
    {
        if (!vrCamEye)
        {
            Debug.LogError("Camera (eye)が見つかりません");
        }
    }

    //メニュー表示の切り替え
    public void SwitchDisplay()
    {
        _isDisplayed = !_isDisplayed;

        //表示位置の調整
        if (_isDisplayed)
        {
            Vector3 d = vrCamEye.transform.forward;
            d.y = 0.0f;
            d.Normalize();

            MenuSetActive(true, 0);

            transform.position = vrCamEye.transform.position + d * offset;
            transform.rotation = vrCamEye.transform.rotation;
            transform.LookAt(vrCamEye.transform);
        }
        else
        {
            MenuSetActiveAll(false);
        }
    }

    //項目の選択
    public void SelectMenu(string name)
    {
        switch (name)
        {
            case "Restart":
                Restart();
                SwitchDisplay();
                break;

            case "Description":
                MenuSetActive(false, 0);
                MenuSetActive(true, 1);
                break;

            case "BackToTitle":
                BackToTitle();
                SwitchDisplay();
                break;

            case "":
                break;

            default:
                break;
        }
    }

    //メニューを表示しているか
    public bool isDisplayed
    {
        get { return _isDisplayed; }
    }

    //全メニュー項目の表示
    void MenuSetActiveAll(bool f)
    {
        foreach (GameObject nemu in nemuPanel)
        {
            nemu.SetActive(f);
        }
    }

    //メニュー項目の表示
    void MenuSetActive(bool f, int type)
    {
        nemuPanel[type].SetActive(f);
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void BackToTitle()
    {
        SceneManager.LoadScene(titleSceneName);
    }
}
