using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * イベントを発生し制御するオブジェクトを制御する
 * イベントは、クリア制で、基本１本道システムとする。
*/
[System.Serializable]
public class イベントデータマスター 
{
	public string イベント名;
	public bool 設置フラグ;
	public GameObject 設置イベントオブジェクト;
}

[System.Serializable]
public class EventManager : MonoBehaviour 
{
	public GameObject プレイヤー座標;
	public GameObject プレイヤー右手座標;
	public GameObject プレイヤー左手座標;
	public イベントデータマスター[] イベントデータ = new イベントデータマスター[1];
	public int イベントカウンター = 0;
	public GameObject 進行形イベント;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		イベント進行チェック ();
		if (進行形イベント != null) 
		{
			イベントカウンター++;
		}
	}

	void イベント進行チェック()
	{
		if (イベントデータ.Length < イベントカウンター) 
		{
			if(イベントデータ[イベントカウンター].設置フラグ == false)
			{
				イベント設置開始(イベントカウンター);
			}
		}
	}

	void イベント設置開始(int No)
	{
		//設置フラグを立てる(二重設置をさせない)
		イベントデータ [No].設置フラグ = true;
		//イベントオブジェクトを設置する
		進行形イベント = (GameObject)Instantiate (
			イベントデータ [No].設置イベントオブジェクト,
			this.transform.position,
			this.transform.rotation);
		進行形イベント.GetComponent<EventManager> ().プレイヤー座標 = プレイヤー座標;
		進行形イベント.GetComponent<EventManager> ().プレイヤー右手座標 = プレイヤー右手座標;
		進行形イベント.GetComponent<EventManager> ().プレイヤー左手座標 = プレイヤー左手座標;
	}
}
