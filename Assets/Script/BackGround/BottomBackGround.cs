using UnityEngine;

public class BottomBackGround : MonoBehaviour
{
    public GameObject[] bg;
    public Sprite bgSprite;
    float bgheight;

    float scollSpeed = 2.0f;
    void Start()
    {
        bgheight = bgSprite.bounds.size.y;

        //Debug.Log(bgheight);
        FindPositios();
    }

    void FindPositios()
    {
        for (int i = 0; i < bg.Length; i++)
        {
            Vector3 pos = Vector3.zero + new Vector3(0, bgheight, 0);
            bg[i].transform.position = pos + new Vector3(0, -bgheight, 0) * i;
        }
    }

    void Update()
    {
        for (int i = 0; i < bg.Length; i++)
        {
            Vector3 pos = bg[i].transform.position;
            pos.y -= scollSpeed * Time.deltaTime;
            bg[i].transform.position = pos;

            if (bg[i].transform.position.y < -20)
            {
                pos = bg[i].transform.position;
                pos.y += bgheight * 3;
                bg[i].transform.position = pos;
            }
        }
    }
}