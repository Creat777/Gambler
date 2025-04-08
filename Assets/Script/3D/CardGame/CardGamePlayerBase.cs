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
    public List<GameObject> revealedCardList { get; protected set; } // 클로즈박스에 상관없이 게임에서 공개된 카드 목록

    public Dictionary<eCardType, int> cardCountPerType { get; protected set; } // 게임 세팅을 위해 플레이어가 갖고있는 각 문양의 카드 숫자


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
            randomValue = Mathf.CeilToInt(randomValue / 10f) * 10; // 10의 자리 올림
            coin = randomValue;
        }
        else
        {
            coin = value;
        }
    }

    public abstract void AddCoin(int value);

    /// <summary>
    /// 플레이어가 소지한 금액에서 현재 값을 차감했을 때 파산여부를 알 수 있음
    /// </summary>
    /// <param name="value"></param>
    /// <returns>차감하는데 성공한 금액</returns>
    public abstract int TryMinusCoin(int value, out bool isBankrupt);

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

            // 한번 오픈되었다가 다시 손패로 가져간 카드는 이미 공개된 것
            if (revealedCardList.Contains(card) == false)
            {
                revealedCardList.Add(card);
            }
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
                Debug.Log($"카드{cardScript.trumpCardInfo.cardName}는 뒤집혀 있음");
                SetParent_CloseBox(card.gameObject);
            }
            else
            {
                Debug.Log($"카드{cardScript.trumpCardInfo.cardName}는 공개되어 있음");
                SetParent_OpenBox(card.gameObject);
            }
        }
        else
        {
            Debug.LogWarning($"{cardScript.gameObject.name}는 TrumpCardDefault 이 없음");
        }

    }

    //public void GetSequnceJoin_AllCardRotation(Sequence sequence, float delay, GameObject cardBox)
    //{
    //    for(int i = 0; i < cardBox.transform.childCount; i++)
    //    {
    //        // 플레이어 방향에 맞게 카드를 회전
    //        Vector3 targetRotation = transform.rotation.eulerAngles;
    //        Vector3 currentRotation = cardBox.transform.GetChild(i).rotation.eulerAngles;
    //        sequence.Join(
    //            cardBox.transform.GetChild(i).DORotate(targetRotation - currentRotation, delay, RotateMode.WorldAxisAdd)
    //            );
    //    }
    //}

    //public void GetSequnceJoin_CardRotation(Sequence sequence, float delay, Transform card)
    //{
    //    // 플레이어 방향에 맞게 카드를 회전
    //    Vector3 targetRotation = transform.rotation.eulerAngles;
    //    Vector3 currentRotation = card.rotation.eulerAngles;
    //    sequence.Join(
    //        card.DORotate(targetRotation - currentRotation, delay, RotateMode.WorldAxisAdd)
    //        );
    //}

    public float GetSequnece_CardSpread(Sequence sequence, float delay)
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
        if(cardBox.transform.childCount > 1)
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
    
    public void GetSequnce_OpenAndCloseCards(Sequence sequence)
    {
        Debug.Log("GetSequnce_CompleteSelectCard 실행");

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
        else if(AttackTarget != null)
        {
            Debug.Log($"이미 공격대상을 {AttackTarget.characterInfo.CharacterName}으로 선택했습니다.");
            return false;
        }
        else
        {
            AttackTarget = target;
            CardGamePlayManager.Instance.SetDeffender(AttackTarget);
            Debug.Log($"{gameObject.name}의 공격 대상 : {AttackTarget.characterInfo.CharacterName}");
            return true;
        }
    }

    public virtual void ClearAttackTarget()
    {
        if(AttackTarget != null)
        {
            Debug.Log($"공격 대상을 취소합니다. 기존 공격대상 : {AttackTarget.characterInfo.CharacterName}");
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

        // 카드를 제시하는 애니메이션
        returnDelay = GetSequnce_PresentCard(sequence, isAttack);

        // 내 공격 끝내기
        Debug.Log($"{gameObject.name}이 공격을 실행함");

        if (isAttack)
        {
            AttackDone = true;
        }

        // 다음으로 진행
        sequence.AppendInterval(progressDelay);
        sequence.AppendCallback(CardGamePlayManager.Instance.NextProgress);

        sequence.SetLoops(1);
        sequence.Play();
        //Debug.Log($"애니메이션 시간 : {returnDelay}");
    }

    /// <summary>
    /// 카드를 중앙으로 제시하는 애니메이션
    /// </summary>
    /// <param name="sequence"></param>
    /// <param name="isAttack"></param>
    /// <returns></returns>
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
