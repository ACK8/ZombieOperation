using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SwitchStage : MonoBehaviour {

    public Elevator elevator;
    public string SceneName;
    private bool isChange=false;

	void Start ()
    {
	
	}
	

	void Update ()
    {
	
	}

    void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.tag=="Player")
        {
            if(elevator.isElevatorCloseing&& isChange==false)//エレベーターが閉まる
            {
                isChange = true;
                SceneManager.LoadScene(SceneName);
            }
        }
    }

}
