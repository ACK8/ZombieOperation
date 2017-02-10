using System.Collections;
using UnityEngine.Events;
using UnityEngine;

public class Lever : MonoBehaviour
{
    [SerializeField]
    private UnityEvent leverEvent;

    void Start()
    {
        GetComponent<VRTK.VRTK_Control>().defaultEvents.OnValueChanged.AddListener(HandleChange);
        HandleChange(GetComponent<VRTK.VRTK_Control>().GetValue(), GetComponent<VRTK.VRTK_Control>().GetNormalizedValue());
    }

    private void HandleChange(float value, float normalizedValue)
    {
        if (normalizedValue != 0)
        {
            leverEvent.Invoke();
        }
    }
}
