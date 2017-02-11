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
    private GameObject[] particle;  //薬吸引時のパーティクル
    [SerializeField]
    private CapsuleCollider injectionJudgment; //注射中を判定する用のコライダー
    [SerializeField]
    private MeshRenderer pipeMat;
    [SerializeField]
    private Color zombieColor;
    [SerializeField]
    private Color StrengthColor;
    [SerializeField]
    private TextMesh textMesh;
    [SerializeField]
    private int zombieMedicineNum = 1;
    [SerializeField]
    private int strengthMedicineNum = 0;

    private MedicineType medicine = MedicineType.Zombie;
    private Material mat;   //色で薬の種類識別

    void Awake()
    {
        injectionJudgment.enabled = false;
        mat = pipeMat.materials[1];
    }

    void Update()
    {
        //強化薬を持っていないときは強制的にゾンビ役を選択
        if (strengthMedicineNum <= 0)
        {
            medicine = MedicineType.Zombie;
            mat.color = zombieColor;
        }
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
            textMesh.text = "蘇生薬 " + zombieMedicineNum + "個";
        }
        else if (t == 1)
        {
            //強化薬を持っていると選択できる
            if (0 < strengthMedicineNum)
            {
                medicine = MedicineType.Strength;
                mat.color = StrengthColor;
                textMesh.text = "強化薬 " + zombieMedicineNum + "個";
            }
        }
    }

    //注射をしている状態にする
    public void OnInjection()
    {
        switch (medicine)
        {
            case MedicineType.Zombie:
                injectionJudgment.enabled = true;

                break;
            case MedicineType.Strength:
                injectionJudgment.enabled = true;

                break;
        }
    }

    //注射をしていない状態にする
    public void OffInjection()
    {
        injectionJudgment.enabled = false;
    }

    //薬の種類
    public MedicineType medicineType
    {
        get { return medicine; }
    }

    //ゾンビ薬の数
    public int zombieMedicineNumber
    {
        get { return zombieMedicineNum; }
    }

    //強化薬の数
    public int strengthMedicineNumber
    {
        get { return strengthMedicineNum; }
    }
}
