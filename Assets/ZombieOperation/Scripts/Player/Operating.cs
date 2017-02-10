using UnityEngine;
using System.Collections;

public enum OperatingType
{
    None,
    Wait,
    Move,
    Following,
    Attack,
}

public class Operating : MonoBehaviour
{
    public GameObject rayPointObject;
    public Circle circlePoint; //円を表示するシェーダー
    public GameObject selectionTarget; //レイの先の当たり判定用
    public Transform playerTransform;

    private OperatingType operatingType;
    private GameObject movePointObject = null;
    private GameObject selectedZombie = null;
    private GameObject selectedObject = null;
    private LineRenderer line;
    private RaycastHit hit;
    private Ray ray;

    void Start()
    {
        line = GetComponent<LineRenderer>();
        movePointObject = GameObject.Find("!MoveTarget");
        movePointObject.SetActive(false);
    }

    void Update()
    {
        ray.direction = rayPointObject.transform.forward;
        ray.origin = rayPointObject.transform.position;

        line.SetPosition(0, ray.origin);
        line.SetPosition(1, ray.GetPoint(200));
        line.enabled = true;

        if (Physics.Raycast(ray, out hit, 200.0f))
        {
            //circlePoint.SetPoint(hit.point);
        }
        else
        {
            //selectionTarget.SetActive(false);
        }
    }

    public void Decision()
    {
        if (!hit.collider) return;

        //ゾンビのGameObjectを取得
        if (hit.collider.tag == "Zombie")
        {
            if (hit.collider.GetComponent<ChildeCollider>().GetGameObject().GetComponent<Zombie>().isZombie)
            {
                GameObject zombie = hit.collider.GetComponent<ChildeCollider>().GetGameObject();
                if (selectedZombie != zombie)
                {
                    selectedObject = null;

                    if (selectedZombie != null)
                    {
                        selectedZombie.GetComponent<ChangeMaterial>().ChangeNormal();
                    }

                    selectedZombie = hit.collider.GetComponent<ChildeCollider>().GetGameObject();
                    selectedZombie.GetComponent<ChangeMaterial>().ChangeSelected();
                }
            }
        }

        if (selectedZombie != null)
        {
            //選択対象のGameObject取得
            if (hit.collider.tag == "Object" || hit.collider.tag == "Robot")
            {
                if (selectedObject != null)
                {
                    if (selectedObject.GetComponent<ChangeMaterialMesh>())
                        selectedObject.GetComponent<ChangeMaterialMesh>().ChangeNormal();
                    else
                        selectedObject.GetComponent<ChangeMaterial>().ChangeNormal();
                }
                selectedObject = null;

                selectedObject = hit.collider.gameObject;

                if (selectedObject.GetComponent<ChangeMaterialMesh>())
                    selectedObject.GetComponent<ChangeMaterialMesh>().ChangeSelected();
                else
                    selectedObject.GetComponent<ChangeMaterial>().ChangeSelected();
            }

            //移動
            if (hit.collider.tag == "Map")
            {
                if (selectedObject != null)
                {
                    if (selectedObject.GetComponent<ChangeMaterialMesh>())
                        selectedObject.GetComponent<ChangeMaterialMesh>().ChangeNormal();
                    else
                        selectedObject.GetComponent<ChangeMaterial>().ChangeNormal();
                }
                selectedObject = null;

                movePointObject.SetActive(true);
                movePointObject.transform.position = hit.point;
            }
            else
            {
                movePointObject.SetActive(false);
            }

            //生体認証
            if (hit.collider.tag == "BiometricsMachine")
            {
                if (selectedObject != null)
                {
                    if (selectedObject.GetComponent<ChangeMaterialMesh>())
                        selectedObject.GetComponent<ChangeMaterialMesh>().ChangeNormal();
                    else
                        selectedObject.GetComponent<ChangeMaterial>().ChangeNormal();
                }
                selectedObject = null;

                selectedObject = hit.collider.gameObject;
                selectedObject.GetComponent<ChangeMaterialMesh>().ChangeSelected();

                selectedZombie.GetComponent<Zombie>().Authentication(selectedObject.GetComponent<Biometrics>().MovePoint, selectedObject.transform.position);
            }

            //隔壁持ち上げ
            if (hit.collider.tag == "Bulkhead")
            {
                //壁が開いているときは呼ばない
                if (hit.collider.GetComponent<Bulkhead>().isOpen) return;
                if (!selectedZombie.GetComponent<Zombie>().isStrengthZombie) return;

                if (selectedObject != null)
                {
                    if (selectedObject.GetComponent<ChangeMaterialMesh>())
                        selectedObject.GetComponent<ChangeMaterialMesh>().ChangeNormal();
                    else
                        selectedObject.GetComponent<ChangeMaterial>().ChangeNormal();
                }
                selectedObject = null;

                selectedObject = hit.collider.gameObject;
                selectedObject.GetComponent<ChangeMaterial>().ChangeSelected();

                selectedZombie.GetComponent<Zombie>().LiftBulkhead(selectedObject.GetComponent<Bulkhead>().MovePoint, selectedObject.transform.position);
            }
        }
    }

    //待機
    public void OperatingWait()
    {
        //ゾンビの命令を実行
        if ((selectedZombie != null))
        {
            operatingType = OperatingType.Wait;
            selectedZombie.GetComponent<Zombie>().Wait();
            Debug.Log(operatingType + "実行");
        }
    }

    //移動
    public void OperatingMove()
    {
        //ゾンビの命令を実行
        if ((selectedZombie != null) && movePointObject.activeSelf)
        {
            operatingType = OperatingType.Move;
            selectedZombie.GetComponent<Zombie>().Move(movePointObject.transform);
            Debug.Log(operatingType + "実行");
        }
    }

    //追従
    public void OperatingFollowing()
    {
        //ゾンビの命令を実行
        if ((selectedZombie != null))
        {
            operatingType = OperatingType.Following;
            selectedZombie.GetComponent<Zombie>().Following(playerTransform);
            Debug.Log(operatingType + "実行");
        }
    }

    //攻撃
    public void OperatingAttack()
    {
        //ゾンビの命令を実行
        if ((selectedZombie != null) && (selectedObject != null))
        {
            if (selectedObject.tag != "Object" && selectedObject.tag != "Robot") return;

            operatingType = OperatingType.Attack;
            selectedZombie.GetComponent<Zombie>().Attack(selectedObject);
            selectedObject = null;
            Debug.Log(operatingType + "実行");
        }
    }
}
