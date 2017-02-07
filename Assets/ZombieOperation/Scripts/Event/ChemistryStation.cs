using UnityEngine;
using System.Collections.Generic;

public class ChemistryStation : MonoBehaviour
{
    enum State
    {
        Wait,
        Create,
    }

    [SerializeField]
    private Transform outletPos; //完成した薬を出す位置
    [SerializeField]
    private GameObject strengthMedicine; //強化薬のプレハブ
    [SerializeField]
    private GameObject zombieMedicine; //蘇生薬のプレハブ
    [SerializeField]
    private Door doorScr; //開閉するドアのスクリプト
    [SerializeField]
    private MeshRenderer meshRend; 
    [SerializeField]
    private Light lamp;
    
    private GameObject[] list = new GameObject[4]; //調合台に入っている薬
    private bool isPut_A = false;   //緑（蘇生薬）
    private bool isPut_B = false;   //赤（強化薬）
    private bool isPut_C = false;   //青
    private bool isPut_D = false;   //ピンク
    private bool isDoorOpen = false;

    private bool isCreated = false;

    //ボタンで呼ぶ
    public void DoorMove()
    {
        //ドア開閉
        doorScr.MoveDoor();
    }

    void Update()
    {
        //ランプの色変更
        if (!isPut_A && !isPut_B && isPut_C && isPut_D)
        {
            LampColor(Color.green);
        }
        else if (isPut_A && !isPut_B && isPut_C && isPut_D)
        {
            LampColor(Color.green);
        }
        else if (isCreated)
        {
            LampColor(Color.blue);
        }
        else if(!isCreated)
        {
            LampColor(Color.red);
        }
    }

    //ボタンで呼ぶ
    public void Create()
    {
        //ドアが閉まっているとき、中には行っている薬がレシピ通りなら調合
        if (!doorScr.isOpen)
        {
            //蘇生薬(青、ピンク)
            if (!isPut_A && !isPut_B && isPut_C && isPut_D)
            {
                isPut_C = false;
                isPut_D = false;
                isCreated = true;
                CreateMedicine(zombieMedicine);
            }
            //強化薬(緑、青、ピンク)
            else if (isPut_A && !isPut_B && isPut_C && isPut_D)
            {
                isPut_A = false;
                isPut_C = false;
                isPut_D = false;
                isCreated = true;
                CreateMedicine(strengthMedicine);
            }
        }
    }

    //ランプの色変更
    void LampColor(Color col)
    {
        lamp.color = col;
        meshRend.materials[1].color = col;
    }

    void CreateMedicine(GameObject m)
    {
        foreach (GameObject g in list)
        {
            if (g != null)
            {
                Destroy(g);
            }
        }

        Instantiate(m, outletPos.position, outletPos.rotation);
    }

    //調合台に入れた薬のフラグをTrue
    void OnCollisionEnter(Collision hit)
    {
        switch (hit.gameObject.tag)
        {
            case "Medicine_A":
                isPut_A = true;
                list[0] = hit.gameObject;

                break;
            case "Medicine_B":
                isPut_B = true;
                list[1] = hit.gameObject;

                break;
            case "Medicine_C":
                isPut_C = true;
                list[2] = hit.gameObject;

                break;
            case "Medicine_D":
                isPut_D = true;
                list[3] = hit.gameObject;

                break;
        }
    }

    //調合台から出した薬のフラグをFalse
    void OnCollisionExit(Collision hit)
    {
        switch (hit.gameObject.tag)
        {
            case "Medicine_A":
                isCreated = false;
                isPut_A = false;
                list[0] = null;

                break;
            case "Medicine_B":
                isCreated = false;
                isPut_B = false;
                list[1] = null;

                break;
            case "Medicine_C":
                isCreated = false;
                isPut_C = false;
                list[2] = null;

                break;
            case "Medicine_D":
                isCreated = false;
                isPut_D = false;
                list[3] = null;

                break;
        }
    }
}
