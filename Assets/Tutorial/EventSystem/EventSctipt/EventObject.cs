using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * イベント単体のシステム
 * 登場人物や、メッセージ類をすべて管理し、すべて終わると
 * 管理して医務ゲームオブジェクトを開放する
*/
[System.Serializable]
public class 管理シナリオマスター
{
	public string シナリオ名;
	public GameObject 設置シナリオ;
	public GameObject 発動フラグ;
	public bool 実行フラグ;
}
[System.Serializable]
public class EventObject : MonoBehaviour 
{
	public GameObject プレイヤー座標;
	public GameObject プレイヤー頭座標;
	public GameObject プレイヤー右手座標;
	public GameObject プレイヤー左手座標;
	public FIO_System フェードインアウト管理;

	public bool イベント終了フラグ = false;
	public 管理シナリオマスター[] 管理シナリオ = new 管理シナリオマスター[2];

	public GameObject 全扉施錠プログラム;

	//イベントオブジェクトの配置
	void Start () 
	{
	}
	
	void Update () 
	{
		for (int i = 0; i < 管理シナリオ.Length; i++) 
		{
			//シナリオのオブジェクトがある
			if (管理シナリオ [i].設置シナリオ != null) 
			{
				//実行されていない
				if (管理シナリオ [i].実行フラグ == false)
				{
					//発動フラグがある
					if (管理シナリオ [i].発動フラグ != null) 
					{
						if(Vector3.Distance(管理シナリオ [i].発動フラグ.transform.position,プレイヤー座標.transform.position) < 1.5f)
							{
							//発動フラグ位置から1.5f以内の距離に入ったら発動
							管理シナリオ [i].実行フラグ = true;
							管理シナリオ [i].設置シナリオ.SetActive (true);
							}
					}
					else
					{
						//発動フラグがない場合、強制的に、イベントをスタートする
						管理シナリオ [i].実行フラグ = true;
						管理シナリオ [i].設置シナリオ.SetActive (true);
					}
				}
			}
		}
		int 消去カウント = 0;
		for(int i=0;i<管理シナリオ.Length;i++)
		{
			if (管理シナリオ [i].設置シナリオ != null) 
			{
				消去カウント++;
			}
		}
		if(消去カウント== 0)
		{
			消去処理 ();	
		}
	}


	//■データ消去
	void 消去処理()
	{
		Destroy (this.gameObject);
	}
}
