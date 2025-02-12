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

    IEnumerator BlackViewProcess(float delay, Action callBack)
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
        sequence
            .AppendCallback(() => blackViewImage.color = Color.clear)
            .Append(blackViewImage.DOColor(Color.black, delay / 2))
            .AppendCallback(() => callBack())
            .Append(blackViewImage.DOColor(Color.clear, delay / 2))
            .SetLoops(1);

        // ������ �÷���(������ �÷��̴� ������ ���� 1ȸ�� �÷��� ������)
        sequence.Play();

        // ȭ���� ���� �Ǵ� �ð���ŭ ������
        yield return new WaitForSeconds(delay);

        // ��Ȱ��ȭ�� �ؾ� ȭ�� Ŭ���� ������
        GameManager.Connector.blackView.SetActive(false);

        // �������̽� Ȱ��ȭ �� ���� ����
        GameManager.Connector.interfaceView.SetActive(true);
        GameManager.Instance.Continue_theGame();

        isBlakcViewReady = true;
    }

    // csv���� �ε��������� �Լ��� ������ ���ֵ��� ����
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
        Debug.LogAssertion("���ǵ��� ���� �ݹ��Լ�");
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
        // �ϸ� �߿� ����� ó���� �����Լ��� ����
        StartCoroutine(BlackViewProcess(delay, 
            () =>
        {
            // �� ����
            GameManager.Connector.insideOfHouse.SetActive(false);
            GameManager.Connector.outsideOfHouse.SetActive(true);

            // �÷��̾ ����� ���� �� ������ �̵�
            GameManager.Connector.player.transform.position = Vector2.zero;
        }
        ));
        
    }

    // 3
    public void ChangeMapToInsideOfHouse()
    {
        float delay = 2.0f;
        // �ϸ� �߿� ����� ó���� �����Լ��� ����
        StartCoroutine(BlackViewProcess(delay,
            () =>
            {

                // �� ����
                GameManager.Connector.insideOfHouse.SetActive(true);
                GameManager.Connector.outsideOfHouse.SetActive(false);

                // �÷��̾ ����� ���� �� ������ �̵�
                GameManager.Connector.player.transform.position = new Vector2(0, 2);
            }
        ));
        
    }

    // 4
    public void GoToNextDay()
    {
        float delay = 4.0f;
        // �ϸ� �߿� ����� ó���� �����Լ��� ����
        StartCoroutine(BlackViewProcess(delay,
            () =>
            {
                // ��¥ �̵�
                GameManager.Instance.__day++;
            }
        ));

        
    }

    

    // 5
    public virtual void BoxOpen()
    {
        if(PlayerPrefsManager.Instance != null)
        {
            // falseŰ�� "Interactable_Box_Empty"����Ǿ�����
            GameManager.Connector.box_Script.EmptyOutBox();

            int newItemIndex = PlayerPrefsManager.Instance.GetNewLastId();

            // ���� ����� �� �־����
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
    /// �ݹ��Լ��� ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <param name="index">������ �ݹ��Լ��� ������</param>
    /// <returns> ��ư�� ������ �ݹ��Լ� </returns>
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
        Debug.Log("Ʃ�丮�� ����");

        

        IconView iconView_Script =  GameManager.Connector.iconView.GetComponent<IconView>();
        iconView_Script.IconUnLock(Icon.Quest);
    }
}
