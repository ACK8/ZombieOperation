﻿using UnityEngine;
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
        MenuSetActive(false);
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

            MenuSetActive(true);

            transform.position = vrCamEye.transform.position + d * offset;
            transform.rotation = vrCamEye.transform.rotation;
            transform.LookAt(vrCamEye.transform);
        }
        else
        {
            MenuSetActive(false);
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

            case "BackToTitle":
                BackToTitle();
                SwitchDisplay();
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

    //メニュー項目の表示
    void MenuSetActive(bool f)
    {
        foreach (GameObject nemu in nemuPanel)
        {
            nemu.SetActive(f);
        }
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
