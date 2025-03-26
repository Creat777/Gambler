using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using PublicSet;
using System;

public abstract class CardGamePlayerBase : MonoBehaviour
{
    // ������
    public CardGamePlayManager cardGamePlayManager;
    public GameObject closeBox;
    public GameObject openBox;
    public cCharacterInfo characterInfo {  get; private set; }

    // ��ũ��Ʈ

    public CardGamePlayerBase AttackTarget {  get; protected set; } // ���� ���
    public TrumpCardDefault PresentedCardScript { get; protected set; } // ���� �Ǵ� ���� ����� ī��
    public List<Transform> CardList {  get; protected set; }
    public List<GameObject> openedCardList { get; protected set; } // ���¹ڽ� �ڽİ�ü�� �ִ� ī��
    public List<GameObject> closedCardList { get; protected set; } // Ŭ����ڽ� �ڽİ�ü�� �ִ� ī��

    public Dictionary<eCardType, int> cardCountPerType { get; protected set; } // ���� ������ ���� �÷��̾ �����ִ� �� ������ ī�� ����


    public int coin;

    
    public bool diceDone {  get; private set; }
    public bool AttackDone { get; protected set; }
    
    public int myDiceValue { get; private set; }
    

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
        AttackTarget = null;

        if (CardList == null) CardList = new List<Transform>();
        else CardList.Clear();

        if (openedCardList == null) openedCardList = new List<GameObject>();
        else openedCardList.Clear();

        if (closedCardList == null) closedCardList = new List<GameObject>();
        else closedCardList.Clear();

        diceDone = false;
        AttackDone = false;
        
