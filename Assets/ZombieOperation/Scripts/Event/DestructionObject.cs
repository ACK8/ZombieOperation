using UnityEngine;
using System.Collections;

public class DestructionObject : MonoBehaviour
{
    [SerializeField]
    private GameObject destructionPos;
    [SerializeField]
    private int enduranceValue;     //耐久値
    [SerializeField]
    private int damageValue;        //ダメージ値

    public void DecreaseEnduranceValue()
    {
        //耐久地減少
        if (0 < enduranceValue)
        {
            enduranceValue -= damageValue;
        }

        //破壊
        if (enduranceValue <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public GameObject destructionPosition
    {
        get { return destructionPos; }
    }
}
