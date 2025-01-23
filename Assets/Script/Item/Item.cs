using UnityEngine;

public abstract class Item : MonoBehaviour
{
    GameObject Bottom;
    void Start()
    {
        Bottom = GameObject.Find("wall_Bottom");
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            onTriggerPlayer(collision.gameObject);
            OutofScreen();
        }
        else if(collision.gameObject == Bottom)
        {
            OutofScreen();
        }
        else
        {
            //Debug.Log("not Player");
        }
    }

    protected abstract void onTriggerPlayer(GameObject player);

    void Update()
    {
        //게임 종료시 제거
        if (GameManager.Instance.isGameStart == false)
        {
            OutofScreen();
        }

        // 아래로 움직임
        Vector2 pos = transform.position;
        pos.y -= GameManager.Instance.gameSpeed * Time.deltaTime;
        transform.position = pos;

    }

    protected abstract void OutofScreen();
}
