using UnityEngine;
using System.Collections;

public enum MedicineType
{
    Zombie = 0,
    Strength
}

//注射器関係の処理をする
public class Syringe : MonoBehaviour
{
    [SerializeField]
    private CapsuleCollider injectionJudgment; //注射中を判定する用のコライダー
    [SerializeField]
    private MeshRenderer pipeMat;
    [SerializeField]
    private Color zombieColor;
    [SerializeField]
    private Color StrengthColor;

    private MedicineType medicine = MedicineType.Zombie;
    private Material mat;   //色で薬の種類識別
    private int zombieMedicineNum = 1;
    private int strengthMedicineNum = 0;

    void Awake()
    {
        injectionJudgment.enabled = false;
        mat = pipeMat.materials[1];
    }

    void Update()
    {

    }

    //薬追加
    public void AddMedicine(MedicineType type)
    {
        switch (type)
        {
            case MedicineType.Zombie:
                zombieMedicineNum++;

                break;
            case MedicineType.Strength:

                strengthMedicineNum++;

                break;
        }
    }

    //薬減らす
    public void DecreaseMedicine(MedicineType type)
    {
        switch (type)
        {
            case MedicineType.Zombie:
                if (0 < zombieMedicineNum)
                {
                    zombieMedicineNum--;
                }

                break;
            case MedicineType.Strength:

                if (0 < strengthMedicineNum)
                {
                    strengthMedicineNum--;
                }
                break;
        }
    }

    //薬の種類変更
    public void ChangeMedicineType(int t)
    {
        if (t == 0)
        {
            medicine = MedicineType.Zombie;
            mat.color = zombieColor;
        }
        else if (t == 1)
        {
            medicine = MedicineType.Strength;
            mat.color = StrengthColor;
        }
    }

    //注射をしている状態にする
    public void OnInjection()
    {
        switch (medicine)
        {
            case MedicineType.Zombie:
                if (0 < zombieMedicineNum)
                {
                    injectionJudgment.enabled = true;
                }

                break;
            case MedicineType.Strength:
                if (0 < strengthMedicineNum)
                {
                    injectionJudgment.enabled = true;
                }

                break;
        }
    }

    //注射をしていない状態にする
    public void OffInjection()
    {
        injectionJudgment.enabled = false;
    }

    public MedicineType medicineType
    {
        get { return medicine; }
    }

    public int zombieMedicineNumber
    {
        get { return zombieMedicineNum; }
    }

    public int strengthMedicineNumber
    {
        get { return strengthMedicineNum; }
    }
}
