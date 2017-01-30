using UnityEngine;
using System.Collections;

public class TutorialManager : SingletonMonoBehaviour<TutorialManager>
{
    [Tooltip("タッチパッドの入力可否")]
    public bool canTouchPadInput = true;

    [Tooltip("トリガーの入力可否")]
    public bool canTriggerInput = true;

    [Tooltip("メニューボタンの入力可否")]
    public bool canMenuButtonInput = true;

    [Tooltip("グリップボタンの入力可否")]
    public bool canGripInput = true;   
}
