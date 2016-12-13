using UnityEngine;
using System.Collections;

public class DestructionObject : MonoBehaviour
{
    [SerializeField]
    private GameObject destructionPos;
    [SerializeField]
    private int enduranceValue;
    [SerializeField]
    private int decrease;

    public void DecreaseEnduranceValue()
    {
        //耐久地減少
        if (0 < enduranceValue)
        {
            enduranceValue -= decrease;
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
