using UnityEngine;
using System.Collections;

public class DestructionObject : MonoBehaviour
{
    [SerializeField]
    private GameObject destructionPos;
    [SerializeField]
    private GameObject particle;
    [SerializeField]
    private int enduranceValue;     //耐久値
    [SerializeField]
    private int damageValue;        //ダメージ値

    public void DecreaseEnduranceValue(Collider c)
    {
        //耐久地減少
        if (0 < enduranceValue)
        {
            enduranceValue -= damageValue;

            GameObject g = Instantiate(particle, c.transform.position, particle.transform.rotation) as GameObject;
            Destroy(g, 1.0f);
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
