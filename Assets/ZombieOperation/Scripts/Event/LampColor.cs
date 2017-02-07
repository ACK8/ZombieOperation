using UnityEngine;

public class LampColor : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer meshRend;
    [SerializeField]
    private Light lamp;

    //ランプの色変更
    public void ColorChange(Color col)
    {
        lamp.color = col;
        meshRend.materials[1].color = col;
    }
}
