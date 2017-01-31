using UnityEngine;
using System.Collections;
using UnityEngine.UI;     //UIを使用可能にする
/*
 * ノーマルのフェードインアウト用プログラム
*/
public class FIO_System : MonoBehaviour 
{
	public bool ダメージ = false;
	public int フェードフラグ = 0;
	/*
	 * 0 起動しない
	 * 1　フォードアウト[ダメージがfalseに]
	 * 2　フェードイン[ダメージがfalseに]
	*/
	public float スピード = 0.01f;  //透明化の速さ
	float アルファ;    //A値を操作するための変数
	float 赤, 緑, 青;    //RGBを操作するための変数


	void Start ()
	{
		　　　　　//Panelの色を取得
		赤 = GetComponent<Image>().color.r;
		緑 = GetComponent<Image>().color.g;
		青 = GetComponent<Image>().color.b;
	}
	void Update () 
	{
		switch (フェードフラグ) 
		{
		case 0:
			if (ダメージ == true) 
			{
				Red_DMG ();
			}
			break;
		case 1:
			F_OUT ();
			break;
		case 2:
			F_IN ();
			break;
		}
	}
	void Red_DMG()
	{
		赤 = 1.0f;
		緑 = 0.5f;
		青 = 0.5f;
		アルファ = 0.5f;
		GetComponent<Image>().color = new Color(赤, 緑, 青, アルファ);
	}
	void F_IN()
	{
		赤 = GetComponent<Image> ().color.r - (1.0f * Time.deltaTime);
		if (赤 < 0.0f)
			赤 = 0.0f;
		緑 = GetComponent<Image> ().color.g - (1.0f * Time.deltaTime);
		if (緑 < 0.0f)
			緑 = 0.0f;
		青 = GetComponent<Image> ().color.b - (1.0f * Time.deltaTime);
		if (青 < 0.0f)
			青 = 0.0f;
		アルファ += 1.0f * Time.deltaTime;
		if (アルファ > 1.0f)
			アルファ = 1.0f;
		GetComponent<Image>().color = new Color(赤, 緑, 青, アルファ);
	}
	void F_OUT()
	{
		赤 = GetComponent<Image> ().color.r + (1.0f * Time.deltaTime);
		if (赤 > 1.0f)
			赤 = 1.0f;
		緑 = GetComponent<Image> ().color.g + (1.0f * Time.deltaTime);
		if (緑 > 1.0f)
			緑 = 1.0f;
		青 = GetComponent<Image> ().color.b + (1.0f * Time.deltaTime);
		if (青 > 1.0f)
			青 = 1.0f;
		アルファ -= 1.0f * Time.deltaTime;
		if (アルファ < 0.0f)
			アルファ = 0.0f;
		GetComponent<Image>().color = new Color(赤, 緑, 青, アルファ);
	}

}
