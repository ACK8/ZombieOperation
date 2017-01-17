using UnityEngine;
using System.Collections;

public class ChemistryStation : MonoBehaviour
{
    [SerializeField]
    private Transform outletPos;
    [SerializeField]
    private GameObject strengthMedicine;

    void Start()
    {

    }

    void Update()
    {

    }

    void OnCollisionEnter(Collision hit)
    {
        if (hit.gameObject.tag == "Medicine_A" && hit.gameObject.tag == "Medicine_B")
        {
            Instantiate(strengthMedicine, outletPos.position, outletPos.rotation);
        }
    }
}
