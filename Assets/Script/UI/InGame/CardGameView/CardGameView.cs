using UnityEngine;
using System;
using System.Collections;
using DG.Tweening;
using PublicSet;

public class CardGameView : MonoBehaviour
{
    // 에디터 연결
    public PlayerInterface_CardGame playerInterface;
    public CardGamePlayManager cardGamePlayManager;
    public CardScreenButton cardScreenButton;
    public DiceButton diceButton;
    public SelectCompleteButton selectCompleteButton;

    public GameObject SubScreen_Card;
    public GameObject StartButton;

    public DeckOfCards deckOfCards;

    // 스크립트 편집
    
    private bool isCardScreenInCenter;
    private Vector3 CenterPos;
    
    private Vector3 CardScreen_OutOfMainScreenPos;

    public void InitAttribute()
    {
        selectCompleteButton.InitAttribute();
        diceButton.TryActivate_Button();
        cardScreenButton.TryDeactivate_Button();
    }

    private void Awake()
    {
        isCardScreenInCenter = false;
        CenterPos = transform.position;
        
        CardScreen_OutOfMainScreenPos = SubScreen_Card.transform.position;

        if (cardGamePlayManager == null)
            Debug.LogAssertion($"cardGamePlayManager == null ");
    }

    private void OnEnable()
    {
        cardGamePlayManager.EnterCardGame();
    }


    public void StartGame()
    {
        Sequence sequence = DOTween.Sequence();

        // 인터페이스 초기화
        playerInterface.returnInterface();

        // dotween이 끝난후 복귀할 스케일 저장
        Vector3 scaleOrigin = StartButton.transform.localScale;

        // dotween 애니메이션 시간
        float delay = 0.5f;

        // 게임 시작 누를 시 필요없는 인터페이스 비활성화
        // 화면에서 버튼의 크기를 완전히 줄인다음 실제 크기로 복귀와 동시에 비활성화
        sequence.Append(StartButton.transform.DOScale(Vector3.zero, delay));
        sequence.AppendCallback(()=>StartButton.transform.localScale = scaleOrigin);
        sequence.AppendCallback(() => StartButton.SetActive(false));

        // 모든 카드를 덱의 자식객체로 전환
        sequence.AppendCallback(() => deckOfCards.ReturnAllOfCards());

        // 카드 셔플 하고 덱을 뷰의 가운데로 이동
        sequence.AppendCallback(() => deckOfCards.CardShuffle());
        sequence.AppendInterval(2f); // 카드가 중력으로 바닥에 떨어질때까지 기다림

        // 인터페이스 활성화
        playerInterface.GetSequnce_InterfaceOn(sequence);

        sequence.AppendCallback(
            () =>
            {
                // progress 전환
                CardGamePlayManager.Instance.NextProgress();

                // 세팅 초기화
                CardGamePlayManager.Instance.InitCurrentGame();
                diceButton.TryActivate_Button();
                
            });
        
        sequence.SetLoops(1);
        sequence.Play();
    }

    

    // 백버튼 콜백
    public void ReturnCasino()
    {
        CallbackManager.Instance.EnterCasino();
    }

    IEnumerator startDelay(Action startCallback, Action endCallback, float delay)
    {
        startCallback();
        yield return new WaitForSeconds(delay);
        endCallback();
    }

    


    public float GetSequnce_CardScrrenOpen(Sequence sequence)
    {
        float delay = 1.0f;
        float returnDelay = 0;

        // 스크린을 센터로 불러오는 경우
        if (isCardScreenInCenter == false)
        {
            isCardScreenInCenter = true;
            cardScreenButton.SetButtonCallback(cardScreenButton.subScreenClose);
            sequence.Append(SubScreen_Card.transform.DOMove(CenterPos, delay));
            returnDelay += delay;
        }

        return returnDelay;
    }

    public float GetSequnce_CardScrrenClose(Sequence sequence)
    {
        Debug.Log("GetSequnce_CardScrrenClose 실행");
        float delay = 1.0f;
        float returnDelay = 0;

        // 스크린을 밖으로 빼는 경우
        if (isCardScreenInCenter == true)
        {
            isCardScreenInCenter = false;
            cardScreenButton.SetButtonCallback(cardScreenButton.subScreenOpen);
            sequence.Append(SubScreen_Card.transform.DOMove(CardScreen_OutOfMainScreenPos, delay));
            returnDelay += delay;
        }

        return returnDelay;
    }

}
