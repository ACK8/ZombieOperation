using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[System.Serializable]
public class 管理オブジェクトマスター
{
    public string オブジェクト名;
    public GameObject 登場人物;
    public bool 制御ロック = true;
    public int イベントNo = 0;
    //NPC及び、プレイヤーの移動先
    public Transform[] ルートポイント = new Transform[1];
}

[System.Serializable]
public class 管理イベントマスター
{
    public string イベント名;
    public float ボイス切り替え時間;
    public AudioClip ボイス;
}

[System.Serializable]
public class EventScenario : MonoBehaviour
{
    public GameObject 設置イベントリンク;
    public AudioSource ボイス設置;
    public 管理オブジェクトマスター[] 管理オブジェクト = new 管理オブジェクトマスター[1];
    public 管理イベントマスター[] 管理イベントデータ = new 管理イベントマスター[1];
    public int シナリオカウンター = 0;
    public float ボイスカウンター = 0.0f;
    public int イベントNo = -1;
    public GameObject[] ドロップボイス = new GameObject[1];
    public EventStageObject stageObj;

    public bool 処理座標セッター = false;
    public GameObject アイテム;

    //■以降、シナリオオブジェクトの子オブジェクトにする。

    void Start()
    {
        //ドア等のオブジェクトを非表示(河田追加)
        switch(イベントNo)
        {
            case 0:
                stageObj.SetActiveObjects(false);
                break;
            case 1:

                break;
        }
    }

    void Update()
    {
        特殊イベント(イベントNo);
    }

    //■データ消去
    void 消去処理()
    {
        Destroy(this.gameObject);
    }

