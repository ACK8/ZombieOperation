using UnityEngine;
using System.Collections;

//選択時にアウトラインを表示する
public class ChangeMaterialMesh : MonoBehaviour
{
    public Material[] SelectedMaterials;
    public Material[] NormalMaterials;
    public MeshRenderer meshRenderer;

    void Start()
    {

    }

    //アウトライン表示
    public void ChangeSelected()
    {
        meshRenderer.sharedMaterials = SelectedMaterials;
    }

    //アウトライン非表示
    public void ChangeNormal()
    {
        meshRenderer.sharedMaterials = NormalMaterials;
    }
}
