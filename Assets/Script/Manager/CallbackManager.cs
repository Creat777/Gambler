using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using UnityEngine.UI;
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

    public void PlaySequnce_BlackViewProcess(float delay, Action middleCallBack, Action endCallback = null)
    {
        isBlakcViewReady = false;

        // 대화창 끄고 일시정지
        GameManager.connector_InGame.textWindowView.SetActive(false);
        GameManager.Instance.Pause_theGame();

        // 먼저 화면가림막 활성화
        GameManager.connector.blackView.SetActive(true);

        // 화면이 검게 변했다가 다시 원상복귀됨
        if (blackViewImage == null)
        {
            blackViewImage = GameManager.connector.blackView.GetComponent<Image>();
        }

        // 시퀀스 생성
        Sequence sequence = DOTween.Sequence();

        // 시퀀스 설정
        sequence.AppendCallback(() => blackViewImage.color = Color.clear);
        sequence.Append(blackViewImage.DOColor(Color.black, delay / 2));

        if(middleCallBack != null)
        {
            sequence.AppendCallback(() => middleCallBack());
        }

        sequence.Append(blackViewImage.DOColor(Color.clear, delay / 2));

        sequence.AppendCallback(
        () =>
        {
            // 비활성화를 해야 화면 클릭이 가능함
            GameManager.connector.blackView.SetActive(false);

            // 게임 지속
            GameManager.Instance.Continue_theGame();

            isBlakcViewReady = true;
        }
        );

        if (endCallback != null)
        {
            sequence.AppendCallback(() => endCallback());
        }


        sequence.SetLoops(1);

        // 시퀀스 플레이(시퀀스 플레이는 생성후 최초 1회만 플레이 가능함)
        sequence.Play();
    }

    public void TrashFuc()
    {
        Debug.LogAssertion("정의되지 않은 콜백함수");
    }

    // csv에서 인덱스만으로 함수를 선택할 수있도록 만듬
    public UnityAction CallBackList_DefaultText(int index)
    {
        // csv선택지에서 자유롭게 콜백함수를 고를 수 있음
        switch (index)
        {
            case 0: return TextWindowPopUp_Open;
            case 1: return TextWindowPopUp_Close;
            case 2: return ChangeMapToOutsideOfHouse;
            case 3: return ChangeMapToInsideOfHouse;
            case 4: return GoToNextDay;
            case 5: return BoxOpen;
            case 6: return TextHoldOn;
            case 7: return SavePlayerData;
            case 8: return StartComputer;
            case 9: return GetGamblingCoin;
            case 10: return GotoCasinoPlace;
            case 11: return GotoUnknownIsland;
            case 12: return TellmeOneMoreTime;
            case 13: return EnterCasino;
            case 14: return TutorialStart;
        }

        return TrashFuc;
    }
    

    // 0
    public virtual void TextWindowPopUp_Open()
    {
        GameManager.connector_InGame.textWindowView.SetActive(true);
        GameManager.connector_InGame.interfaceView.SetActive(false);
    }

    // 1
    public virtual void TextWindowPopUp_Close()
    {
        GameManager.connector_InGame.textWindowView.SetActive(false);

        // 카지노 게임뷰가 아닌 경우에만 인터페이스를 활성화
        if(GameManager.Instance.isCasinoGameView == false)
        {
            GameManager.connector_InGame.interfaceView.SetActive(true);
        }
        
    }

    // 2
    public void ChangeMapToOutsideOfHouse()
    {
        float delay = 2.0f;
        // 암막 중에 실행될 처리를 람다함수로 전달
        PlaySequnce_BlackViewProcess(delay, 
            () => GameManager.connector_InGame.map_Script.ChangeMapTo(eMap.OutsideOfHouse), 
            () => GameManager.connector_InGame.interfaceView.SetActive(true)
        );
        
    }

    // 3
    public void ChangeMapToInsideOfHouse()
    {
        float delay = 2.0f;
        // 암막 중에 실행될 처리를 람다함수로 전달
        PlaySequnce_BlackViewProcess(delay,
            () =>GameManager.connector_InGame.map_Script.ChangeMapTo(eMap.InsideOfHouse),
            () =>GameManager.connector_InGame.interfaceView.SetActive(true)
        );
        
    }

    // 4
    public void GoToNextDay()
    {
        float delay = 4.0f;

        PlaySequnce_BlackViewProcess(delay,
                () =>
                {
                    GameManager.Instance.CountDownRemainingPeriod();
                },
                ()=>
                {
                    PlayManager.Instance.StartPlayerMonologue_OnPlayerWakeUp();
                }
            );

    }

    

    // 5
    public virtual void BoxOpen()
    {
        // false키에 "Interactable_Box_Empty"저장되어있음
        GameManager.connector_InGame.box_Script.EmptyOutBox();

        // 박스에 들어있는 아이템들
        ItemManager.Instance.PlayerGetItem(eItemType.Notice_Stage1);
        ItemManager.Instance.PlayerGetItem(eItemType.TutorialQuest);

        TextWindowPopUp_Close();

        GameManager.connector_InGame.iconView_Script.GetComponent<IconView>().TryIconUnLock(eIcon.Inventory);
    }

    //6
    public virtual void TextHoldOn()
    {
        TextWindowView textWindowView_Script = GameManager.connector_InGame.textWindowView.GetComponent<TextWindowView>();
        textWindowView_Script.PrintText();
        return;
    }

    // 7
    public virtual void SavePlayerData()
    {
        TextWindowPopUp_Close();
        GameManager.connector_InGame.popUpView_Script.SaveDataPopUpOpen();
    }

    // 8
    public virtual void StartComputer()
    {
        Debug.Log("추가 필요");
    }

    // 9
    public virtual void GetGamblingCoin()
    {
        PlayManager.Instance.AddPlayerMoney(100);
        GameManager.connector_InGame.box_Script.FillUpBox();
        TextHoldOn();
    }

    // 10
    public virtual void GotoCasinoPlace()
    {
        float delay = 2.0f;
        // 암막 중에 실행될 처리를 람다함수로 전달
        PlaySequnce_BlackViewProcess(delay,
            () =>
            {
                GameManager.connector_InGame.map_Script.ChangeMapTo(eMap.Casino);
                GameManager.connector_InGame.interfaceView.SetActive(true);
            }
        );
    }

    // 11
    public virtual void GotoUnknownIsland()
    {

    }

    // 12
    public virtual void TellmeOneMoreTime()
    {
        GameManager.connector_InGame.textWindowView.GetComponent<TextWindowView>().TextIndexInit(0);
        TextHoldOn();
    }

    // 13
    public virtual void EnterCasino()
    {
        //Debug.Log("카지노 입장");
        float delay = 2.0f;
        PlaySequnce_BlackViewProcess(delay,
            ()=>
            {
                GameManager.connector_InGame.MainCanvas_script.CasinoViewOpen();
            },
            ()=>  
            {
                GameManager.connector_InGame.MainCanvas_script.CasinoView.GetComponent<CasinoView>().StartDealerDialogue();
            }
            );
    }

    // 14
    public void TutorialStart()
    {
        GameManager.Instance.PlaySequnce_StageAnimation();
        GameManager.connector_InGame.iconView_Script.TryIconUnLock(eIcon.Quest);
    }

    // 15





    public UnityAction CallbackList_OnlyOneLivesText(int index)
    {
        switch (index)
        {
            case 0: return NextProgress;
            case 1: return GameStartButtonOn;
            case 10: return AttackPrgress;
            case 11: return DeffenceProgress;
            case 20: return CardOpen;
            case 21: return OnJokerWin;
            case 22: return OnAttackerWin;
            case 23: return OnDeffenderWin;
            case 24: return OnHuntingTime;
            case 25: return OnPlayerBackrupt;
            case 30: return NextGame;

            default: return TrashFuc;
        }
    }

    public void NextProgress()
    {
        CardGamePlayManager.Instance.NextProgress();
    }

    public void GameStartButtonOn()
    {
        CardGamePlayManager.Instance.cardGameView.PlaySequnce_StartButtonFadeIn();
    }

    public void AttackPrgress()
    {
        CardGamePlayManager.Instance.StartPlayerAttack();
    }

    public void DeffenceProgress()
    {
        if(CardGamePlayManager.Instance.Deffender.closedCardList.Count <= 0)
        {
            CardGamePlayManager.Instance.NextProgress();
            return;
        }
        else
        {
            CardGamePlayManager.Instance.StartPlayerDeffence();
            return;
        }
        
    }

    public void CardOpen()
    {
        CardGamePlayManager.Instance.CardOpenAtTheSameTime();
    }

    public void OnJokerWin()
    {
        CardGamePlayManager.Instance.OnJokerAppear();
    }
    public void OnAttackerWin()
    {
        CardGamePlayManager.Instance.OnAttackSuccess();
    }
    
    public void OnDeffenderWin()
    {
        CardGamePlayManager.Instance.OnDefenceSuccess();
    }

    public void OnHuntingTime()
    {
        CardGamePlayManager.Instance.OnHuntPrey();
    }

    public void OnPlayerBackrupt()
    {
        CardGamePlayManager.Instance.OnPlayerBankrupt();
    }
    
    private void NextGame()
    {
        CardGamePlayManager.Instance.InitCurrentGame();
        CardGamePlayManager.Instance.cardGameView.PlaySequnce_StartButtonFadeIn();
    }




    /// <summary>
    /// 콜백함수를 반환하는 함수
    /// </summary>
    /// <param name="index">값으로 콜백함수를 선택함</param>
    /// <returns> 버튼에 연결할 콜백함수 </returns>
    public UnityAction CallBackList_Item(eItemCallback index)
    {
        switch (index)
        {
            case eItemCallback.FirstQuest : return CasinoOpen;
            case eItemCallback.EatMeal: return EatMeal;

            default: return TrashFuc;
        }
    }

    
    public void CasinoOpen()
    {
        GameManager.Instance.NextStage();
        GameManager.Instance.PlaySequnce_StageAnimation();
    }

    
    public void EatMeal()
    {
        InventoryPopUp inven = GameManager.connector_InGame.
                                popUpView_Script.inventoryPopUp.GetComponent<InventoryPopUp>();
        if (inven != null)
        {
            cItemInfo itemInfo = CsvManager.Instance.GetItemInfo(inven.currentClickItem.type);
            Debug.Log($"허기를 {itemInfo.value_Use.ToString()}만큼 회복했습니다.");
        }
        else
        {
            Debug.LogAssertion("InventoryPopUp이 없음");
        }
    }

    public UnityAction CallBackList_Quest(int index)
    {
        switch (index)
        {
            case 1: return TutorialCompleteCheck;
            default: return TrashFuc;
        }
    }

    public void TutorialCompleteCheck()
    {

    }
}
