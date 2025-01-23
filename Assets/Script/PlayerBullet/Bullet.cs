using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    public int damage { get; protected set; }

    private void Start()
    {
        
    }

    void Update()
    {
        
        Vector2 pos = transform.position;
        pos.y += GameManager.Instance.gameSpeed * Time.deltaTime;
        transform.position = pos;

        // 게임 종료시에도 총알 제거
        if(GameManager.Instance.isGameStart == false)
        {
            OutOfScreen();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Wall")
        {
            OutOfScreen();
        }
        else if(collision.tag == "Enemy")
        {
            OutOfScreen();
        }
    }

    public abstract void OutOfScreen();

    public void SetActive()
    {
        gameObject.SetActive(true);
    }
    public void SetDeActive()
    {
        gameObject.SetActive(false);
    }
}
