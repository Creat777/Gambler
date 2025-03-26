using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using PublicSet;
using System;

public abstract class CardGamePlayerBase : MonoBehaviour
{
    // 에디터
    public CardGamePlayManager cardGamePlayManager;
    public GameObject closeBox;
    public GameObject openBox;
    public cCharacterInfo characterInfo {  get; private set; }

    // 스크립트

    public CardGamePlayerBase AttackTarget {  get; protected set; } // 공격 대상
    public TrumpCardDefault PresentedCardScript { get; protected set; } // 공경 또는 수비에 사용할 카드
    public List<Transform> CardList {  get; protected set; }
    public List<GameObject> openedCardList { get; protected set; } // 오픈박스 자식객체로 있는 카드
    public List<GameObject> closedCardList { get; protected set; } // 클로즈박스 자식객체로 있는 카드

    public Dictionary<eCardType, int> cardCountPerType { get; protected set; } // 게임 세팅을 위해 플레이어가 갖고있는 각 문양의 카드 숫자


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
    /// 플레이어는 가지고 있는 액수를 등록해야함,
    /// 컴퓨터는 매개변수를 입력하지 않음
    /// </summary>
    /// <param name="value"> 컴퓨터의 경우 랜덤값으로 지정됨 </param>
    public void SetCoin(int value = int.MinValue)
    {
        // 컴퓨터의 경우
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

        //Debug.Log($"{gameObject.name}의 눈금은 {myDiceValue}입니다" +
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
            Debug.LogAssertion($"{card.gameObject.name}의 스크립트가 존재하지 않음");
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

        // 섞은 카드를 알맞은 순서로 재배치
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

        Debug.Log($"{gameObject.name}에게 {cardInfo.cardName}카드 추가");
        Debug.Log($"{gameObject.name}의 {cardInfo.cardType} 카드개수 : {cardCountPerType[cardInfo.cardType]}");
    }

    public void SetParent_CloseBox(GameObject card)
    {
        card.transform.SetParent(closeBox.transform);

        // 해당 카드가 리스트에 없으면 추가하고
        if (closedCardList.Contains(card) == false)
            closedCardList.Add(card);

        // 반대쪽 리스트에서 제거
        if (openedCardList.Contains(card))
        {
            openedCardList.Remove(card);
        }
    }

    public void SetParent_OpenBox(GameObject card)
    {
        card.transform.SetParent(openBox.transform);

        // 해당 카드가 리스트에 없으면 추가하고
        if (openedCardList.Contains(card) == false)
            openedCardList.Add(card);

        // 반대쪽 리스트에서 제거
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
                Debug.Log($"카드{cardScript.gameObject.name}는 뒤집혀 있음");
                SetParent_CloseBox(card.gameObject);
            }
            else
            {
                Debug.Log($"카드{cardScript.gameObject.name}는 공개되어 있음");
                SetParent_OpenBox(card.gameObject);
            }
        }
        else
        {
            Debug.LogWarning($"{cardScript.gameObject.name}는 TrumpCardDefault 이 없음");
        }

    }


    public float GetSequnece_CardSpread(Sequence sequence, float delay = 0.5f)
    {
        Debug.Log("GetSequnece_CardSpread 실행");

        float returnDelay = 0;
        returnDelay += GetSequnece_CardSpread_individual(sequence, delay, closeBox);
        returnDelay += GetSequnece_CardSpread_individual(sequence, delay, openBox); 
        return returnDelay;
    }

    private float GetSequnece_CardSpread_individual(Sequence sequence, float delay, GameObject cardBox)
    {
        Debug.Log("GetSequnece_CardSpread_individual 실행");

        float returnDealy = 0;

        // 해당 카드박스에 카드가 없으면 종료
        if (cardBox.transform.childCount == 0)
        {
            Debug.Log("카드가 없음");
            return returnDealy;
        }

        float width = 1.9f;

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
        Debug.Log("GetSequnce_CompleteSelectCard 실행");
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
                    // 카드 오픈이 한번에 되도록 만듬
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
                    Debug.LogAssertion($"{card.gameObject.name}의 newSequnce == null");
                }

            }
            else
            {
                Debug.LogAssertion($"{cardScript.gameObject.name}은 {cardScript.name}을 갖고있지 않음");
            }
            
        }
        return returnDelay;
    }

    public virtual bool TryDownCountPerCardType(cTrumpCardInfo cardInfo)
    {
        if (cardCountPerType[cardInfo.cardType] > 1)
        {
            cardCountPerType[cardInfo.cardType]--;

            // 플레이어가 TryDownCountPerCardType를 실행할 시 선택이 완료됐는지를 확인하고 버튼을 활성화함
            if (gameObject.tag == "Player")
            {
                cardGamePlayManager.cardGameView.selectCompleteButton.CheckCompleteSelect_OnChooseCardsToReveal(cardCountPerType);
            }
            
            Debug.Log($"{gameObject.name}에게 {cardInfo.cardName}카드 제거");
            Debug.Log($"{gameObject.name}의 {cardInfo.cardType.ToString()} 남은 카드 수 : {cardCountPerType[cardInfo.cardType]}");
            return true;
        }
        else
        {
            Debug.Log($"{gameObject.name}의 {cardInfo.cardType.ToString()}의 남은 카드 수 : {cardCountPerType[cardInfo.cardType]}");
            Debug.Log("카드 개수를 줄일 수 없음");
            return false;
        }
    }

    // 실제플레이어와 컴퓨터에서 각각 재정의
    public abstract void AttackOtherPlayers(List<CardGamePlayerBase> PlayerList);
    public abstract void DeffenceFromOtherPlayers(CardGamePlayerBase AttackerScript);

    public virtual bool TrySetAttackTarget(CardGamePlayerBase target)
    {
        if (target.gameObject == gameObject)
        {
            Debug.Log("자기 자신을 공격대상으로 삼을 수 없음");
            return false;
        }
        else
        {
            AttackTarget = target;
            CardGamePlayManager.Instance.SetDeffender(AttackTarget);
            Debug.Log($"{gameObject.name}의 공격 대상 : {target.gameObject.name}");
            return true;
        }
    }

    public float GetSequnce_PresentCard(Sequence sequence, bool isAttack)
    {
        float returnDelay = 0;
        float delay = 0.5f;
        float width = 1.9f;


        // 카드 중력 끄기
        Rigidbody cardRigid =  PresentedCardScript.GetComponent<Rigidbody>();
        if(cardRigid == null)
        {
            Debug.LogAssertion($"{PresentedCardScript.trumpCardInfo.cardName}의 rigidBody가 없음");
            return returnDelay;
        }
        sequence.AppendCallback(() => cardRigid.useGravity = false);

        // 중앙으로 이동
        Vector3 targetPos = CardGamePlayManager.Instance.deckOfCards.transform.position;
        if (isAttack) // 공격하는거면 중앙에서 왼쪽에 위치
        {
            targetPos.x -= width/2;
        }
        else // 수비하는거면 중앙에서 오른쪽에 위치
        {
            targetPos.x += width/2;
        }
        
        sequence.Append(PresentedCardScript.transform.DOMove(targetPos, delay)); returnDelay += delay;

        // 카드 배분할 때 처음 회전했던 값을 복귀함
        Vector3 targetRotation = transform.rotation.eulerAngles;
        sequence.Join(PresentedCardScript.transform.DORotate(-targetRotation, delay, RotateMode.WorldAxisAdd));

        // 중력 활성화
        sequence.AppendCallback(() => cardRigid.useGravity = true);

        return returnDelay;
    }
}