    void 特殊イベント(int No)
    {
        switch (No)
        {
            case 0:
                チュートリアル1();
                break;
            case 1:
                上司との再会();
                break;
            case 2:
                break;
            default:
                //それ以外のイベント(ノーマル)
                break;
        }
    }
    //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
    //			チュートリアル1
    //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
    void チュートリアル1()
    {
        /*
		 * 管理オブジェクト [0].登場人物　NPC.A
		 * 管理オブジェクト [1].登場人物　プレイヤー
		 * 管理オブジェクト [2].登場人物　NPC.B
		 * 管理オブジェクト [3].登場人物　上司
		 * 管理オブジェクト [4].登場人物　ゾンビ
		*/

        //プレイヤーの座標を登場人物1に合わせる(強制移動用)
        if (管理オブジェクト[1].登場人物 != null)
        {
            設置イベントリンク.GetComponent<EventObject>().プレイヤー座標.transform.position = 管理オブジェクト[1].登場人物.transform.position;
            設置イベントリンク.GetComponent<EventObject>().プレイヤー座標.transform.rotation = 管理オブジェクト[1].登場人物.transform.rotation;
        }

        ボイスカウンター += 1.0f * Time.deltaTime;

        switch (シナリオカウンター)
        {
            case 0:
                //助手移動【研究室前まで移動】
                管理オブジェクト[0].登場人物.GetComponent<NavAI>().ターゲット = 管理オブジェクト[0].ルートポイント[0];
                アニメーション(0, 0);
                //助手がルートポイント0に2.0は以下まで近づいたら、カウンターアップ
                if (ボイスカウンター > 管理イベントデータ[シナリオカウンター].ボイス切り替え時間)
                {
                    if (Vector3.Distance(管理オブジェクト[0].登場人物.transform.position, 管理オブジェクト[0].ルートポイント[0].position) <= 2.0f)
                    {
                        Debug.Log("助手「博士ー、そろそろですよー?!」");
                        ALL_NPCNoMove(); //登場人物全員停止
                        シナリオカウンター++;
                        ボイス設定();
                    }
                }
                break;
            case 1:
                //助手セリフ【博士っ、そろそろですよっ!?】
                アニメーション(9, 0);
                対象に向く(管理オブジェクト[0].登場人物.transform, 管理オブジェクト[1].登場人物.transform);
                if (ボイスカウンター > 管理イベントデータ[シナリオカウンター].ボイス切り替え時間)
                {
                    ALL_NPCNoMove(); //登場人物全員停止
                    管理オブジェクト[1].登場人物.GetComponent<NavAI>().ターゲット = 管理オブジェクト[1].ルートポイント[0];
                    シナリオカウンター++;
                    ボイス設定();
                }
                break;
            case 2:
                //プレイヤー、助手に接近する	
                アニメーション(9, 0);
                if (ボイスカウンター > 管理イベントデータ[シナリオカウンター].ボイス切り替え時間)
                {
                    対象に向く(管理オブジェクト[0].登場人物.transform, 管理オブジェクト[1].登場人物.transform);
                    if (Vector3.Distance(管理オブジェクト[1].登場人物.transform.position, 管理オブジェクト[1].ルートポイント[0].position) <= 2.0f)
                    {
                        Debug.Log("助手「博士、行きましょう」");
                        ALL_NPCNoMove(); //登場人物全員停止
                        シナリオカウンター++;
                        ボイス設定();
                    }
                }
                break;
            case 3:
                //助手セリフ【博士っ、行きましょう】
                アニメーション(9, 0);
                if (ボイスカウンター > 管理イベントデータ[シナリオカウンター].ボイス切り替え時間)
                {
                    Debug.Log("助手「塩基配列がー」");
                    ALL_NPCNoMove(); //登場人物全員停止
                    管理オブジェクト[0].登場人物.GetComponent<NavAI>().ターゲット = 管理オブジェクト[0].ルートポイント[1];
                    管理オブジェクト[1].登場人物.GetComponent<NavAI>().ターゲット = 管理オブジェクト[1].ルートポイント[1];
                    シナリオカウンター++;
                    ボイス設定();
                }
                break;
            case 4:
                //助手会話しながら、目的地へ
                アニメーション(0, 0);
                if (ボイスカウンター > 管理イベントデータ[シナリオカウンター].ボイス切り替え時間)
                {
                    if (Vector3.Distance(管理オブジェクト[0].登場人物.transform.position, 管理オブジェクト[0].ルートポイント[1].position) <= 2.0f)
                    {
                        Debug.Log("助手「博士、研究成果楽しみにしてます」");
                        ALL_NPCNoMove(); //登場人物全員停止
                        シナリオカウンター++;
                        ボイス設定();
                    }
                }
                break;
            case 5:
                //助手セリフ【博士っ、研究成果楽しみにしています】
                アニメーション(9, 0);
                対象に向く(管理オブジェクト[0].登場人物.transform, 管理オブジェクト[1].登場人物.transform);
                if (ボイスカウンター > 管理イベントデータ[シナリオカウンター].ボイス切り替え時間)
                {
                    ALL_NPCNoMove(); //登場人物全員停止
                    管理オブジェクト[1].登場人物.GetComponent<NavAI>().ターゲット = 管理オブジェクト[1].ルートポイント[2];
                    シナリオカウンター++;
                    ボイス設定();
                }
                break;
            case 6:
                //実験室入室
                対象に向く(管理オブジェクト[0].登場人物.transform, 管理オブジェクト[1].登場人物.transform);
                if (ボイスカウンター > 管理イベントデータ[シナリオカウンター].ボイス切り替え時間)
                {
                    if (Vector3.Distance(管理オブジェクト[1].登場人物.transform.position, 管理オブジェクト[1].ルートポイント[2].position) <= 2.0f)
                    {
                        Debug.Log("研究員「お待ちしておりました。今回、博士の手で実験を行うそうですね、こちらへどうぞ」");
                        ALL_NPCNoMove(); //登場人物全員停止
                        シナリオカウンター++;
                        ボイス設定();
                    }
                }
                break;
            case 7:
                //NPCB【お待ちしておりました、今回、博士の手で実験を行うそうですね、こちらへどうぞ】
                アニメーション(9, 2);
                対象に向く(管理オブジェクト[2].登場人物.transform, 管理オブジェクト[1].登場人物.transform);
                if (ボイスカウンター > 管理イベントデータ[シナリオカウンター].ボイス切り替え時間)
                {
                    ALL_NPCNoMove(); //登場人物全員停止
                    管理オブジェクト[1].登場人物.GetComponent<NavAI>().ターゲット = 管理オブジェクト[1].ルートポイント[3];
                    管理オブジェクト[2].登場人物.GetComponent<NavAI>().ターゲット = 管理オブジェクト[2].ルートポイント[0];
                    管理オブジェクト[3].登場人物.GetComponent<NavAI>().ターゲット = 管理オブジェクト[3].ルートポイント[0];
                    シナリオカウンター++;
                    ボイス設定();
                }
                break;
            case 8:
                //NPCBがカウンターへ誘導
                if (Vector3.Distance(管理オブジェクト[2].登場人物.transform.position, 管理オブジェクト[0].ルートポイント[1].position) <= 1.0f)
                {
                    アニメーション(1, 2);
                }
                else
                {
                    アニメーション(0, 2);
                }
                if (Vector3.Distance(管理オブジェクト[3].登場人物.transform.position, 管理オブジェクト[0].ルートポイント[1].position) <= 1.0f)
                {
                    アニメーション(1, 3);
                }
                else
                {
                    アニメーション(0, 3);
                }
                if (ボイスカウンター > 管理イベントデータ[シナリオカウンター].ボイス切り替え時間)
                {
                    if (Vector3.Distance(管理オブジェクト[1].登場人物.transform.position, 管理オブジェクト[1].ルートポイント[3].position) <= 2.0f)
                    {
                        Debug.Log("上司「今回はー、ちゅうしゃきがー」");
                        ALL_NPCNoMove(); //登場人物全員停止
                        シナリオカウンター++;
                        ボイス設定();
                    }
                }
                break;
            case 9:
                //上司説明【注射器の操作説明】
                アニメーション(9, 2);
                アニメーション(1, 3);
                対象に向く(管理オブジェクト[2].登場人物.transform, 管理オブジェクト[1].登場人物.transform);
                対象に向く(管理オブジェクト[3].登場人物.transform, 管理オブジェクト[1].登場人物.transform);
                if (ボイスカウンター > 管理イベントデータ[シナリオカウンター].ボイス切り替え時間)
                {
                    Debug.Log("研究者「では、注射器を持って、死体に注射してください」");
                    ALL_NPCNoMove(); //登場人物全員停止
                    シナリオカウンター++;
                    ボイス設定();
                }
                break;
            case 10:
                //NPCB説明【注射器の操作説明】
                アニメーション(1, 2);
                if (ボイスカウンター > 管理イベントデータ[シナリオカウンター].ボイス切り替え時間)
                {
                    Debug.Log("研究者「やった、やりましたよっ!」");
                    ALL_NPCNoMove(); //登場人物全員停止
                    シナリオカウンター++;
                    ボイス設定();
                }
                break;
            case 11:
                //【注射器を使用】
                対象に向く(管理オブジェクト[2].登場人物.transform, 管理オブジェクト[4].登場人物.transform);
                アニメーション(1, 2);
                アニメーション(1, 3);
                //NPCBセリフ【やった、やりましたよ!!】
                if (ボイスカウンター > 管理イベントデータ[シナリオカウンター].ボイス切り替え時間)
                {
                    if (管理オブジェクト[4].登場人物.GetComponent<EventZombie>().isZombie == true)
                    {
                        アイテム.SetActive(true);
                        対象に向く(管理オブジェクト[3].登場人物.transform, 管理オブジェクト[1].登場人物.transform);
                        アニメーション(8, 3);
                        アニメーション(6, 4);
                        Debug.Log("上司「ふむ・・・・・・・・では、最後の実験を・・・」");
                        ALL_NPCNoMove(); //登場人物全員停止
                        シナリオカウンター++;
                        ボイス設定();
                    }
                }
                break;
            case 12:
                //上司セリフ【・・・・】
                アニメーション(9, 2);
                対象に向く(管理オブジェクト[3].登場人物.transform, 管理オブジェクト[4].登場人物.transform);
                if (ボイスカウンター > 管理イベントデータ[シナリオカウンター].ボイス切り替え時間)
                {
                    対象に向く(管理オブジェクト[3].登場人物.transform, 管理オブジェクト[4].登場人物.transform);
                    Debug.Log("上司「ぽちっとな」");
                    ALL_NPCNoMove(); //登場人物全員停止
                    シナリオカウンター++;
                    ボイス設定();
                }
                break;
            case 13:
                //上司リモコン操作【セリフ】
                if (ボイスカウンター > 管理イベントデータ[シナリオカウンター].ボイス切り替え時間)
                {
                    対象に向く(管理オブジェクト[2].登場人物.transform, 管理オブジェクト[1].登場人物.transform);
                    対象に向く(管理オブジェクト[3].登場人物.transform, 管理オブジェクト[1].登場人物.transform);
                    ドロップアイテム(0, 管理オブジェクト[4].登場人物.transform);
                    アニメーション(7, 4);
                    Debug.Log("ゾンビ「うぉぉおぉぉぉぉぉおぉおぉおおぉおぉぉっ!!!」");
                    ALL_NPCNoMove(); //登場人物全員停止
                    シナリオカウンター++;
                    ボイス設定();
                }
                break;
            case 14:
                //ゾンビプレイヤーを襲う
                対象に向く(管理オブジェクト[4].登場人物.transform, 設置イベントリンク.GetComponent<EventObject>().プレイヤー頭座標.transform);
                //対象に向く(管理オブジェクト [4].登場人物.transform,管理オブジェクト [1].登場人物.transform);
                if (ボイスカウンター > 管理イベントデータ[シナリオカウンター].ボイス切り替え時間)
                {
                    //プレイヤー画面真っ赤
                    ドロップアイテム(1, 管理オブジェクト[1].登場人物.transform);
                    設置イベントリンク.GetComponent<EventObject>().フェードインアウト管理.ダメージ = true;
                    Debug.Log("研究者「ど、どういうことだ?!」");
                    ALL_NPCNoMove(); //登場人物全員停止
                    シナリオカウンター++;
                    ボイス設定();
                }
                break;
            case 15:
                //NPCBセリフ【な、なっ?!ど、どういうことですか?!】
                アニメーション(9, 2);
                対象に向く(管理オブジェクト[2].登場人物.transform, 管理オブジェクト[4].登場人物.transform);
                if (ボイスカウンター > 管理イベントデータ[シナリオカウンター].ボイス切り替え時間)
                {
                    ALL_NPCNoMove(); //登場人物全員停止
                    対象に向く(管理オブジェクト[2].登場人物.transform, 管理オブジェクト[4].登場人物.transform);
                    対象に向く(管理オブジェクト[3].登場人物.transform, 管理オブジェクト[4].登場人物.transform);
                    管理オブジェクト[4].登場人物.GetComponent<NavAI>().ターゲット = 管理オブジェクト[2].登場人物.transform;
                    シナリオカウンター++;
                    ボイス設定();
                }
                break;
            case 16:
                //ゾンビ、NPCBへ移動
                アニメーション(0, 4);
                アニメーション(4, 2);
                if (Vector3.Distance(管理オブジェクト[2].登場人物.transform.position, 管理オブジェクト[4].登場人物.transform.position) <= 2.0f)
                {
                    アニメーション(5, 4);
                    アニメーション(8, 3);
                    if (ボイスカウンター > 管理イベントデータ[シナリオカウンター].ボイス切り替え時間)
                    {
                        ドロップアイテム(1, 管理オブジェクト[4].登場人物.transform);
                        Debug.Log("研究者「ぎゃああぁぁぁぁぁぁあああっ!」");
                        ALL_NPCNoMove(); //登場人物全員停止
                        シナリオカウンター++;
                        ボイス設定();
                    }
                }
                break;
            case 17:
                //NPCB【ぎゃぁぁぁぁぁ】
                対象に向く(管理オブジェクト[4].登場人物.transform, 管理オブジェクト[2].登場人物.transform);
                対象に向く(管理オブジェクト[3].登場人物.transform, 管理オブジェクト[4].登場人物.transform);
                アニメーション(1, 4);
                アニメーション(3, 2);

                if (ボイスカウンター > 管理イベントデータ[シナリオカウンター].ボイス切り替え時間)
                {
                    Debug.Log("上司「なっ、こっちへ来るなっ!!くそっ、コントロールが!?使えない、ちぃっ!!!」");
                    ALL_NPCNoMove(); //登場人物全員停止
                    シナリオカウンター++;
                    ボイス設定();
                }
                break;
            case 18:
                //上司【コントロールが効かないだと?! ちっ】
                対象に向く(管理オブジェクト[3].登場人物.transform, 管理オブジェクト[4].登場人物.transform);
                対象に向く(管理オブジェクト[4].登場人物.transform, 管理オブジェクト[3].登場人物.transform);
                if (ボイスカウンター > 管理イベントデータ[シナリオカウンター].ボイス切り替え時間)
                {
                    ALL_NPCNoMove(); //登場人物全員停止
                    管理オブジェクト[3].登場人物.GetComponent<NavAI>().ターゲット = 管理オブジェクト[3].ルートポイント[1];
                    管理オブジェクト[4].登場人物.GetComponent<NavAI>().ターゲット = 管理オブジェクト[3].登場人物.transform;
                    シナリオカウンター++;
                    ボイス設定();
                }
                break;
            case 19:
                //上司逃亡
                アニメーション(0, 4);
                アニメーション(2, 3);
                if (ボイスカウンター > 管理イベントデータ[シナリオカウンター].ボイス切り替え時間)
                {
                    設置イベントリンク.GetComponent<EventObject>().フェードインアウト管理.ダメージ = true;
                    設置イベントリンク.GetComponent<EventObject>().フェードインアウト管理.フェードフラグ = 2;
                    //ALL_NPCNoMove (); //登場人物全員停止
                    シナリオカウンター++;
                    ボイス設定();
                    //イベント終了
                }
                break;
            case 20:
                //NPC達【絶叫】
                if (ボイスカウンター > 管理イベントデータ[シナリオカウンター].ボイス切り替え時間)
                {
                    Debug.Log("みんな「ぎゃぁあああぁぁ」");
                    //ALL_NPCNoMove (); //登場人物全員停止
                    シナリオカウンター++;
                    //ボイス設定 ();
                    //イベント終了
                }
                break;
            case 21:
                //フェードアウト
                if (ボイスカウンター > 管理イベントデータ[シナリオカウンター].ボイス切り替え時間)
                {
                    シナリオカウンター++;
                    //ボイス設定 ();
                    登場人物消去();
                }
                break;
            case 22:
                //フェードイン・初期化
                設置イベントリンク.GetComponent<EventObject>().フェードインアウト管理.ダメージ = false;
                設置イベントリンク.GetComponent<EventObject>().フェードインアウト管理.フェードフラグ = 1;
                if (ボイスカウンター > 管理イベントデータ[シナリオカウンター].ボイス切り替え時間)
                {
                    シナリオカウンター++;
                    //ボイス設定 ();
                }
                break;
            case 23:
                //イベント終了処理
                //設置イベントリンク.GetComponent<EventObject>().イベント終了フラグ = true;

                //ドア等のオブジェクトを表示(河田追加)
                stageObj.SetActiveObjects(true);

                Destroy(this.gameObject);
                break;
            default:
                //それ以外のイベント(ノーマル)
                break;
        }
    }

