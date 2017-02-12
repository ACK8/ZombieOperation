using UnityEngine;

//イベント実行時、ステージにあるオブジェクトの表示を設定
public class EventStageObject : MonoBehaviour
{
    [SerializeField]
    private GameObject[] stageObjects;
    
    public void SetActiveObjects(bool f)
    {
        foreach (GameObject g in stageObjects)
        {
            g.SetActive(f);
        }
    }
}
