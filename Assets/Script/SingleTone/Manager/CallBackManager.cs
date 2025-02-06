using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;


public class CallBackManager : Singleton<CallBackManager>
{
    // 이니셜라이저로 연결
    [SerializeField] private GameObject insideOfHouse;
    [SerializeField] private GameObject outsideOfHouse;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject interfaceView;
    [SerializeField] private GameObject textWindowView;
    [SerializeField] private GameObject BalckView;
    [SerializeField] private GameObject iconView;

    public GameObject __insideOfHouse { get { return insideOfHouse; } set { insideOfHouse = value; } }
    public GameObject __outsideOfHouse { get { return outsideOfHouse; } set { outsideOfHouse = value; } }
    public GameObject __player { get { return player; } set { player = value; } }
    public GameObject __interfaceView { get { return interfaceView; } set { interfaceView = value; } }
    public GameObject __textWindowView { get { return textWindowView; } set { textWindowView = value; } }
    public GameObject __BalckView { get {return BalckView; } set { BalckView = value; } }
    public GameObject __iconView { get { return iconView; } set { iconView = value; } }

    // 스크립트로 편집
    Image blackViewImage;
    Dictionary<bool, string> BoxNameDict;
    private bool isBlakcViewReady;

    protected override void Awake()
    {
        base.Awake();
        BoxNameDict = new Dictionary<bool, string>();
        BoxNameDict.Add(true, "Interactable_Box_Full");
        BoxNameDict.Add(false, "Interactable_Box_Empty");
    }

    void Start()
    {
        isBlakcViewReady = true;
    }
    private void FixedUpdate()
    {
        
    }

    IEnumerator BlackViewProcess(float delay, Action callBack)
    {
        isBlakcViewReady = false;

        // 대화창 끄고 일시정지
        __textWindowView.SetActive(false);
        GameManager.Instance.Pause_theGame();

        // 먼저 화면가림막 활성화
        __BalckView.SetActive(true);

        // 화면이 검게 변했다가 다시 원상복귀됨
        if (blackViewImage == null)
        {
            blackViewImage = __BalckView.GetComponent<Image>();
        }

        // 시퀀스 생성
        Sequence sequence = DOTween.Sequence();

        // 시퀀스 설정
        sequence
            .AppendCallback(() => blackViewImage.color = Color.clear)
            .Append(blackViewImage.DOColor(Color.black, delay / 2))
            .AppendCallback(() => callBack())
            .Append(blackViewImage.DOColor(Color.clear, delay / 2))
            .SetLoops(1);

        // 시퀀스 플레이(시퀀스 플레이는 생성후 최초 1회만 플레이 가능함)
        sequence.Play();

        // 화면이 복귀 되는 시간만큼 딜레이
        yield return new WaitForSeconds(delay);

        // 비활성화를 해야 화면 클릭이 가능함
        __BalckView.SetActive(false);

        // 인터페이스 활성화 및 게임 지속
        __interfaceView.SetActive(true);
        GameManager.Instance.Continue_theGame();

        isBlakcViewReady = true;
    }

    // csv에서 인덱스만으로 함수를 선택할 수있도록 만듬
    public UnityAction CallBackList(int index, GameObject obj)
    {
        UnityAction unityAction = () =>
        {
            switch (index)
            {
                case 0 : TextWindowPopUp_Open(); break;
                case 1 : TextWindowPopUp_Close(); break;
                case 2 : ChangeMapToOutsideOfHouse(2.0f); break;
                case 3 : ChangeMapToInsideOfHouse(2.0f); break;
                case 4 : GoToNextDay(4.0f); break;
                case 5 : BoxOpen(obj); break;

            }
        };

        return unityAction;
    }

    // 0
    public virtual void TextWindowPopUp_Open()
    {
        __textWindowView.SetActive(true);
        __interfaceView.SetActive(false);
    }

    // 1
    public virtual void TextWindowPopUp_Close()
    {
        __textWindowView.SetActive(false);
        __interfaceView.SetActive(true);
    }

    // 2
    public void ChangeMapToOutsideOfHouse(float delay)
    {
        // 암막 중에 실행될 처리를 람다함수로 전달
        StartCoroutine(BlackViewProcess(delay, 
            () =>
        {
            if (player == null)
            {
                player = PlayerMoveAndAnime.Instance.gameObject;
            }
            // 맵 변경
            __insideOfHouse.SetActive(false);
            __outsideOfHouse.SetActive(true);

            // 플레이어를 변경된 맵의 문 앞으로 이동
            player.transform.position = Vector2.zero;
        }
        ));
        
    }

    // 3
    public void ChangeMapToInsideOfHouse(float delay)
    {
        // 암막 중에 실행될 처리를 람다함수로 전달
        StartCoroutine(BlackViewProcess(delay,
            () =>
            {
                if (player == null)
                {
                    player = PlayerMoveAndAnime.Instance.gameObject;
                }
                // 맵 변경
                __insideOfHouse.SetActive(true);
                __outsideOfHouse.SetActive(false);

                // 플레이어를 변경된 맵의 문 앞으로 이동
                player.transform.position = new Vector2(0, 2);
            }
        ));
        
    }

    // 4
    public void GoToNextDay(float delay)
    {
        // 암막 중에 실행될 처리를 람다함수로 전달
        StartCoroutine(BlackViewProcess(delay,
            () =>
            {
                // 날짜 이동
                GameManager.Instance.__day++;
            }
        ));

        
    }

    

    // 5
    public virtual void BoxOpen(GameObject Box)
    {
        if(PlayerPrefsManager.Instance != null)
        {
            // false키에 "Interactable_Box_Empty"저장되어있음
            Box.name = BoxNameDict[false];

            int newItemIndex = PlayerPrefsManager.Instance.GetNewLastId();
            PlayerPrefsManager.Instance.PlayerGetItem(newItemIndex, ItemManager.Instance.QuestItemSerialNumber);

            TextWindowPopUp_Close();

            iconView.GetComponent<IconView>().IconUnLock(IconView.Icon.Inventory);
        }
        
    }

    


    //public void

}
