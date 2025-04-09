using DG.Tweening;
using PublicSet;
using System;
using System.Collections;
using UnityEngine;

public class DeckOfCards : MonoBehaviour
{
    // ������ ����
    public CardGamePlayManager cardGamePlayManager;
    public Vector3 cardInterval;

    // ������ ����
    public GameObject floor;
    public GameObject[] Players;

    // ��ũ��Ʈ ����
    public GameObject[] Cards {  get; private set; }
    private Vector3 floorPos;
    
    private int PlayerCount;


    private void Awake()
    {
        floorPos = floor.transform.position;
    }

    private void Start()
    {
        if (cardGamePlayManager == null)
            Debug.LogAssertion($"cardGamePlayManager == null ");

        // ī�� �ʱ�ȭ
        InitCards();

        // ó���� ��ġ ����
        SetCardPositions();
    }

    private void InitCards()
    {
        Cards = new GameObject[transform.childCount];

        Vector3 cardSize = Vector3.zero;
        for (int i = 0; i < transform.childCount; i++)
        {
            Cards[i] = transform.GetChild(i).gameObject;
        }
        ProcessCsvOfCard(Cards);
    }

    private void ProcessCsvOfCard(GameObject[] Cards)
    {
        Debug.Log("ProcessCsvOfCard ���� ����");

        //string path = "CSV/TrumpCardInfo/CardsInfo";

        string path = System.IO.Path.Combine("CSV","TrumpCardInfo","CardsInfo");

        if (Application.platform == RuntimePlatform.Android)
        {
            path = System.IO.Path.Combine(Application.streamingAssetsPath, path);
        }

        CsvManager.Instance.LoadCsv<cTrumpCardInfo>(path,
            (row, trumpCardInfo) => // trumpCard�� ������Ʈ�� ���� ��ũ��Ʈ
            {
                // ��������� �Ҵ���� ���� ���
                if (trumpCardInfo == null)
                {
                    Debug.LogAssertion("trumpCardInfo == null");
                    return;
                }

                    int field_num = 0;


                // �� ���� ó�� ����
                foreach (string field in row)
                {
                    switch (field_num)
                    {
                        case 0: 
                            if (int.TryParse(field, out int intField))
                            {
                                trumpCardInfo.cardIndex = intField; break;
                            }
                            else
                            {
                                Debug.LogAssertion($"Ÿ�Կ��� -> {field}");
                            }
                            break;

                        case 1: trumpCardInfo.cardName = field;
                            break;

                        case 2:
                            if (eCardType.TryParse(field, out eCardType eCardTypeField))
                            {
                                trumpCardInfo.cardType = eCardTypeField; break;
                            }
                            else
                            {
                                Debug.LogAssertion($"Ÿ�Կ��� -> {field}");
                            }
                            break;

                        case 3:
                            if (int.TryParse(field, out int intField2))
                            {
                                trumpCardInfo.cardValue = intField2; break;
                            }
                            else
                            {
                                Debug.LogAssertion($"Ÿ�Կ��� -> {field}");
                            }
                            break;

                        default: Debug.LogAssertion($"�߸��� ������ : {field}");
                            break;
                    }
                    field_num++;
                }

                // ó���� �����͸� �� ī�忡 ����
                if(trumpCardInfo.cardIndex>=0 && trumpCardInfo.cardIndex < Cards.Length)
                {
                    Cards[trumpCardInfo.cardIndex].GetComponent<TrumpCardDefault>().SetTrumpCard(trumpCardInfo);
                }
                
            }
            );
    }

    public void SetPlayerNumber()
    {
        PlayerCount = Players.Length;
    }

    public void SetPlayerNumber(int vlaue)
    {
        PlayerCount = vlaue;
    }

    public void ReturnAllOfCards()
    {
        for (int i = 0; i < Cards.Length; i++)
        {
            // ��� ī�带 Ȱ��ȭ
            Cards[i].SetActive(true);

            Cards[i].GetComponent<TrumpCardDefault>().InitAttribute();

            // ���̾ ����Ʈ�� �ٲ���
            if (Cards[i].layer != 0)
            {
                Cards[i].layer = 0;
                // ��Ŀ ó��
                if (Cards[i].transform.childCount != 0)
                {
                    Cards[i].transform.GetChild(0).gameObject.layer = 0;
                }
            }
            // ī�带 ȸ���ϰ� ������ ����
            Cards[i].transform.SetParent(transform, false);
            Cards[i].transform.localScale = Vector3.one;
        }

    }

    private void ReturnAllOfCardFromCardBox(Transform cardBox)
    {
        // �ڽİ�ü�� �ִ� ī��ڽ���
        for (int j = 0; j < cardBox.childCount; j++)
        {
            Transform child = cardBox.GetChild(j);

            // ���̾ ����Ʈ���� �ƴϸ� ����Ʈ�� ����
            if (cardBox.gameObject.layer != 0)
            {
                child.gameObject.layer = 0;
                // ��Ŀ ó��
                if (child.transform.childCount != 0)
                {
                    child.transform.GetChild(0).gameObject.layer = 0;
                }
            }

            child.SetParent(this.transform);
        }
    }

