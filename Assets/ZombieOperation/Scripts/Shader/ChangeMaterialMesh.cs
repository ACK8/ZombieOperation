﻿using UnityEngine;
using System.Collections;

public class ChangeMaterialMesh : MonoBehaviour
{
    public Material[] changeMaterials1;
    public Material[] changeMaterials2;
    public MeshRenderer meshRenderer;

    void Start()
    {

    }

    public void Change1()
    {
        meshRenderer.sharedMaterials = changeMaterials1;
    }

    public void Change2()
    {
        meshRenderer.sharedMaterials = changeMaterials2;
    }
}
