using UnityEngine;
using UnityEngine.Events;

public class HeavensGate : MonoBehaviour
{
    [SerializeField]
    private UnityEvent gateOpenEvent;
    [SerializeField]
    private LampColor[] gateLights;     //ゲートの近くにあるランプ
    [SerializeField]
    private LampColor[] leverLights;    //レバーの近くにあるランプ

    private bool[] openFlag = new bool[3] { false, false, false };
    private bool isOpen = false;

    void Start()
    {
        //開いてないときランプを赤色
        foreach (LampColor l in gateLights)
        {
            l.ColorChange(Color.red);
        }

        foreach (LampColor l in leverLights)
        {
            l.ColorChange(Color.red);
        }
    }

    void Update()
    {
        //すべてのレバーを下げるとゲートが開く
        if (openFlag[0] && openFlag[1] && openFlag[2] && !isOpen)
        {
            gateOpenEvent.Invoke();
            isOpen = true;
        }
    }

    //レバーから呼ばれる(Unity Event)
    public void LeverOn(int index)
    {
        gateLights[index].ColorChange(Color.blue);
        leverLights[index].ColorChange(Color.blue);
        openFlag[index] = true;
    }
}
