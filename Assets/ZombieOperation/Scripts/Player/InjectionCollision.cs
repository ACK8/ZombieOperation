using UnityEngine;
using System.Collections;

//注射器の先のコライダー
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
                        //死体に蘇生薬を注射
                        if (zombie.zombieChangeTime <= zombie.injectionVolume)
                        {
                            syringe.DecreaseMedicine(MedicineType.Zombie);
                        }
                        break;

                    case MedicineType.Strength:
                        //ゾンビに強化薬を注射
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
                //蘇生薬を増やす
                syringe.AddMedicine(MedicineType.Zombie);
                Destroy(hit.gameObject);
                break;

            case "Medicine_B":
                //強化薬を増やす
                syringe.AddMedicine(MedicineType.Strength);
                Destroy(hit.gameObject);
                break;
        }
    }

    public MedicineType GetMedicineType()
    {
        return syringe.medicineType;
    }

    public int zombieMedicineNumber
    {
        get { return syringe.zombieMedicineNumber; }
    }

    public int strengthMedicineNumber
    {
        get { return syringe.strengthMedicineNumber; }
    }
}