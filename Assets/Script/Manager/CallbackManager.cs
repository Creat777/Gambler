using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections;
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

    public void BlackViewProcess(float delay, Action middleCallBack, Action endCallback = null)
    {
        isBlakcViewReady = false;

        // ��ȭâ ���� �Ͻ�����
        GameManager.Connector.textWindowView.SetActive(false);
        GameManager.Instance.Pause_theGame();

        // ���� ȭ�鰡���� Ȱ��ȭ
        GameManager.Connector.blackView.SetActive(true);

        // ȭ���� �˰� ���ߴٰ� �ٽ� ���󺹱͵�
        if (blackViewImage == null)
        {
            blackViewImage = GameManager.Connector.blackView.GetComponent<Image>();
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

        if (endCallback != null)
        {
            sequence.AppendCallback(() => endCallback());
        }

        sequence.AppendCallback(
            () =>
            {
                // ��Ȱ��ȭ�� �ؾ� ȭ�� Ŭ���� ������
                GameManager.Connector.blackView.SetActive(false);

                // ���� ����
                GameManager.Instance.Continue_theGame();

                isBlakcViewReady = true;
            }
            );

        sequence.SetLoops(1);

        // ������ �÷���(������ �÷��̴� ������ ���� 1ȸ�� �÷��� ������)
        sequence.Play();
    }

    public void TrashFuc()
    {
        Debug.LogAssertion("���ǵ��� ���� �ݹ��Լ�");
    }

    // csv���� �ε��������� �Լ��� ������ ���ֵ��� ����
    public UnityAction CallBackList_Selection(int index)
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
        }

        return TrashFuc;
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

        // ī���� ���Ӻ䰡 �ƴ� ��쿡�� �������̽��� Ȱ��ȭ
        if(GameManager.Instance.isCasinoGameView == false)
        {
            GameManager.Connector.interfaceView.SetActive(true);
        }
        
    }

    // 2
    public void ChangeMapToOutsideOfHouse()
    {
        float delay = 2.0f;
        // �ϸ� �߿� ����� ó���� �����Լ��� ����
        BlackViewProcess(delay, 
            () => GameManager.Connector.map_Script.ChangeMapTo(eMap.OutsideOfHouse), 
            () => GameManager.Connector.interfaceView.SetActive(true)
        );
        
    }

    // 3
    public void ChangeMapToInsideOfHouse()
    {
        float delay = 2.0f;
        // �ϸ� �߿� ����� ó���� �����Լ��� ����
        BlackViewProcess(delay,
            () =>GameManager.Connector.map_Script.ChangeMapTo(eMap.InsideOfHouse),
            () =>GameManager.Connector.interfaceView.SetActive(true)
        );
        
    }

    // 4
    public void GoToNextDay()
    {
        float delay = 4.0f;

        BlackViewProcess(delay,
                () =>
                {
                    GameManager.Instance.Day++;
                },
                ()=>
                {
                    GameManager.Instance.StartPlayerMonologue();
                }
            );

    }

    

    // 5
    public virtual void BoxOpen()
    {
        if(PlayerPrefsManager.Instance != null)
        {
            // falseŰ�� "Interactable_Box_Empty"����Ǿ�����
            GameManager.Connector.box_Script.EmptyOutBox();

            // �ڽ��� ����ִ� �����۵�
            PlayerPrefsManager.Instance.PlayerGetItem(eItemSerialNumber.Notice_Stage1);
            PlayerPrefsManager.Instance.PlayerGetItem(eItemSerialNumber.TutorialQuest);
            PlayerPrefsManager.Instance.PlayerGetItem(eItemSerialNumber.Meat);
            PlayerPrefsManager.Instance.PlayerGetItem(eItemSerialNumber.Fish);
            PlayerPrefsManager.Instance.PlayerGetItem(eItemSerialNumber.Egg);

            TextWindowPopUp_Close();

            GameManager.Connector.iconView_Script.GetComponent<IconView>().IconUnLock(eIcon.Inventory);
        }
        
    }

    //6
    public virtual void TextHoldOn()
    {
        TextWindowView textWindowView_Script = GameManager.Connector.textWindowView.GetComponent<TextWindowView>();
        textWindowView_Script.PrintText();
        return;
    }

    // 7
    public virtual void SavePlayerData()
    {
        Debug.Log("�߰� �ʿ�");
    }

    // 8
    public virtual void StartComputer()
    {
        Debug.Log("�߰� �ʿ�");
    }

    // 9
    public virtual void GetGamblingCoin()
    {
        PlayManager.Instance.PlayerMoneyPlus(100);
        GameManager.Connector.box_Script.FillUpBox();
        TextHoldOn();
    }

    // 10
    public virtual void GotoCasinoPlace()
    {
        float delay = 2.0f;
        // �ϸ� �߿� ����� ó���� �����Լ��� ����
        BlackViewProcess(delay,
            () =>
            {
                GameManager.Connector.map_Script.ChangeMapTo(eMap.Casino);
                GameManager.Connector.interfaceView.SetActive(true);
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
        GameManager.Connector.textWindowView.GetComponent<TextWindowView>().TextIndexInit(0);
        TextHoldOn();
    }

    // 13
    public virtual void EnterCasino()
    {
        //Debug.Log("ī���� ����");
        float delay = 2.0f;
        BlackViewProcess(delay,
            ()=>
            {
                GameManager.Connector.MainCanvas_script.CasinoViewOpen();
            },
            ()=>  
            {
                GameManager.Connector.MainCanvas_script.CasinoView.GetComponent<CasinoView>().StartDealerDialogue();
            }
            );
    }















    /// <summary>
    /// �ݹ��Լ��� ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <param name="index">������ �ݹ��Լ��� ������</param>
    /// <returns> ��ư�� ������ �ݹ��Լ� </returns>
    public UnityAction CallBackList_Item_Quest(eItemCallback index)
    {
        switch (index)
        {
            case eItemCallback.TutorialStart : return TutorialStart;
            case eItemCallback.EatMeal: return EatMeal;

            default: return TrashFuc;
        }
    }

    
    public void TutorialStart()
    {
        GameManager.Instance.NextStage();

        GameManager.Connector.iconView_Script.IconUnLock(eIcon.Quest);
        Debug.Log("Ʃ�丮�� ����");
    }

    
    public void EatMeal()
    {
        InventoryPopUp inven = GameManager.Connector.popUpView_Script.inventoryPopUp.GetComponent<InventoryPopUp>();
        if (inven != null)
        {
            cItemInfo itemInfo = CsvManager.Instance.GetItemInfo(inven.currentClickItem.serialNumber);
            Debug.Log($"��⸦ {itemInfo.value_Use.ToString()}��ŭ ȸ���߽��ϴ�.");
        }
        else
        {
            Debug.LogAssertion("InventoryPopUp�� ����");
        }
    }
}
