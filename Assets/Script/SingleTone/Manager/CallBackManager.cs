using UnityEngine;
using UnityEngine.Events;

//public enum CallBackCode
//{
//    TextWindowPopUp_Open,
//    TextWindowPopUp_Close,
//    ChangeMapToOutsideOfHouse

//}


public class CallBackManager : Singleton<CallBackManager>
{
    // 이니셜라이저로 연결
    [SerializeField] private GameObject insideOfHouse;
    [SerializeField] private GameObject outsideOfHouse;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject interfaceView;
    [SerializeField] private GameObject textWindowView;

    public GameObject __insideOfHouse { get { return insideOfHouse; } set { insideOfHouse = value; } }
    public GameObject __outsideOfHouse { get { return outsideOfHouse; } set { outsideOfHouse = value; } }
    public GameObject __player { get { return player; } set { player = value; } }
    public GameObject __interfaceView { get { return interfaceView; } set { interfaceView = value; } }
    public GameObject __textWindowView { get { return textWindowView; } set { textWindowView = value; } }

    void Start()
    {
        
    }
    private void FixedUpdate()
    {
        
    }

    // csv에서 인덱스만으로 함수를 선택할 수있도록 만듬
    public UnityAction CallBackList(int index)
    {
        UnityAction unityAction = () =>
        {
            switch (index)
            {
                case 0 : TextWindowPopUp_Open(); break;
                case 1 : TextWindowPopUp_Close(); break;
                case 2 : ChangeMapToOutsideOfHouse(); break;
                case 3 : ChangeMapToInsideOfHouse(); break;
            }
        };

        return unityAction;
    }

    public void ChangeMapToOutsideOfHouse()
    {
        if(player == null)
        {
            player = PlayerMoveAndAnime.Instance.gameObject;
        }
        player.transform.position = Vector2.zero;
        __insideOfHouse.SetActive(false);
        __outsideOfHouse.SetActive(true);
        TextWindowPopUp_Close();
    }

    public void ChangeMapToInsideOfHouse()
    {
        if (player == null)
        {
            player = PlayerMoveAndAnime.Instance.gameObject;
        }
        __insideOfHouse.SetActive(true);
        __outsideOfHouse.SetActive(false);
        TextWindowPopUp_Close();
    }

    public virtual void TextWindowPopUp_Open()
    {
        __textWindowView.SetActive(true);
        __interfaceView.SetActive(false);
    }

    public virtual void TextWindowPopUp_Close()
    {
        __textWindowView.SetActive(false);
        __interfaceView.SetActive(true);
    }


}
