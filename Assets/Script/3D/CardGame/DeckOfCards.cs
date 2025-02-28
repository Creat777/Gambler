using DG.Tweening;
using PublicSet;
using System;
using Unity.VisualScripting;
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
    private GameObject[] Cards;
    private Vector3 floorPos;
    
    private int PlayerCount;


    private void Awake()
    {
        floorPos = floor.transform.position;

        

        // ó���� ��ġ ����
        SetCardPositions();
    }

    private void Start()
    {
        if (cardGamePlayManager == null)
            Debug.LogAssertion($"cardGamePlayManager == null ");

        // ī�� �ʱ�ȭ
        InitCards();
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
        string fileNamePath = "CSV/Card/Card";
        CsvManager.Instance.LoadCsv<cTrumpCardInfo>(fileNamePath,
            (row, trumpCardInfo) => // trumpCard�� ������Ʈ�� ���� ��ũ��Ʈ
            {
                // ��������� �Ҵ���� ���� ���
                if (trumpCardInfo == null) return;

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
                        default: break;
                    }
                    field_num++;
                }

                // ó���� �����͸� �� ī�忡 ����
                if(trumpCardInfo.cardIndex>=0 && trumpCardInfo.cardIndex < Cards.Length)
                {
                    Cards[trumpCardInfo.cardIndex].AddComponent<TrumpCard>().SetTrumpCard(trumpCardInfo);
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
        for (int i = 0; i < Players.Length; i++)
        {
            for (int j = Players[i].transform.childCount-1; j >= 0; j--)
            {
                Transform child = Players[i].transform.GetChild(j);

                // �ڻ� ��ü�� ���� ���̾ ������̸� ��ȯ�� ���̾ ����Ʈ�� ����
                if (Players[i].layer == cardGamePlayManager.layerOfMe)
                {
                    child.gameObject.layer = 0;
                }

                child.SetParent(this.transform);
            }
        }
    }

    public void CardDistribution(GameObject player, int diceValue)
    {
        float delay = 0.5f;
        DG.Tweening.Sequence sequence = DG.Tweening.DOTween.Sequence();

        Debug.Log($"{player.name}�� ī�� �й� ����");

        // ���徿 ������ �÷��̾����� �й�
        for (int i = 0; i < diceValue; i++)
        {
            // �׻� 0���ε����� �ڽİ�ü(ī��)�� ������
            Transform child = transform.GetChild(0);

            // ī�带 �÷��̾��� �ڽİ�ü�� ����
            child.SetParent(player.transform);

            // ������ �ʱ�ȭ
            child.localScale = Vector3.one * transform.localScale.x;

            // ���� ��й��� �÷��̾ �� �ڽ��̶�� �ڽİ�ü�� ���� ���̾ ��� -> subī�޶��� culling mask�� ���
            if (player.layer == cardGamePlayManager.layerOfMe)
            {
                Debug.Log($"���̾� ���� : {player.layer}");
                child.gameObject.layer = cardGamePlayManager.layerOfMe;
            }

            // ī�带 �÷��̾����� �й�
            sequence.AppendCallback(()=> child.position += Vector3.up*5);
            sequence.Append(
                child.DOMove(child.parent.position + Vector3.up * (i+1), delay)
                );

            // �÷��̾� ���⿡ �°� ī�带 ȸ��
            Vector3 targetRotation = child.parent.rotation.eulerAngles;
            sequence.Join(
                child.DORotate(targetRotation, delay, RotateMode.WorldAxisAdd)
                );
        }

        // ī���� �� ��� ��ٸ�
        sequence.AppendInterval(delay * 2 );

        // �÷��̾ ���� ī�带 ��ġ��
        sequence.AppendInterval(delay); // join�� ���� Ÿ��
        float width = float.MinValue;
        float multiple = 1.1f;
        for (int i = 0; i < player.transform.childCount; i++)
        {
            if (width < -1)
            {
                Renderer render = player.transform.GetChild(i).gameObject.GetComponent<Renderer>();

                // ī�޶�� ����
                if (render != null)
                {
                    width = render.bounds.size.x * multiple;
                }
            }


            // �÷��̾ �������� ��� ī�带 ��ħ
            // ¦���� ��� ī�� ���̰� �߾����� ��ġ
            // Ȧ���� ��� ī�尡 �߾����� ��ġ
            float offset = (diceValue % 2 == 0) ? (i - diceValue / 2 + 0.5f) : (i - diceValue / 2);
            sequence.Join(player.transform.GetChild(i).DOLocalMoveX(width * offset, delay));
        }

        // �÷��̾�(Me)�� ī�� ����� �������� �ڱ� ī�带 Ȯ�� �� �� �ֵ��� ����
        if(player.layer == cardGamePlayManager.layerOfMe)
        {
            //Debug.Log("�� ī�� ���� Ȱ��ȭ!");
            sequence.AppendCallback(()=> cardGamePlayManager.cardGameView.cardScreenButton.Activate_Button());
        }

        // ��� ó���� �������� ī����� �Ŵ����� ���� ������ �÷��̾ �����ϵ��� ��û
        sequence.AppendCallback(cardGamePlayManager.NPC_Dice);

        sequence.SetLoops(1);
        sequence.Play();
    }

    /*
    public void CardDistribution(GameObject player, int diceValue)
    {
        float delay = 0.5f;
        DG.Tweening.Sequence sequence = DG.Tweening.DOTween.Sequence();

        int count = 20;

        // ���徿 ������ �÷��̾����� �й�
        for (int i = 0; i < count; i++)
        {
            // �÷��̾� ��� ����
            int PlayerOrder = i % 4;

            // �÷��̾�� ���� ī���� ��� ����
            int PlayerCardNumber = i / 4;

            Transform child = transform.GetChild(0);

            // ī�带 �÷��̾��� �ڽİ�ü�� ����
            child.SetParent(Players[PlayerOrder].transform);

            // ������ �ʱ�ȭ
            child.localScale = Vector3.one * transform.localScale.x;

            // ���� ��й��� �÷��̾ �� �ڽ��̶�� �ڽİ�ü�� ���� ���̾ ���
            if (Players[PlayerOrder].layer == layerOfMe)
            {
                Debug.Log($"���̾� ���� : {Players[PlayerOrder].layer}");
                child.gameObject.layer = layerOfMe;
            }

            // �� ī�带 �÷��̾����� �й�
            sequence.AppendCallback(() => child.position += Vector3.up * 5);
            sequence.Append(
                child.DOMove(child.parent.position + Vector3.up * PlayerCardNumber, delay)
                );

            // �÷��̾� ���⿡ �°� ī�带 ȸ��
            Vector3 targetRotation = child.parent.rotation.eulerAngles;
            sequence.Join(
                child.DORotate(targetRotation, delay, RotateMode.WorldAxisAdd)
                );
        }

        // ī���� �� ��� ��ٸ�
        sequence.AppendInterval(delay);

        float width = float.MinValue;
        float multiple = 1.2f;
        // �� �÷��̾ ���� ī�带 ��ġ��
        sequence.AppendInterval(1);
        for (int i = 0; i < Players.Length; i++)
        {
            for (int i = 0; i < Players[i].transform.childCount; i++)
            {
                if (width < -1)
                {
                    Renderer render = Players[i].transform.GetChild(i).gameObject.GetComponent<Renderer>();

                    // ī�޶�� ����
                    if (render != null)
                    {
                        width = render.bounds.size.x * multiple;
                    }
                }

                // �÷��̾ �������� ��� �÷��̾ ���ÿ� 5���� ��ħ
                sequence.Join(Players[i].transform.GetChild(i).DOLocalMoveX(width * (i - 2), delay));
            }
        }

        if (endCallback != null)
        {
            sequence.AppendCallback(() => endCallback());
        }

        sequence.SetLoops(1);
        sequence.Play();
    }
    */

    // ī���� ��ġ�� �����ϴ� �Լ�
    private void SetCardPositions()
    {
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
