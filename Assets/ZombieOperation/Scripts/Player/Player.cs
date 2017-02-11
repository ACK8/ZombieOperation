using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    [SerializeField]
    private DamageScreenEffect blood;
    [SerializeField]
    private DeahScreenEffect death;

    public int _hp = 100;

    private bool _isAlive = true;

    void Start()
    {

    }

    void Update()
    {
        if (!_isAlive)
        {
            death.Death();
        }
    }

    public void DecrementHP(int val)
    {
        blood.Damage();

        _hp -= val;

        if (_hp <= 0)
        {
            _hp = 0;
            _isAlive = false;
        }
    }

    public bool isAlive
    {
        get { return _isAlive; }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet")
        {
            DecrementHP(5);
        }
    }
}
