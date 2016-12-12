using UnityEngine;
using System.Collections;

public enum TutorialDrawType
{
    Move,
    Grab,
};

public class TutorialManager : SingletonMonoBehaviour<TutorialManager>
{

    public GameObject[] Tutorial;

    void Start()
    {
    }

    void Update()
    {
    }

    void SetDescription(TutorialDrawType name)
    {
        for (int i = 0; i < Tutorial.Length; i++)
        {
            Tutorial[i].SetActive(false);
        }
        Tutorial[(int)name].SetActive(true);
    }

}
