using UnityEngine;
using System.Collections;

public class TutorialManager : SingletonMonoBehaviour<TutorialManager>
{
    [HideInInspector]
    public bool canTouchPadInput = true;
    [HideInInspector]
    public bool canTriggerInput = true;
    [HideInInspector]
    public bool canMenuButtonInput = true;
    [HideInInspector]
    public bool canGripInput = true;

    void Start()
    {

    }

    void Update()
    {

    }
}
