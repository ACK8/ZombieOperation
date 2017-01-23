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
    private Transform outletPos;
    [SerializeField]
    private GameObject strengthMedicine;
    [SerializeField]
    private GameObject zombieMedicine;
    [SerializeField]
    private Door doorScr;
    [SerializeField]
    private MeshRenderer meshRend;
    [SerializeField]
    private Light lamp;

    private State state = State.Wait;
    private GameObject[] list = new GameObject[4];
    private bool isPut_A = false;
    private bool isPut_B = false;
    private bool isPut_C = false;
    private bool isPut_D = false;
    private bool isDoorOpen = false;

    private bool isCreated = false;

    //ボタンで呼ぶ
    public void DoorMove()
    {
        doorScr.MoveDoor();
    }

    void Update()
    {
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
        if (!doorScr.isOpen)
        {
            if (!isPut_A && !isPut_B && isPut_C && isPut_D)
            {
                isPut_C = false;
                isPut_D = false;
                isCreated = true;
                CreateMedicine(zombieMedicine);
            }
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
