using UnityEngine;
using System.Collections.Generic;

public class ChemistryStation : MonoBehaviour
{
    enum State
    {
        Wait,
        Open,
        Close,
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

    private State state = State.Wait;
    private GameObject[] list = new GameObject[3];
    private bool isPut_A = false;
    private bool isPut_B = false;
    private bool isPut_C = false;
    private bool isDoorOpen = false;

    private bool isCreated = false;
    
    //ボタンで呼ぶ
    public void DoorMove()
    {
        doorScr.MoveDoor();
    }
    
    //ボタンで呼ぶ
    public void Create()
    {
        if (!doorScr.isOpen)
        {
            if (isPut_A && isPut_B)
            {
                isPut_A = false;
                isPut_B = false;
                CreateMedicine(zombieMedicine);
            }

            if (isPut_A && isPut_C)
            {
                isPut_A = false;
                isPut_C = false;
                CreateMedicine(strengthMedicine);
            }
        }
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
        
        Instantiate(strengthMedicine, outletPos.position, outletPos.rotation);
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
