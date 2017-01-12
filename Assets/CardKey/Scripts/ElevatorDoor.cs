using UnityEngine;
using System.Collections;

public class ElevatorDoor : MonoBehaviour
{
    [SerializeField]
    private float movingSpeed = 0f;
    [SerializeField]
    private bool isRotate90D = false;   //ドアが90度回転しているか
    [SerializeField]
    private bool isRightDoor = false;

    private Vector3 initalPos;
    private float moveingDistance = 1;  //開く距離
    private bool isOpen = false;        //現在ドアが開いているか
    private bool isCloseing = true;    //ドアが閉じているときtrue

    void Start()
    {
        initalPos = this.transform.position;
    }

    //CardKeyから呼ばれる
    public void MoveDoor()
    {
        isOpen = !isOpen;
    }

    void Update()
    {
        if (isOpen)
        {
            if (isRightDoor)
                CloseRightDoor();
            else
                CloseLeftDoor();
        }
        else
        {
            if (isRightDoor)
                OpenRightDoor();
            else
                OpenLeftDoor();
        }
    }



    //*************************
    //******  RightDoor  ******
    //*************************
    public void OpenRightDoor()
    {
        Vector3 targetPos = transform.position;

        if (isRotate90D)
        {
            //90度回転しているときX座標を移動
            targetPos += new Vector3(movingSpeed * Time.deltaTime, 0f, 0f);
            targetPos.x = Mathf.Clamp(targetPos.x, initalPos.x, initalPos.x + moveingDistance);
        }
        else
        {
            //0°のときはZ座標を移動
            targetPos += new Vector3(0f, 0f, movingSpeed * Time.deltaTime);
            targetPos.z = Mathf.Clamp(targetPos.z, initalPos.z, initalPos.z + moveingDistance);
        }

        transform.position = targetPos;
    }

    public void CloseRightDoor()
    {
        Vector3 targetPos = transform.position;

        if (isRotate90D)
        {
            //90度回転しているときX座標を移動
            targetPos += new Vector3(-movingSpeed * Time.deltaTime, 0f);
            targetPos.x = Mathf.Clamp(targetPos.x, initalPos.x, initalPos.x + moveingDistance);

            if (transform.position.x == targetPos.x)
            {
                isCloseing = false;
            }
            else
            {
                isCloseing = true;
            }
        }
        else
        {
            //0°のときはZ座標を移動
            targetPos += new Vector3(0f, 0f, -movingSpeed * Time.deltaTime);
            targetPos.z = Mathf.Clamp(targetPos.z, initalPos.z, initalPos.z + moveingDistance);

            if (transform.position.z == targetPos.z)
            {
                isCloseing = false;
            }
            else
            {
                isCloseing = true;
            }
        }

        transform.position = targetPos;
    }


    //************************
    //******  LeftDoor  ******
    //************************
    public void OpenLeftDoor()
    {
        Vector3 targetPos = transform.position;

        if (isRotate90D)
        {
            //90度回転しているときX座標を移動
            targetPos += new Vector3(-movingSpeed * Time.deltaTime, 0f, 0f);
            targetPos.x = Mathf.Clamp(targetPos.x, initalPos.x + -moveingDistance, initalPos.x);
        }
        else
        {
            //0°のときはZ座標を移動
            targetPos += new Vector3(0f, 0f, -movingSpeed * Time.deltaTime);
            targetPos.z = Mathf.Clamp(targetPos.z, initalPos.z + -moveingDistance, initalPos.z);
        }

        transform.position = targetPos;
    }

    public void CloseLeftDoor()
    {
        Vector3 targetPos = transform.position;

        if (isRotate90D)
        {
            //90度回転しているときX座標を移動
            targetPos += new Vector3(movingSpeed * Time.deltaTime, 0f);
            targetPos.x = Mathf.Clamp(targetPos.x, initalPos.x + -moveingDistance, initalPos.x);

            if (transform.position.x == targetPos.x)
            {
                isCloseing = false;
            }
            else
            {
                isCloseing = true;
            }
        }
        else
        {
            //0°のときはZ座標を移動
            targetPos += new Vector3(0f, 0f, movingSpeed * Time.deltaTime);
            targetPos.z = Mathf.Clamp(targetPos.z, initalPos.z + -moveingDistance, initalPos.z);

            if (transform.position.z == targetPos.z)
            {
                isCloseing = false;
            }
            else
            {
                isCloseing = true;
            }
        }

        transform.position = targetPos;
    }

    void OnCollisionEnter(Collision hit)
    {
        if (hit.gameObject.tag != "Wall" && hit.gameObject.tag != "Key" && hit.gameObject.tag != "CardKey")
        {
            if (isCloseing)
            {
                isOpen = true;
            }
        }
    }

    public bool isClose
    {
        get { return isCloseing; }
    }
}
