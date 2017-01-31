using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLeaver : MonoBehaviour
{
    [SerializeField]
    private Door door;

    private bool isOpen = false;

	void Start ()
    {
        GetComponent<VRTK.VRTK_Control>().defaultEvents.OnValueChanged.AddListener(HandleChange);
        HandleChange(GetComponent<VRTK.VRTK_Control>().GetValue(), GetComponent<VRTK.VRTK_Control>().GetNormalizedValue());
    }

    private void HandleChange(float value, float normalizedValue)
    {
        bool b = (normalizedValue != 0);

        if(b && !isOpen)
        {
            isOpen = true;
            door.MoveDoor();
        }
    }
}
