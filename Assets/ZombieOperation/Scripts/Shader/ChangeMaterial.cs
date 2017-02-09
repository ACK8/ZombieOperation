using UnityEngine;
using System.Collections;

public class ChangeMaterial : MonoBehaviour
{
    public Material[] SelectedMaterials;
    public Material[] NormalMaterials;
    public SkinnedMeshRenderer meshRenderer;

    void Start ()
    {

    }
	
    public void ChangeSelected()
    {
        meshRenderer.sharedMaterials = SelectedMaterials;
    }

    public void ChangeNormal()
    {
        meshRenderer.sharedMaterials = NormalMaterials;
    }
}
