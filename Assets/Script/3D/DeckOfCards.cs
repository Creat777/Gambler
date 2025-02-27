using DG.Tweening;
using System;
using UnityEngine;

public class DeckOfCards : MonoBehaviour
{
    // ������ ����
    public Vector3 cardInterval;

    // ������ ����
    public GameObject floor;
    public GameObject[] Players;

    // ��ũ��Ʈ ����
    private GameObject[] Cards;
    private Vector3 floorPos;
    private int layerOfMe { get; set;}


    private void Awake()
    {
        floorPos = floor.transform.position;
        layerOfMe = LayerMask.NameToLayer("Me");

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
    }

    public void ReturnAllOfCards()
    {
        for (int i = 0; i < Players.Length; i++)
        {
            for (int j = Players[i].transform.childCount-1; j >= 0; j--)
            {
                Transform child = Players[i].transform.GetChild(j);

                // �ڻ� ��ü�� ���� ���̾ ������̸� ��ȯ�� ���̾ ����Ʈ�� ����
                if (Players[i].layer == layerOfMe)
                {
                    child.gameObject.layer = 0;
                }

                child.SetParent(this.transform);
            }
        }
    }

    public void CardDistribution(Action endCallback = null)
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
            sequence.AppendCallback(()=> child.position += Vector3.up*5);
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
        sequence.AppendInterval(delay );

        float width = float.MinValue;
        float multiple = 1.2f;
        // �� �÷��̾ ���� ī�带 ��ġ��
        sequence.AppendInterval(1);
        for (int i = 0; i < Players.Length; i ++)
        {
            for (int j = 0; j < Players[i].transform.childCount; j++)
            {
                if(width < -1)
                {
                    Renderer render = Players[i].transform.GetChild(j).gameObject.GetComponent<Renderer>();

                    // ī�޶�� ����
                    if(render != null)
                    {
                        width = render.bounds.size.x * multiple;
                    }
                }

                // �÷��̾ �������� ��� �÷��̾ ���ÿ� 5���� ��ħ
                sequence.Join(Players[i].transform.GetChild(j).DOLocalMoveX(width*(j-2), delay));
            }
        }

        if(endCallback != null)
        {
            sequence.AppendCallback(()=>endCallback());
        }

        sequence.SetLoops(1);
        sequence.Play();
    }

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
