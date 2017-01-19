namespace VRTK.Examples
{
    using UnityEngine;

    public class ControlReactor : MonoBehaviour
    {
        public TextMesh go;

        private void Start()
        {
            GetComponent<VRTK_Control>().defaultEvents.OnValueChanged.AddListener(HandleChange);
            HandleChange(GetComponent<VRTK_Control>().GetValue(), GetComponent<VRTK_Control>().GetNormalizedValue());

            GetComponent<VRTK_Control>().defaultEvents.OnValueChanged.AddListener(HandleChange);
            HandleChange(GetComponent<VRTK_Control>().GetValue(), GetComponent<VRTK_Control>().GetNormalizedValue());
        }
        

        private void HandleChange(float value, float normalizedValue)
        {
            bool b = (normalizedValue != 0);
        }
    }
}