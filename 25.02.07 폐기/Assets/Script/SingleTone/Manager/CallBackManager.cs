using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;


public class CallBackManager : Singleton<CallBackManager>
{
    // �����Ϳ��� ����
    [SerializeField] private GameObject BalckView;
    public GameObject __BalckView { get { return BalckView; } set { BalckView = value; } }


    // ��ũ��Ʈ�� ����
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

    public IEnumerator BlackViewProcess(float delay, Action callBack)
    {
        isBlakcViewReady = false;

        // ��ȭâ ���� �Ͻ�����
        if(GameManager.Instance.connector.textWindowView != null)
        {
            GameManager.Instance.connector.textWindowView.SetActive(false);
        }
        GameManager.Instance.Pause_theGame();

        // ���� ȭ�鰡���� Ȱ��ȭ
        __BalckView.SetActive(true);

        // ȭ���� �˰� ���ߴٰ� �ٽ� ���󺹱͵�
        if (blackViewImage == null)
        {
            blackViewImage = __BalckView.GetComponent<Image>();
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
        __BalckView.SetActive(false);

        // �������̽� Ȱ��ȭ �� ���� ����
        if(GameManager.Instance.connector.interfaceView != null)
        {
            GameManager.Instance.connector.interfaceView.SetActive(true);
        }
        GameManager.Instance.Continue_theGame();

        isBlakcViewReady = true;
    }

    // csv���� �ε��������� �Լ��� ������ ���ֵ��� ����
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
        GameManager.Instance.connector.textWindowView.SetActive(true);
        GameManager.Instance.connector.interfaceView.SetActive(false);
        GameManager.Instance.connector.textWindowView.GetComponent<TextWindowView>().StartTextWindow(TextWindowView.TextType.Interaction);
    }

    // 1
    public virtual void TextWindowPopUp_Close()
    {
        GameManager.Instance.connector.textWindowView.SetActive(false);
        GameManager.Instance.connector.interfaceView.SetActive(true);
    }

    // 2
    public void ChangeMapToOutsideOfHouse(float delay)
    {
        // �ϸ� �߿� ����� ó���� �����Լ��� ����
        StartCoroutine(BlackViewProcess(delay, 
            () =>
        {
            if (GameManager.Instance.connector.map_InGame == null)
            {
                GameManager.Instance.connector.map_InGame = GameObject.Find("map_InGame");
            }
            GameManager.Instance.connector.map_InGame.GetComponent<Map_InGame>().ChangeMapToOutSide();
        }
        ));
        
    }

    // 3
    public void ChangeMapToInsideOfHouse(float delay)
    {
        // �ϸ� �߿� ����� ó���� �����Լ��� ����
        StartCoroutine(BlackViewProcess(delay,
            () =>
            {
                if (GameManager.Instance.connector.map_InGame == null)
                {
                    GameManager.Instance.connector.map_InGame = GameObject.Find("map_InGame");
                }
                GameManager.Instance.connector.map_InGame.GetComponent<Map_InGame>().ChangeMapToInSide();
            }
        ));
        
    }

    // 4
    public void GoToNextDay(float delay)
    {
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
    public virtual void BoxOpen(GameObject Box)
    {
        if(PlayerPrefsManager.Instance != null)
        {
            // falseŰ�� "Interactable_Box_Empty"����Ǿ�����
            Box.name = BoxNameDict[false];

            int newItemIndex = PlayerPrefsManager.Instance.GetNewLastId();
            PlayerPrefsManager.Instance.PlayerGetItem(newItemIndex, ItemManager.Instance.QuestItemSerialNumber);

            TextWindowPopUp_Close();

            GameManager.Instance.connector.iconView.GetComponent<IconView>().IconUnLock(IconView.Icon.Inventory);
        }
        
    }

    


    //public void

}
