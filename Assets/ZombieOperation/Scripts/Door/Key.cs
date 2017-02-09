using UnityEngine;
using System.Collections;


public class Key : MonoBehaviour
{
    [SerializeField]
    private TextMesh text;
    [SerializeField, TextArea]
    private string cardName;

    public int cardID = 0;

    private void Start()
    {
        text.text = cardName + "ID" + cardID;
    }
}

