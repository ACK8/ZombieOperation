using UnityEngine;
using System.Collections.Generic;

public class ChemistryStation : MonoBehaviour
{
    [SerializeField]
    private Transform outletPos;
    [SerializeField]
    private GameObject strengthMedicine;
    [SerializeField]
    private GameObject zombieMedicine;
    [SerializeField]
    private GameObject button;

    private GameObject[] list = new GameObject[3];
    private bool isPut_A = false;
    private bool isPut_B = false;
    private bool isPut_C = false;

    private bool isCreated = false;

    void Start()
    {
        button.GetComponent<VRTK.VRTK_Button>().events.OnPush.AddListener(Create);
    }
    
    //ボタンで呼ぶ
    public void Create()
    {
        print("Create!!!!!!!!!!!!!!!!!!!!!!");
        if (isPut_A && isPut_B)
        {
            CreateMedicine(zombieMedicine);
        }

        if (isPut_A && isPut_C)
        {
            CreateMedicine(strengthMedicine);
        }
    }

    void CreateMedicine(GameObject m)
    {
        Instantiate(strengthMedicine, outletPos.position, outletPos.rotation);

        foreach (GameObject g in list)
        {
            if (g != null)
            {
                Destroy(g);
            }
        }
    }

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
        }
    }

    void OnCollisionExit(Collision hit)
    {
        switch (hit.gameObject.tag)
        {
            case "Medicine_A":
                isPut_A = false;
                list[0] = null;

                break;
            case "Medicine_B":
                isPut_B = false;
                list[1] = null;

                break;
            case "Medicine_C":
                isPut_C = false;
                list[2] = null;

                break;
        }
    }
}
