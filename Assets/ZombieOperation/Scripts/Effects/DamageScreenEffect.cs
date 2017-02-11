using UnityEngine.UI;
using UnityEngine;

public class DamageScreenEffect : MonoBehaviour
{
    [SerializeField]
    private float speed = 0.01f;    //アルファ値減少スピード

    private Image screenBlood;
    private Color col;
    private float alpha = 0f;
    private bool isDamage = false;

    void Start()
    {
        screenBlood = GetComponent<Image>();
        col = screenBlood.color;
    }

    void Update()
    {
        if (alpha <= 0)
        {
            isDamage = false;
            alpha = 0f;
        }

        if (isDamage && 0 < alpha)
        {
            alpha -= speed * Time.deltaTime;
        }

        screenBlood.color = new Color(col.r, col.g, col.b, alpha);
    }

    public void Damage()
    {
        if (!isDamage)
        {
            isDamage = true;
            alpha = 1f;
        }
    }
}