    //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
    //			上司との再会
    //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
    void 上司との再会()
    {
        /*
		 * 管理オブジェクト [0].登場人物　上司
		 * 管理オブジェクト [1].登場人物　プレイヤー
		*/

        if (処理座標セッター == false)
        {
            管理オブジェクト[1].登場人物.transform.position = 設置イベントリンク.GetComponent<EventObject>().プレイヤー座標.transform.position;
            管理オブジェクト[1].登場人物.transform.rotation = 設置イベントリンク.GetComponent<EventObject>().プレイヤー座標.transform.rotation;
            処理座標セッター = true;
        }

        //プレイヤーの座標を登場人物1に合わせる(強制移動用・プレイヤー座標が基準)
        if (管理オブジェクト[1].登場人物 != null)
        {
            設置イベントリンク.GetComponent<EventObject>().プレイヤー座標.transform.position = 管理オブジェクト[1].登場人物.transform.position;
            設置イベントリンク.GetComponent<EventObject>().プレイヤー座標.transform.rotation = 管理オブジェクト[1].登場人物.transform.rotation;
        }

        ボイスカウンター += 1.0f * Time.deltaTime;

        switch (シナリオカウンター)
        {
            case 0:
                //プレイヤーが部屋の中を散策
                管理オブジェクト[1].登場人物.GetComponent<NavAI>().ターゲット = 管理オブジェクト[1].ルートポイント[0];
                //上司が部屋から出現する
                管理オブジェクト[0].登場人物.GetComponent<NavAI>().ターゲット = 管理オブジェクト[0].ルートポイント[0];
                //アタッシュケースもち
                アイテム.SetActive(true);
                アニメーション(0, 0);
                //プレイヤーがルートポイント0に2.0は以下まで近づいたら、カウンターアップ
                if (ボイスカウンター > 管理イベントデータ[シナリオカウンター].ボイス切り替え時間)
                {
                    if (Vector3.Distance(管理オブジェクト[0].登場人物.transform.position, 管理オブジェクト[0].ルートポイント[0].position) <= 2.0f)
                    {
                        Debug.Log("上司と再会する");
                        ALL_NPCNoMove(); //登場人物全員停止
                        シナリオカウンター++;
                        ボイス設定();
                    }
                }
                break;
            case 1:
                //上司セリフ【】
                アニメーション(9, 0);
                対象に向く(管理オブジェクト[0].登場人物.transform, 管理オブジェクト[1].登場人物.transform);
                if (ボイスカウンター > 管理イベントデータ[シナリオカウンター].ボイス切り替え時間)
                {
                    ALL_NPCNoMove(); //登場人物全員停止
                    管理オブジェクト[0].登場人物.GetComponent<NavAI>().ターゲット = 管理オブジェクト[0].ルートポイント[1];
                    シナリオカウンター++;
                    ボイス設定();
                }
                break;
            case 2:
                //上司退去
                アニメーション(2, 0);
                if (ボイスカウンター > 管理イベントデータ[シナリオカウンター].ボイス切り替え時間)
                {
                    ALL_NPCNoMove(); //登場人物全員停止
                    シナリオカウンター++;
                    ボイス設定();
                }
                break;
            case 3:
                //イベント終了処理
                設置イベントリンク.GetComponent<EventObject>().イベント終了フラグ = true;
                break;
            default:
                //それ以外のイベント(ノーマル)
                break;
        }
    }
    void ALL_NPCNoMove()
    {
        for (int i = 0; i < 管理オブジェクト.Length; i++)
        {
            管理オブジェクト[i].登場人物.GetComponent<NavAI>().ターゲット = null;
        }
        ボイスカウンター = 0.0f;
    }
    void 登場人物消去()
    {
        for (int i = 0; i < 管理オブジェクト.Length; i++)
        {
            Destroy(管理オブジェクト[i].登場人物);
        }
    }

