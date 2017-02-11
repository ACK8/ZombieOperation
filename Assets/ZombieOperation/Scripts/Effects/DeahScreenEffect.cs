using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class DeahScreenEffect : MonoBehaviour
{
    [SerializeField]
    private float speed = 0.01f;    //アルファ値減少スピード

    private Image screenBlood;
    private Color col;
    private float alpha = 0f;
    private bool isDeath = false;

    void Start()
    {
        screenBlood = GetComponent<Image>();
        col = screenBlood.color;
    }

    void Update()
    {
        if (1f <= alpha)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            isDeath = false;
            alpha = 0f;
        }

        if (isDeath && alpha < 1f)
        {
            alpha += speed * Time.deltaTime;
        }

        screenBlood.color = new Color(col.r, col.g, col.b, alpha);
    }

    public void Death()
    {
        if (!isDeath)
        {
            isDeath = true;
            alpha = 0f;
        }
    }
}
