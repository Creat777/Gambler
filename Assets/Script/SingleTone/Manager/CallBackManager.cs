using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections;
using System;
using PublicSet;


public class CallbackManager : Singleton<CallbackManager>
{
    
    // 스크립트로 편집
    Image blackViewImage;
    private bool isBlakcViewReady;

    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        isBlakcViewReady = true;
    }

    IEnumerator BlackViewProcess(float delay, Action callBack)
    {
        isBlakcViewReady = false;

        // 대화창 끄고 일시정지
        GameManager.Connector.textWindowView.SetActive(false);
        GameManager.Instance.Pause_theGame();

        // 먼저 화면가림막 활성화
        GameManager.Connector.blackView.SetActive(true);

        // 화면이 검게 변했다가 다시 원상복귀됨
        if (blackViewImage == null)
        {
            blackViewImage = GameManager.Connector.blackView.GetComponent<Image>();
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
        GameManager.Connector.blackView.SetActive(false);

        // 인터페이스 활성화 및 게임 지속
        GameManager.Connector.interfaceView.SetActive(true);
        GameManager.Instance.Continue_theGame();

        isBlakcViewReady = true;
    }

    // csv에서 인덱스만으로 함수를 선택할 수있도록 만듬
    public UnityAction CallBackList_Selection(int index)
    {
        switch (index)
        {
            case 0: return TextWindowPopUp_Open;
            case 1: return TextWindowPopUp_Close;
            case 2: return ChangeMapToOutsideOfHouse;
            case 3: return ChangeMapToInsideOfHouse;
            case 4: return GoToNextDay;
            case 5: return BoxOpen;
            case 6: return TextHoldOn;
        }
        
        return TrashFuc;
    }
    public void TrashFuc()
    {
        Debug.LogAssertion("정의되지 않은 콜백함수");
    }

    // 0
    public virtual void TextWindowPopUp_Open()
    {
        GameManager.Connector.textWindowView.SetActive(true);
        GameManager.Connector.interfaceView.SetActive(false);
    }

    // 1
    public virtual void TextWindowPopUp_Close()
    {
        GameManager.Connector.textWindowView.SetActive(false);
        GameManager.Connector.interfaceView.SetActive(true);
    }

    // 2
    public void ChangeMapToOutsideOfHouse()
    {
        float delay = 2.0f;
        // 암막 중에 실행될 처리를 람다함수로 전달
        StartCoroutine(BlackViewProcess(delay, 
            () =>
        {
            // 맵 변경
            GameManager.Connector.insideOfHouse.SetActive(false);
            GameManager.Connector.outsideOfHouse.SetActive(true);

            // 플레이어를 변경된 맵의 문 앞으로 이동
            GameManager.Connector.player.transform.position = Vector2.zero;
        }
        ));
        
    }

    // 3
    public void ChangeMapToInsideOfHouse()
    {
        float delay = 2.0f;
        // 암막 중에 실행될 처리를 람다함수로 전달
        StartCoroutine(BlackViewProcess(delay,
            () =>
            {

                // 맵 변경
                GameManager.Connector.insideOfHouse.SetActive(true);
                GameManager.Connector.outsideOfHouse.SetActive(false);

                // 플레이어를 변경된 맵의 문 앞으로 이동
                GameManager.Connector.player.transform.position = new Vector2(0, 2);
            }
        ));
        
    }

    // 4
    public void GoToNextDay()
    {
        float delay = 4.0f;
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
    public virtual void BoxOpen()
    {
        if(PlayerPrefsManager.Instance != null)
        {
            // false키에 "Interactable_Box_Empty"저장되어있음
            GameManager.Connector.box_Script.EmptyOutBox();

            int newItemIndex = PlayerPrefsManager.Instance.GetNewLastId();

            // 값이 변경될 수 있어야함
            PlayerPrefsManager.Instance.PlayerGetItem(newItemIndex, (int)eItemSerialNumber.TutorialQuest);

            TextWindowPopUp_Close();

            GameManager.Connector.iconView.GetComponent<IconView>().IconUnLock(Icon.Inventory);
        }
        
    }

    //6
    public virtual void TextHoldOn()
    {
        TextWindowView textWindowView_Script = GameManager.Connector.textWindowView.GetComponent<TextWindowView>();
        textWindowView_Script.PrintText();
        textWindowView_Script.selectionView.SetActive(false);
        return;
    }














    /// <summary>
    /// 콜백함수를 반환하는 함수
    /// </summary>
    /// <param name="index">값으로 콜백함수를 선택함</param>
    /// <returns> 버튼에 연결할 콜백함수 </returns>
    public UnityAction CallBackList_Item_Quest(int index)
    {
        switch (index)
        {
            case 0: return TutorialStart;
        }

        return TrashFuc;
    }

    // 0
    public void TutorialStart()
    {
        Debug.Log("튜토리얼 시작");

        

        IconView iconView_Script =  GameManager.Connector.iconView.GetComponent<IconView>();
        iconView_Script.IconUnLock(Icon.Quest);
    }
}
