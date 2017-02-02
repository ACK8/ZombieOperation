using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]
    private float movingSpeed = 0f;
    [SerializeField]
    private float moveingDistance = 1;  //開く距離
    [SerializeField]
    private float autoCloseTime = 2f;
    [SerializeField]
    private bool isRotate90D = false;   //ドアが90度回転しているか
    [SerializeField]
    private bool isOpenLeft = false;
    [SerializeField]
    private bool isAutoClose = false;

    private Vector3 initalPos;
    private bool _isOpen = false;    //現在ドアが開いているか
    private bool isCloseing = false;    //ドアが閉じているときtrue

    void Start()
    {
        initalPos = this.transform.position;
    }

    //CardKeyから呼ばれる
    public void MoveDoor()
    {
        _isOpen = !_isOpen;
    }

    void Update()
    {
        if (_isOpen)
        {
            OpenDoor();

            if (isAutoClose)
                Invoke("AutoClose", autoCloseTime);
        }
        else
        {
            CloseDoor();
        }
    }

    public void OpenDoor()
    {
        Vector3 targetPos = transform.position;

        if (isOpenLeft)
        {
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
        }
        else
        {
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
        }

        transform.position = targetPos;
    }

    public void CloseDoor()
    {
        Vector3 targetPos = transform.position;

        if (isOpenLeft)
        {
            if (isRotate90D)
            {
                //90度回転しているときX座標を移動
                targetPos += new Vector3(-movingSpeed * Time.deltaTime, 0f, 0f);
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
        }
        else
        {
            if (isRotate90D)
            {
                //90度回転しているときX座標を移動
                targetPos += new Vector3(movingSpeed * Time.deltaTime, 0f, 0f);
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
        }
        transform.position = targetPos;
    }

    void AutoClose()
    {
        _isOpen = false;
    }

    void OnCollisionEnter(Collision hit)
    {
        if (hit.gameObject.tag != "Map" && hit.gameObject.tag != "Wall" && hit.gameObject.tag != "Key" && hit.gameObject.tag != "CardKey")
        {
            if (isCloseing)
            {
                _isOpen = true;
            }
        }
    }

    public bool isOpen
    {
        get { return _isOpen; }
    }
}