    public void CardDistribution(GameObject player, int diceValue)
    {
        if(cardGamePlayManager == null)
        {
            Debug.LogAssertion("cardGamePlayManager == null");
            return;
        }

        float delay = 0.5f;
        DG.Tweening.Sequence sequence = DG.Tweening.DOTween.Sequence();

        Debug.Log($"{player.name}�� ī�� �й� ����");
        CardGamePlayerBase playerScript = player.GetComponent<CardGamePlayerBase>();

        if(playerScript == null)
        {
            Debug.LogAssertion("playerScript == null");
            return;
        }

        // ���徿 ������ �÷��̾����� �й�
        for (int i = 0; i < diceValue; i++)
        {
            // �׻� 0���ε����� �ڽİ�ü(ī��)�� ������
            Transform child = transform.GetChild(0);

            // ī�带 �÷��̾��� �ڽİ�ü�� ����, �÷��̾�� ���� ī�带 ����
            playerScript.AddCard(child);

            // ������ �ʱ�ȭ
            child.localScale = Vector3.one * transform.localScale.x;

            // ���� ��й��� �÷��̾ �� �ڽ��̶�� �ڽİ�ü�� ���� ���̾ ��� -> subī�޶��� culling mask�� ���
            if (player.layer == cardGamePlayManager.layerOfMe)
            {
                Debug.Log($"���̾� ���� : {player.layer}");
                child.gameObject.layer = cardGamePlayManager.layerOfMe;

                // ��Ŀ�� ���� �̹����� ���̾� ���� ����� ����ī�޶� �̹����� ���� �� ����
                if(child.transform.childCount != 0)
                {
                    child.transform.GetChild(0).gameObject.layer = cardGamePlayManager.layerOfMe;
                }
            }

            // ī�带 �÷��̾����� �й� Animaition
            sequence.AppendCallback(()=> child.position += Vector3.up*5);
            sequence.Append(
                child.DOMove(child.parent.position + Vector3.up * (i+1), delay)
                );


            // �÷��̾� ���⿡ �°� ī�带 ȸ�� Animaition
            Vector3 targetRotation = child.parent.rotation.eulerAngles;
            sequence.Join(
                child.DORotate(targetRotation, delay, RotateMode.WorldAxisAdd)
                );
        }

        // ī���� �� ��� ��ٸ�
        sequence.AppendInterval(delay * 2);

        // ������ ī�带 ��ħ
        playerScript.GetSequnece_CardSpread(sequence, delay);


        // �÷��̾�(Me)�� ī�� ����� �������� �ڱ� ī�带 Ȯ�� �� �� �ֵ��� ����
        if (player.layer == cardGamePlayManager.layerOfMe)
        {
            //Debug.Log("�� ī�� ���� Ȱ��ȭ!");
            sequence.AppendCallback(()=> 
            {
                cardGamePlayManager.cardGameView.cardScreenButton.TryActivate_Button();
                cardGamePlayManager.CardButtonMemoryPool.InitCardButton(playerScript.closeBox.transform);
            });
        }

        // ��� ó���� �������� ī����� �Ŵ����� ���Ӽ����� �̾ �ϵ��� ��û
        sequence.AppendCallback(()=>cardGamePlayManager.GameSetting());

        sequence.SetLoops(1);
        sequence.Play();
    }


    public void StartDisappearEffect()
    {
        float animationDuration = 0.5f;
        float parentScale = transform.localScale.x;

        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject obj = transform.GetChild(i).gameObject;

            // �߷� ����
            Rigidbody rigid = obj.GetComponent<Rigidbody>();

            rigid.useGravity = false;

            // �̵�
            obj.transform.DOLocalMoveY(5f / parentScale, animationDuration);
            obj.transform.DOLocalMoveZ(10f / parentScale, animationDuration).SetDelay(animationDuration);

            // ȭ�鿡�� ����� �� ��Ȱ��ȭ
            StartCoroutine(CallbackSetDealy(
                () => 
                {
                    rigid.useGravity = true; // �߷� Ȱ��ȭ
                    obj.SetActive(false);
                }
                , animationDuration *2)
                );
        }
    }

    IEnumerator CallbackSetDealy(Action callback,float delay)
    {
        yield return new WaitForSeconds(delay);
        callback();
    }

    // ī���� ��ġ�� �����ϴ� �Լ�
    public void SetCardPositions()
    {
        ReturnAllOfCards();
        for (int i = 0; i < transform.childCount; i++)
        {
            // z������ ī�� ������
            transform.GetChild(i).transform.rotation = Quaternion.Euler(90, 0, 0);

            // �ڽİ�ü�� �ε����� ū ������ ���ʿ� ��ġ��
            transform.GetChild(i).position = floorPos + cardInterval * (i + 1);
        }
    }

    public void CardShuffle()
    {
        // �ڽ� ��ü�� ������ ����
        for (int i = 0; i < transform.childCount; i++)
        {
            int newSiblingIndex = UnityEngine.Random.Range(0, transform.childCount);
            transform.GetChild(i).transform.SetSiblingIndex(newSiblingIndex);
        }

        // ī�带 ���� ��, �� ��ġ�� ����
        SetCardPositions();
    }


}
