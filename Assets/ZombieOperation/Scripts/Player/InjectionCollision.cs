using UnityEngine;
using System.Collections;

public class InjectionCollision : MonoBehaviour
{
    [SerializeField]
    private Syringe syringe;

    private Zombie zombie;

    void OnTriggerEnter(Collider hit)
    {
        switch (hit.tag)
        {
            case "Zombie":
                zombie = hit.GetComponent<ChildeCollider>().getZombie;
                switch (syringe.medicineType)
                {
                    case MedicineType.Zombie:
                        if (zombie.zombieChangeTime <= zombie.injectionVolume)
                        {
                            syringe.DecreaseMedicine(MedicineType.Zombie);
                        }
                        break;

                    case MedicineType.Strength:
                        if (zombie.zombieChangeTime <= zombie.strengthVolume)
                        {
                            syringe.DecreaseMedicine(MedicineType.Strength);
                        }
                        break;
                }

                break;

            case "EventZombie":
                hit.GetComponent<EventZombie>().isZombie = true;

                break;
                
            case "Medicine_A":
                syringe.AddMedicine(MedicineType.Zombie);
                Destroy(hit.gameObject);
                break;

            case "Medicine_B":
                syringe.AddMedicine(MedicineType.Strength);
                Destroy(hit.gameObject);
                break;
        }
    }

    public MedicineType GetMedicineType()
    {
        return syringe.medicineType;
    }
}