    void 対象に向く(Transform 向く人, Transform ターゲット)
    {
        /*
		float Speed = 3.0f;
		var relativePos = ターゲット.position - 向く人.position;
		var rotation = Quaternion.LookRotation(relativePos);
		transform.rotation =
			Quaternion.Slerp (transform.rotation, rotation, Time.deltaTime * Speed);
		向く人.LookAt (ターゲット);
				*/


        Vector3 ターゲットY座標補正 = new Vector3(
            ターゲット.position.x,
            向く人.position.y,
            ターゲット.position.z);

        var 角度調整 = Quaternion.LookRotation(ターゲットY座標補正 - 向く人.position).eulerAngles;
        向く人.rotation = Quaternion.Slerp(向く人.rotation, Quaternion.Euler(角度調整), Time.deltaTime);
    }

    void アニメーション(int flag, int No)
    {
        float 移動スピード = 管理オブジェクト[No].登場人物.GetComponent<Animator>().GetFloat("Speed");

        switch (flag)
        {
            case 0:
                //歩く
                //加算
                移動スピード += 0.5f * Time.deltaTime;
                if (移動スピード > 0.5f)
                {
                    移動スピード = 0.5f;
                }
                管理オブジェクト[No].登場人物.GetComponent<Animator>().SetBool("会話", false);
                break;
            case 1:
                //止まる
                //減算
                移動スピード -= 0.25f * Time.deltaTime;
                if (移動スピード < 0.0f)
                {
                    移動スピード = 0.0f;
                }
                管理オブジェクト[No].登場人物.GetComponent<Animator>().SetBool("会話", false);
                break;
            case 2:
                //走る
                移動スピード += 1.0f * Time.deltaTime;
                if (移動スピード > 1.0f)
                {
                    移動スピード = 1.0f;
                }
                管理オブジェクト[No].登場人物.GetComponent<Animator>().SetBool("会話", false);
                break;
            case 3:
                //死亡
                管理オブジェクト[No].登場人物.GetComponent<Animator>().SetBool("会話", false);
                管理オブジェクト[No].登場人物.GetComponent<Animator>().SetBool("Dead", true);
                break;
            case 4:
                //怯え
                管理オブジェクト[No].登場人物.GetComponent<Animator>().SetBool("会話", false);
                管理オブジェクト[No].登場人物.GetComponent<Animator>().SetBool("BackMove", true);
                break;
            case 5:
                //襲う
                管理オブジェクト[No].登場人物.GetComponent<Animator>().SetBool("会話", false);
                管理オブジェクト[No].登場人物.GetComponent<Animator>().SetTrigger("Attack");
                break;
            case 6:
                //蘇生
                管理オブジェクト[No].登場人物.GetComponent<Animator>().SetBool("会話", false);
                管理オブジェクト[No].登場人物.GetComponent<Animator>().SetBool("Dead", false);
                break;
            case 7:
                //雄たけび攻撃
                管理オブジェクト[No].登場人物.GetComponent<Animator>().SetBool("会話", false);
                管理オブジェクト[No].登場人物.GetComponent<Animator>().SetTrigger("Otakebi");
                break;
            case 8:
                //スイッチを押す
                管理オブジェクト[No].登場人物.GetComponent<Animator>().SetBool("会話", false);
                管理オブジェクト[No].登場人物.GetComponent<Animator>().SetTrigger("スイッチを押す");
                break;
            case 9:
                //会話
                管理オブジェクト[No].登場人物.GetComponent<Animator>().SetBool("会話", true);
                break;
        }
        管理オブジェクト[No].登場人物.GetComponent<Animator>().SetFloat("Speed", 移動スピード);
    }

    void ボイス設定()
    {
        if (管理イベントデータ[シナリオカウンター].ボイス != null)
        {
            ボイス設置.loop = false;
            ボイス設置.Stop();
            ボイス設置.clip = 管理イベントデータ[シナリオカウンター].ボイス;
            ボイス設置.Play();
        }
    }

    void ドロップアイテム(int ドロップナンバー, Transform 出現位置)
    {
        Instantiate(ドロップボイス[ドロップナンバー],
            出現位置.position,
            出現位置.rotation);
    }
}
