using UnityEngine;

public class Canvas_Comunication : MonoBehaviour
{
    // 에디터에서 연결
    public GameObject TextWindowView;
    public KeyBoardView keyBoardView;

    void Start()
    {
        TextWindowView.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && keyBoardView.isSpacebarOn)
        {
            if (TextWindowView.activeSelf == false)
            {
                TextWindowView.SetActive(true);
                GameManager.Instance.Pause_theGame();
            }
        }
        
    }
}
