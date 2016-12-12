using UnityEngine;
using System.Collections;

public class Tutorial : MonoBehaviour
{
    enum TutorialType
    {
        None,
        Move,
        Zombied,
        OpenDoor,
        MightinessSwitching,
        ZombieSelection,
        MovementRangeSelection,
        MovementOrder,
        ObjectSelection,
        AttackOrder,
    }

    private TutorialType currntTutorialType = TutorialType.Move;

    void Start ()
    {

	}
	
	void Update ()
    {
        switch (currntTutorialType)
        {
            case TutorialType.Move:

                break;

            case TutorialType.Zombied:
                break;

            case TutorialType.MightinessSwitching:
                break;

            case TutorialType.ZombieSelection:
                break;

            case TutorialType.MovementRangeSelection:
                break;

            case TutorialType.MovementOrder:
                break;

            case TutorialType.ObjectSelection:
                break;

            case TutorialType.AttackOrder:
                break;
        }
	}
}
