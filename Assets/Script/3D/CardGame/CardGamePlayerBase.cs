using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using PublicSet;
using System;

public abstract class CardGamePlayerBase : MonoBehaviour
{
    public GameObject closeBox;
    public GameObject openBox;
    public List<Transform> Cards;

    public bool diceDone {  get; private set; }
    public int myDiceValue { get; private set; }
    public Dictionary<eCardType, int> cardCountPerType { get; private set; }

    protected virtual void Awake()
    {
        InitAttribute();
        cardCountPerType = new Dictionary<eCardType, int>();
        foreach (eCardType type in Enum.GetValues(typeof(eCardType)))
        {
            cardCountPerType.Add(type, 0);
        }
    }

    public virtual void InitAttribute()
    {
        Cards = new List<Transform>();
        diceDone = false;
        myDiceValue = 0;
    }

    public virtual void SetDiceValue(int diceValue)
    {
        myDiceValue = diceValue;
        diceDone = true;

        //Debug.Log($"{gameObject.name}�� ������ {myDiceValue}�Դϴ�" +
        //    $"-> diceDone == {diceDone}");
    }

    public void AddCard(Transform card)
    {
        card.SetParent(transform);
        Cards.Add(card);
        OrganizeCard(card);
    }

    public void OrganizeCard(Transform card)
    {

        TrumpCardDefault cardScript = card.GetComponent<TrumpCardDefault>();

        if (cardScript != null)
        {
            if (cardScript.trumpCardInfo.isFaceDown)
            {
                Debug.Log($"ī��{cardScript.gameObject.name}�� ������ ����");
                cardScript.transform.SetParent(closeBox.transform);
            }
            else
            {
                Debug.Log($"ī��{cardScript.gameObject.name}�� �����Ǿ� ����");
                cardScript.transform.SetParent(openBox.transform);
            }
        }
        else
        {
            Debug.LogWarning($"{cardScript.gameObject.name}�� TrumpCardDefault �� ����");
        }

    }


    public float GetSequnece_CardSpread(Sequence sequence, float delay = 0.5f)
    {
        float returnDelay = 0;
        returnDelay += GetSequnece_CardSpread_individual(sequence, delay, closeBox);
        returnDelay += GetSequnece_CardSpread_individual(sequence, delay, openBox); 
        return returnDelay;
    }

    private float GetSequnece_CardSpread_individual(Sequence sequence, float delay, GameObject cardBox)
    {
        float returnDealy = 0;

        // �ش� ī��ڽ��� ī�尡 ������ ����
        if (cardBox.transform.childCount == 0)
        {
            Debug.Log("ī�尡 ����");
            return returnDealy;
        }

        float width = 1.9f;
        /*
        float width = float.MinValue;
        float multiple = 1.1f;

        Renderer render = cardBox.transform.GetChild(0).GetComponent<Renderer>();
        if (render != null)
        {
            width = render.bounds.size.x * multiple;
        }
        else
        {
            Debug.LogAssertion("render == null");
        }
        */

        int cardCount = cardBox.transform.childCount;

        //��� ī���� �߷� ����
        for (int i = 0; i < cardCount; i++)
        {
            Rigidbody cardRigid = cardBox.transform.GetChild(i).GetComponent<Rigidbody>();
            if(cardRigid != null)
            {
                sequence.AppendCallback( ()=>cardRigid.useGravity = false);
            }
            else
            {
                Debug.LogAssertion($"{cardBox.transform.GetChild(i).gameObject.name}�� rigidbody ����");
                return returnDealy;
            }
        }

        // ī�带 ��� ī��ڽ��� ��ġ�� �� �߾����� �̵�
        sequence.AppendInterval(delay);
        returnDealy += delay;

        for (int i = 0; i < cardCount; i++)
        {
            Vector3 targetPos = cardBox.transform.position + Vector3.up * i * 0.2f;
            sequence.Join(cardBox.transform.GetChild(i).DOMove(targetPos, delay));
        }

        // ī�尡 1���� ������ ��ĥ �ʿ�� ����
        if(cardBox.transform.childCount != 1)
        {
            // �� �߾ӿ��� ī�带 ��Ʈ��
            sequence.AppendInterval(delay);
            returnDealy += delay;

            for (int i = 0; i < cardCount; i++)
            {
                // ¦���� ��� ī�� ���̰� �߾����� ��ġ
                // Ȧ���� ��� ī�尡 �߾����� ��ġ
                float offset = (cardCount % 2 == 0) ? (i - cardCount / 2 + 0.5f) : (i - cardCount / 2);
                sequence.Join(cardBox.transform.GetChild(i).DOLocalMoveX(width * offset, delay));
            }
        }

        //��� ī���� �߷� �ѱ�
        for (int i = 0; i < cardCount; i++)
        {
            Rigidbody cardRigid = cardBox.transform.GetChild(i).GetComponent<Rigidbody>();
            if (cardRigid != null)
            {
                sequence.AppendCallback(() => cardRigid.useGravity = true);
            }
        }

        return returnDealy;
    }

    public float GetSequnce_CompleteSelectCard(Sequence sequence)
    {
        float returnDelay = 0;

        int num = 0;
        foreach(Transform card in Cards)
        {
            TrumpCardDefault cardScript = card.GetComponent<TrumpCardDefault>();
            if(cardScript != null)
            {
                Sequence newSequnce = DOTween.Sequence();
                returnDelay += cardScript.GetSequnce_TryCardOpen(newSequnce, openBox.transform);

                if (newSequnce != null)
                {
                    // ī�� ������ �ѹ��� �ǵ��� ����
                    if (num == 0)
                    {
                        sequence.Append(newSequnce);
                    }
                    else
                    {
                        sequence.Join(newSequnce);
                    }

                }
                else
                {
                    Debug.LogAssertion($"{card.gameObject.name}�� newSequnce == null");
                }
                
            }
            else
            {
                Debug.LogAssertion($"{cardScript.gameObject.name}�� {cardScript.name}�� �������� ����");
            }
            num++;
        }
        return returnDelay;
    }

    public void UpCountPerCardType(eCardType cardType)
    {
        cardCountPerType[cardType]++;
    }

    public bool DownCountPerCardType(eCardType cardType)
    {
        if (cardCountPerType[cardType] > 0)
        {
            cardCountPerType[cardType]--;
            return true;
        }
        else
        {
            Debug.Log("ī�� ������ ���� �� ����");
            return false;
        }

    }
}
