using UnityEngine;
using System.Collections;

public class InjectionCollision : MonoBehaviour
{
    [SerializeField]
    private Syringe syringe;

    void OnTriggerEnter(Collider hit)
    {
        if(hit.tag == "StrengthMedicine")
        {
            syringe.AddStrengthMedicine();
            Destroy(hit.gameObject);
        }
    }

    public MedicineType GetMedicineType()
    {
        return syringe.medicineType;
    }
}