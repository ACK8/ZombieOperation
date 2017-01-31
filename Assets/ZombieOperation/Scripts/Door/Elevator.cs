using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField]
    private ElevatorDoor doorScript;
    [SerializeField]
    private ElevatorCardkey cardKeyScript;

    private bool isCloseing = false;

    void Start()
    {

    }

    void Update()
    {
        if (!isCloseing && doorScript.isClose && cardKeyScript.isAuthenticated)
        {
            isCloseing = true;
        }
    }

    public bool isElevatorCloseing
    {
        get { return isCloseing; }
    }
}
