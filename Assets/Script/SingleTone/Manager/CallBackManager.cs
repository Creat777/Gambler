using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;


public class CallBackManager : Singleton<CallBackManager>
{
    // �̴ϼȶ������� ����
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
    private void FixedUpdate()
    {
        
    }

    IEnumerator BlackViewProcess(float delay, Action callBack)
    {
        isBlakcViewReady = false;

        // ��ȭâ ���� �Ͻ�����
        __textWindowView.SetActive(false);
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
        __interfaceView.SetActive(true);
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
        // �ϸ� �߿� ����� ó���� �����Լ��� ����
        StartCoroutine(BlackViewProcess(delay, 
            () =>
        {
            if (player == null)
            {
                player = PlayerMoveAndAnime.Instance.gameObject;
            }
            // �� ����
            __insideOfHouse.SetActive(false);
            __outsideOfHouse.SetActive(true);

            // �÷��̾ ����� ���� �� ������ �̵�
            player.transform.position = Vector2.zero;
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
                if (player == null)
                {
                    player = PlayerMoveAndAnime.Instance.gameObject;
                }
                // �� ����
                __insideOfHouse.SetActive(true);
                __outsideOfHouse.SetActive(false);

                // �÷��̾ ����� ���� �� ������ �̵�
                player.transform.position = new Vector2(0, 2);
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

            iconView.GetComponent<IconView>().IconUnLock(IconView.Icon.Inventory);
        }
        
    }

    


    //public void

}
