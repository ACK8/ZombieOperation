using UnityEngine;
using System.Collections;

public class MoveTutorial : MonoBehaviour
{
    public VRInputLeft input;
    public GameObject nextTutorial;

    void Start ()
    {
    }
	
	void Update ()
    {
        if(input.GetTouchpad(VRInputTouchpadType.PressUp))
        {
            Instantiate(nextTutorial);
            Destroy(this.gameObject);
        }
    }
}
