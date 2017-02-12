using UnityEngine;
using System.Collections;

public class FaedCheck : MonoBehaviour
{
	public bool FOFlag = false;
	void Start()
	{
	}
	void Update()
	{
		if (FOFlag == true) {
			SteamVR_Fade.Start (Color.black, 1.0f);
		} else {
			SteamVR_Fade.Start (Color.clear, 1.0f);
		}
	}
}