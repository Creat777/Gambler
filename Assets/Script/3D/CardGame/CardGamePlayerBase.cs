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

        //Debug.Log($"{gameObject.name}의 눈금은 {myDiceValue}입니다" +
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
                Debug.Log($"카드{cardScript.gameObject.name}는 뒤집혀 있음");
                cardScript.transform.SetParent(closeBox.transform);
            }
            else
            {
                Debug.Log($"카드{cardScript.gameObject.name}는 공개되어 있음");
                cardScript.transform.SetParent(openBox.transform);
            }
        }
        else
        {
            Debug.LogWarning($"{cardScript.gameObject.name}는 TrumpCardDefault 이 없음");
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

        // 해당 카드박스에 카드가 없으면 종료
        if (cardBox.transform.childCount == 0)
        {
            Debug.Log("카드가 없음");
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

        //모든 카드의 중력 끄기
        for (int i = 0; i < cardCount; i++)
        {
            Rigidbody cardRigid = cardBox.transform.GetChild(i).GetComponent<Rigidbody>();
            if(cardRigid != null)
            {
                sequence.AppendCallback( ()=>cardRigid.useGravity = false);
            }
            else
            {
                Debug.LogAssertion($"{cardBox.transform.GetChild(i).gameObject.name}에 rigidbody 없음");
                return returnDealy;
            }
        }

        // 카드를 모두 카드박스가 위치한 정 중앙으로 이동
        sequence.AppendInterval(delay);
        returnDealy += delay;

        for (int i = 0; i < cardCount; i++)
        {
            Vector3 targetPos = cardBox.transform.position + Vector3.up * i * 0.2f;
            sequence.Join(cardBox.transform.GetChild(i).DOMove(targetPos, delay));
        }

        // 카드가 1개만 있으면 펼칠 필요는 없음
        if(cardBox.transform.childCount != 1)
        {
            // 정 중앙에서 카드를 퍼트림
            sequence.AppendInterval(delay);
            returnDealy += delay;

            for (int i = 0; i < cardCount; i++)
            {
                // 짝수의 경우 카드 사이가 중앙으로 위치
                // 홀수의 경우 카드가 중앙으로 위치
                float offset = (cardCount % 2 == 0) ? (i - cardCount / 2 + 0.5f) : (i - cardCount / 2);
                sequence.Join(cardBox.transform.GetChild(i).DOLocalMoveX(width * offset, delay));
            }
        }

        //모든 카드의 중력 켜기
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
                    // 카드 오픈이 한번에 되도록 만듬
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
                    Debug.LogAssertion($"{card.gameObject.name}의 newSequnce == null");
                }
                
            }
            else
            {
                Debug.LogAssertion($"{cardScript.gameObject.name}은 {cardScript.name}을 갖고있지 않음");
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
            Debug.Log("카드 개수를 줄일 수 없음");
            return false;
        }

    }
}
