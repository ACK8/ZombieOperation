using UnityEngine;
using System.Collections;

public class ChangeMaterial : MonoBehaviour
{
    public Material[] changeMaterials1;
    public Material[] changeMaterials2;
    public SkinnedMeshRenderer meshRenderer;

    void Start ()
    {

    }
	
    public void ChangeSelected()
    {
        meshRenderer.sharedMaterials = changeMaterials1;
    }

    public void ChangeNormal()
    {
        meshRenderer.sharedMaterials = changeMaterials2;
    }
}
