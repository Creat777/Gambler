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
    public List<GameObject> revealedCardList { get; protected set; } // Ŭ����ڽ��� ������� ���ӿ��� ������ ī�� ���

    public Dictionary<eCardType, int> cardCountPerType { get; protected set; } // ���� ������ ���� �÷��̾ �����ִ� �� ������ ī�� ����


    public int coin;

    
    public bool diceDone {  get; private set; }
    public bool AttackDone { get; protected set; }
    
    public int myDiceValue { get; private set; }
    

    protected virtual void Awake()
    {
        InitAttribute_All();
        
    }

    public virtual void InitAttribute_All()
    {
        if (CardList == null) CardList = new List<Transform>();
        else CardList.Clear();

        if (openedCardList == null) openedCardList = new List<GameObject>();
        else openedCardList.Clear();

        if (closedCardList == null) closedCardList = new List<GameObject>();
        else closedCardList.Clear();

        if (revealedCardList == null) revealedCardList = new List<GameObject>();
        else revealedCardList.Clear();

        diceDone = false;
        AttackDone = false;
        
        myDiceValue = 0;

        InitAttribute_ForNextOrder();
    }

    public virtual void InitAttribute_ForNextOrder()
    {
        if(cardCountPerType == null) cardCountPerType = new Dictionary<eCardType, int>();
        foreach (eCardType type in Enum.GetValues(typeof(eCardType)))
        {
            if(cardCountPerType.ContainsKey(type)) cardCountPerType[type] = 0;
            else cardCountPerType.Add(type, 0);
        }
        AttackTarget = null;
        PresentedCardScript = null;
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
            randomValue = Mathf.CeilToInt(randomValue / 10f) * 10; // 10�� �ڸ� �ø�
            coin = randomValue;
        }
        else
        {
            coin = value;
        }
    }

    public abstract void AddCoin(int value);

    /// <summary>
    /// �÷��̾ ������ �ݾ׿��� ���� ���� �������� �� �Ļ꿩�θ� �� �� ����
    /// </summary>
    /// <param name="value"></param>
    /// <returns>�����ϴµ� ������ �ݾ�</returns>
    public abstract int TryMinusCoin(int value, out bool isBankrupt);

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

        if(gameObject.CompareTag("Player"))
        {
            CardButtonMemoryPool.Instance.InitCardButton(closeBox.transform);
        }
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

            // �ѹ� ���µǾ��ٰ� �ٽ� ���з� ������ ī��� �̹� ������ ��
            if (revealedCardList.Contains(card) == false)
            {
                revealedCardList.Add(card);
            }
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
        if (revealedCardList.Contains(card))
        {
            revealedCardList.Remove(card);
        }



    }

    public void OrganizeCard(Transform card)
    {

        TrumpCardDefault cardScript = card.GetComponent<TrumpCardDefault>();

        if (cardScript != null)
        {
            if (cardScript.isFaceDown)
            {
                Debug.Log($"ī��{cardScript.trumpCardInfo.cardName}�� ������ ����");
                SetParent_CloseBox(card.gameObject);
            }
            else
            {
                Debug.Log($"ī��{cardScript.trumpCardInfo.cardName}�� �����Ǿ� ����");
                SetParent_OpenBox(card.gameObject);
            }
        }
        else
        {
            Debug.LogWarning($"{cardScript.gameObject.name}�� TrumpCardDefault �� ����");
        }

    }

    //public void GetSequnceJoin_AllCardRotation(Sequence sequence, float delay, GameObject cardBox)
    //{
    //    for(int i = 0; i < cardBox.transform.childCount; i++)
    //    {
    //        // �÷��̾� ���⿡ �°� ī�带 ȸ��
    //        Vector3 targetRotation = transform.rotation.eulerAngles;
    //        Vector3 currentRotation = cardBox.transform.GetChild(i).rotation.eulerAngles;
    //        sequence.Join(
    //            cardBox.transform.GetChild(i).DORotate(targetRotation - currentRotation, delay, RotateMode.WorldAxisAdd)
    //            );
    //    }
    //}

    //public void GetSequnceJoin_CardRotation(Sequence sequence, float delay, Transform card)
    //{
    //    // �÷��̾� ���⿡ �°� ī�带 ȸ��
    //    Vector3 targetRotation = transform.rotation.eulerAngles;
    //    Vector3 currentRotation = card.rotation.eulerAngles;
    //    sequence.Join(
    //        card.DORotate(targetRotation - currentRotation, delay, RotateMode.WorldAxisAdd)
    //        );
    //}

    public float GetSequnece_CardSpread(Sequence sequence, float delay)
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
        if(cardBox.transform.childCount > 1)
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
    
    public void GetSequnce_OpenAndCloseCards(Sequence sequence)
    {
        Debug.Log("GetSequnce_CompleteSelectCard ����");

        int num = 0;
        foreach(Transform card in CardList)
        {
            TrumpCardDefault cardScript = card.GetComponent<TrumpCardDefault>();
            if(cardScript != null)
            {
                Sequence newSequnce = DOTween.Sequence();

                bool result = cardScript.GetSequnce_TryCardOpen(newSequnce, this);
                if(result == false)
                {
                    cardScript.GetSequnce_TryCardClose(newSequnce, this);
                }

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
        else if(AttackTarget != null)
        {
            Debug.Log($"�̹� ���ݴ���� {AttackTarget.characterInfo.CharacterName}���� �����߽��ϴ�.");
            return false;
        }
        else
        {
            AttackTarget = target;
            CardGamePlayManager.Instance.SetDeffender(AttackTarget);
            Debug.Log($"{gameObject.name}�� ���� ��� : {AttackTarget.characterInfo.CharacterName}");
            return true;
        }
    }

    public virtual void ClearAttackTarget()
    {
        if(AttackTarget != null)
        {
            Debug.Log($"���� ����� ����մϴ�. ���� ���ݴ�� : {AttackTarget.characterInfo.CharacterName}");
            AttackTarget = null;
        }
        CardGamePlayManager.Instance.SetDeffender(null);
    }

    public virtual bool TyrSetPresentedCard(TrumpCardDefault card)
    {
        PresentedCardScript = card;
        return true;
    }

    
    public void PlaySequnce_PresentCard(bool isAttack)
    {
        Sequence sequence = DOTween.Sequence();
        float returnDelay = 0;
        float progressDelay = 2.0f;

        // ī�带 �����ϴ� �ִϸ��̼�
        returnDelay = GetSequnce_PresentCard(sequence, isAttack);

        // �� ���� ������
        Debug.Log($"{gameObject.name}�� ������ ������");

        if (isAttack)
        {
            AttackDone = true;
        }

        // �������� ����
        sequence.AppendInterval(progressDelay);
        sequence.AppendCallback(CardGamePlayManager.Instance.NextProgress);

        sequence.SetLoops(1);
        sequence.Play();
        //Debug.Log($"�ִϸ��̼� �ð� : {returnDelay}");
    }

    /// <summary>
    /// ī�带 �߾����� �����ϴ� �ִϸ��̼�
    /// </summary>
    /// <param name="sequence"></param>
    /// <param name="isAttack"></param>
    /// <returns></returns>
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
