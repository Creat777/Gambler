using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using UnityEngine.UI;
using System;
using PublicSet;


public class CallbackManager : Singleton<CallbackManager>
{
    
    // ��ũ��Ʈ�� ����
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

        // ��ȭâ ���� �Ͻ�����
        GameManager.connector_InGame.textWindowView.SetActive(false);
        GameManager.Instance.Pause_theGame();

        // ���� ȭ�鰡���� Ȱ��ȭ
        GameManager.connector.blackView.SetActive(true);

        // ȭ���� �˰� ���ߴٰ� �ٽ� ���󺹱͵�
        if (blackViewImage == null)
        {
            blackViewImage = GameManager.connector.blackView.GetComponent<Image>();
        }

        // ������ ����
        Sequence sequence = DOTween.Sequence();

        // ������ ����
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
            // ��Ȱ��ȭ�� �ؾ� ȭ�� Ŭ���� ������
            GameManager.connector.blackView.SetActive(false);

            // ���� ����
            GameManager.Instance.Continue_theGame();

            isBlakcViewReady = true;
        }
        );

        if (endCallback != null)
        {
            sequence.AppendCallback(() => endCallback());
        }


        sequence.SetLoops(1);

        // ������ �÷���(������ �÷��̴� ������ ���� 1ȸ�� �÷��� ������)
        sequence.Play();
    }

    public void TrashFuc()
    {
        Debug.LogAssertion("���ǵ��� ���� �ݹ��Լ�");
    }

    // csv���� �ε��������� �Լ��� ������ ���ֵ��� ����
    public UnityAction CallBackList_DefaultText(int index)
    {
        // csv���������� �����Ӱ� �ݹ��Լ��� �� �� ����
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

        // ī���� ���Ӻ䰡 �ƴ� ��쿡�� �������̽��� Ȱ��ȭ
        if(GameManager.Instance.isCasinoGameView == false)
        {
            GameManager.connector_InGame.interfaceView.SetActive(true);
        }
        
    }

    // 2
    public void ChangeMapToOutsideOfHouse()
    {
        float delay = 2.0f;
        // �ϸ� �߿� ����� ó���� �����Լ��� ����
        PlaySequnce_BlackViewProcess(delay, 
            () => GameManager.connector_InGame.map_Script.ChangeMapTo(eMap.OutsideOfHouse), 
            () => GameManager.connector_InGame.interfaceView.SetActive(true)
        );
        
    }

    // 3
    public void ChangeMapToInsideOfHouse()
    {
        float delay = 2.0f;
        // �ϸ� �߿� ����� ó���� �����Լ��� ����
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
        // falseŰ�� "Interactable_Box_Empty"����Ǿ�����
        GameManager.connector_InGame.box_Script.EmptyOutBox();

        // �ڽ��� ����ִ� �����۵�
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
        Debug.Log("�߰� �ʿ�");
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
        // �ϸ� �߿� ����� ó���� �����Լ��� ����
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
        //Debug.Log("ī���� ����");
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
    /// �ݹ��Լ��� ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <param name="index">������ �ݹ��Լ��� ������</param>
    /// <returns> ��ư�� ������ �ݹ��Լ� </returns>
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
            Debug.Log($"��⸦ {itemInfo.value_Use.ToString()}��ŭ ȸ���߽��ϴ�.");
        }
        else
        {
            Debug.LogAssertion("InventoryPopUp�� ����");
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