        myDiceValue = 0;
    }

    /// <summary>
    /// �÷��̾�� ������ �ִ� �׼��� ����ؾ���,
    /// ��ǻ�ʹ� �Ű������� �Է����� ����
    /// </summary>
    /// <param name="value"> ��ǻ���� ��� ���������� ������ </param>
    public void SetCoin(int value = int.MinValue)
    {
        // ��ǻ���� ���
        if (value == int.MinValue)
        {
            int randomValue = UnityEngine.Random.Range(100, 500);
            coin = randomValue;
        }
        else
        {
            coin = value;
        }
    }

    public void AddCoin(int value)
    {
        coin += value;
    }

    public void SetCharacterInfo(cCharacterInfo info)
    {
        characterInfo = info;
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
        CardList.Add(card);
        TrumpCardDefault cardScript = card.GetComponent<TrumpCardDefault>();
        if (cardScript != null) UpCountPerCardType(cardScript.trumpCardInfo);
        else
        {
            Debug.LogAssertion($"{card.gameObject.name}�� ��ũ��Ʈ�� �������� ����");
            return;
        }
        OrganizeCard(card);
    }

    public void ShuffleCard_CloseBox()
    {
        for (int i = 0; i < closeBox.transform.childCount; i++)
        {
            int newSiblingIndex = UnityEngine.Random.Range(0, transform.childCount);
            Transform closedCard = closeBox.transform.GetChild(i);
            closedCard.SetSiblingIndex(newSiblingIndex);
        }

        // ���� ī�带 �˸��� ������ ���ġ
        Sequence sequence = DOTween.Sequence();
        float delay = 0.5f;
        GetSequnece_CardSpread_individual(sequence, delay, closeBox);

        sequence.SetLoops(1);
        sequence.Play();
    }

    public void UpCountPerCardType(cTrumpCardInfo cardInfo)
    {
        cardCountPerType[cardInfo.cardType]++;

        if(gameObject.tag == "Player")
        {
            cardGamePlayManager.cardGameView.selectCompleteButton.CheckCompleteSelect_OnChooseCardsToReveal(cardCountPerType);
        }

        Debug.Log($"{gameObject.name}���� {cardInfo.cardName}ī�� �߰�");
        Debug.Log($"{gameObject.name}�� {cardInfo.cardType} ī�尳�� : {cardCountPerType[cardInfo.cardType]}");
    }

    public void SetParent_CloseBox(GameObject card)
    {
        card.transform.SetParent(closeBox.transform);

        // �ش� ī�尡 ����Ʈ�� ������ �߰��ϰ�
        if (closedCardList.Contains(card) == false)
            closedCardList.Add(card);

        // �ݴ��� ����Ʈ���� ����
        if (openedCardList.Contains(card))
        {
            openedCardList.Remove(card);
        }
    }

    public void SetParent_OpenBox(GameObject card)
    {
        card.transform.SetParent(openBox.transform);

        // �ش� ī�尡 ����Ʈ�� ������ �߰��ϰ�
        if (openedCardList.Contains(card) == false)
            openedCardList.Add(card);

        // �ݴ��� ����Ʈ���� ����
        if (closedCardList.Contains(card))
        {
            closedCardList.Remove(card);
        }
    }

    public void OrganizeCard(Transform card)
    {

        TrumpCardDefault cardScript = card.GetComponent<TrumpCardDefault>();

        if (cardScript != null)
        {
            if (cardScript.trumpCardInfo.isFaceDown)
            {
                Debug.Log($"ī��{cardScript.gameObject.name}�� ������ ����");
                SetParent_CloseBox(card.gameObject);
            }
            else
            {
                Debug.Log($"ī��{cardScript.gameObject.name}�� �����Ǿ� ����");
                SetParent_OpenBox(card.gameObject);
            }
        }
        else
        {
            Debug.LogWarning($"{cardScript.gameObject.name}�� TrumpCardDefault �� ����");
        }

    }


    public float GetSequnece_CardSpread(Sequence sequence, float delay = 0.5f)
    {
        Debug.Log("GetSequnece_CardSpread ����");

        float returnDelay = 0;
        returnDelay += GetSequnece_CardSpread_individual(sequence, delay, closeBox);
        returnDelay += GetSequnece_CardSpread_individual(sequence, delay, openBox); 
        return returnDelay;
    }

    private float GetSequnece_CardSpread_individual(Sequence sequence, float delay, GameObject cardBox)
    {
        Debug.Log("GetSequnece_CardSpread_individual ����");

        float returnDealy = 0;

        // �ش� ī��ڽ��� ī�尡 ������ ����
        if (cardBox.transform.childCount == 0)
        {
            Debug.Log("ī�尡 ����");
            return returnDealy;
        }

        float width = 1.9f;

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
        Debug.Log("GetSequnce_CompleteSelectCard ����");
        float returnDelay = 0;

        int num = 0;
        foreach(Transform card in CardList)
        {
            TrumpCardDefault cardScript = card.GetComponent<TrumpCardDefault>();
            if(cardScript != null)
            {
                Sequence newSequnce = DOTween.Sequence();
                returnDelay += cardScript.GetSequnce_TryCardOpen(newSequnce, this);

                if (newSequnce != null)
                {
                    // ī�� ������ �ѹ��� �ǵ��� ����
                    if (num == 0)
                    {
                        sequence.Append(newSequnce);
                        num++;
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
            
        }
        return returnDelay;
    }

    public virtual bool TryDownCountPerCardType(cTrumpCardInfo cardInfo)
    {
        if (cardCountPerType[cardInfo.cardType] > 1)
        {
            cardCountPerType[cardInfo.cardType]--;

            // �÷��̾ TryDownCountPerCardType�� ������ �� ������ �Ϸ�ƴ����� Ȯ���ϰ� ��ư�� Ȱ��ȭ��
            if (gameObject.tag == "Player")
            {
                cardGamePlayManager.cardGameView.selectCompleteButton.CheckCompleteSelect_OnChooseCardsToReveal(cardCountPerType);
            }
            
            Debug.Log($"{gameObject.name}���� {cardInfo.cardName}ī�� ����");
            Debug.Log($"{gameObject.name}�� {cardInfo.cardType.ToString()} ���� ī�� �� : {cardCountPerType[cardInfo.cardType]}");
            return true;
        }
        else
        {
            Debug.Log($"{gameObject.name}�� {cardInfo.cardType.ToString()}�� ���� ī�� �� : {cardCountPerType[cardInfo.cardType]}");
            Debug.Log("ī�� ������ ���� �� ����");
            return false;
        }
    }

    // �����÷��̾�� ��ǻ�Ϳ��� ���� ������
    public abstract void AttackOtherPlayers(List<CardGamePlayerBase> PlayerList);
    public abstract void DeffenceFromOtherPlayers(CardGamePlayerBase AttackerScript);

    public virtual bool TrySetAttackTarget(CardGamePlayerBase target)
    {
        if (target.gameObject == gameObject)
        {
            Debug.Log("�ڱ� �ڽ��� ���ݴ������ ���� �� ����");
            return false;
        }
        else
        {
            AttackTarget = target;
            CardGamePlayManager.Instance.SetDeffender(AttackTarget);
            Debug.Log($"{gameObject.name}�� ���� ��� : {target.gameObject.name}");
            return true;
        }
    }

    public float GetSequnce_PresentCard(Sequence sequence, bool isAttack)
    {
        float returnDelay = 0;
        float delay = 0.5f;
        float width = 1.9f;


        // ī�� �߷� ����
        Rigidbody cardRigid =  PresentedCardScript.GetComponent<Rigidbody>();
        if(cardRigid == null)
        {
            Debug.LogAssertion($"{PresentedCardScript.trumpCardInfo.cardName}�� rigidBody�� ����");
            return returnDelay;
        }
        sequence.AppendCallback(() => cardRigid.useGravity = false);

        // �߾����� �̵�
        Vector3 targetPos = CardGamePlayManager.Instance.deckOfCards.transform.position;
        if (isAttack) // �����ϴ°Ÿ� �߾ӿ��� ���ʿ� ��ġ
        {
            targetPos.x -= width/2;
        }
        else // �����ϴ°Ÿ� �߾ӿ��� �����ʿ� ��ġ
        {
            targetPos.x += width/2;
        }
        
        sequence.Append(PresentedCardScript.transform.DOMove(targetPos, delay)); returnDelay += delay;

        // ī�� ����� �� ó�� ȸ���ߴ� ���� ������
        Vector3 targetRotation = transform.rotation.eulerAngles;
        sequence.Join(PresentedCardScript.transform.DORotate(-targetRotation, delay, RotateMode.WorldAxisAdd));

        // �߷� Ȱ��ȭ
        sequence.AppendCallback(() => cardRigid.useGravity = true);

        return returnDelay;
    }
}
