using UnityEngine;
using VRTK;
using System.Collections;

public enum MoveType
{
    eTouchpadWalking,
    eTeleport,
}

public class ChangeMovingMethod : MonoBehaviour
{
    public MoveType moveType = MoveType.eTouchpadWalking;
    public GameObject cameraRigObject;
    public GameObject rightHandObject;
    public GameObject leftHandObject;

    void Start ()
    {
        switch (moveType)
        {
            case MoveType.eTouchpadWalking:
                cameraRigObject.GetComponent<VRTK_HeightAdjustTeleport>().enabled = false;
                rightHandObject.GetComponent<VRTK_BezierPointer>().enabled = false;
                leftHandObject.GetComponent<VRTK_BezierPointer>().enabled = false;

                //cameraRigObject.AddComponent<VRTK_TouchpadWalking>();

                //rightHandObject.AddComponent<VRTK_ControllerEvents>();
                //leftHandObject.AddComponent<VRTK_ControllerEvents>();

                break; 

            case MoveType.eTeleport:
                cameraRigObject.GetComponent<VRTK_TouchpadWalking>().enabled = false;

                rightHandObject.GetComponent<VRTK_InteractGrab>().enabled = false;
                rightHandObject.GetComponent<VRTK_InteractTouch>().enabled = false;
                rightHandObject.GetComponent<VRTK_ControllerActions>().enabled = false;

                leftHandObject.GetComponent<VRTK_InteractGrab>().enabled = false;
                leftHandObject.GetComponent<VRTK_InteractTouch>().enabled = false;
                leftHandObject.GetComponent<VRTK_ControllerActions>().enabled = false;

                break;
        }
    }
	
	void Update ()
    {

	}
}
