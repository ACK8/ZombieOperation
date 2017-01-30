using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class RadiusCenter : MonoBehaviour
{
    public Material radiusMaterial;
    public Color color = Color.white;
    public float radius = 10f;
    public float radiusWidth = 2f;
    public float power = 5f;
    public float speed = 60f;

    private Transform movePoint;

    private void Start()
    {
        movePoint = GameObject.Find("!MoveTarget").transform;
    }

    void Update()
    {
        radiusMaterial.SetVector("_Center", movePoint.position);
        radiusMaterial.SetColor("_RadiusColor", color);
        radiusMaterial.SetFloat("_RadiusPower", power);
        radiusMaterial.SetFloat("_RadiusSpeed", speed);
        radiusMaterial.SetFloat("_Radius", radius);

        if (movePoint.gameObject.activeSelf)
            radiusMaterial.SetFloat("_RadiusWidth", radiusWidth);
        else
            radiusMaterial.SetFloat("_RadiusWidth", 0);

    }
}