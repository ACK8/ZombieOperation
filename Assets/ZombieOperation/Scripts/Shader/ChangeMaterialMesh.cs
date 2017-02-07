using UnityEngine;
using System.Collections;

//選択時にアウトラインを表示する
public class ChangeMaterialMesh : MonoBehaviour
{
    public Material[] changeMaterials1;
    public Material[] changeMaterials2;
    public MeshRenderer meshRenderer;

    void Start()
    {

    }

    //アウトライン表示
    public void ChangeSelected()
    {
        meshRenderer.sharedMaterials = changeMaterials1;
    }

    //アウトライン非表示
    public void ChangeNormal()
    {
        meshRenderer.sharedMaterials = changeMaterials2;
    }
}
