using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public int _hp = 100;
    private bool _isAlive = true;

    void Start()
    {

    }

    void Update()
    {
        if(!_isAlive)
        {
            print("Player Dead");
        }
    }

    public void DecrementHP(int val)
    {
        _hp -= val;

        if (_hp <= 0)
        {
            _isAlive = false;
            _hp = 0;
        }
    }

    public bool isAlive
    {
        get { return _isAlive; }
    }

}
