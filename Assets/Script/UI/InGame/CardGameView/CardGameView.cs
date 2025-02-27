using UnityEngine;
using System;
using System.Collections;
using DG.Tweening;

public class CardGameView : MonoBehaviour
{
    // 에디터 연결
    public GameObject PlayerInterface;
    public GameObject SubScreen_Card;
    public GameObject StartButton;

    public DeckOfCards DeckOfCards;

    // 스크립트 편집
    private bool isPlayerInterfaceInScreen;
    private bool isCardScreenInCenter;
    private Vector3 CenterPos;
    private Vector3 Interface_OutOfMainScreenPos;
    private Vector3 CardScreen_OutOfMainScreenPos;

    private void Awake()
    {
        
    }

    private void Start()
    {
        isPlayerInterfaceInScreen = false;
        isCardScreenInCenter = false;
        CenterPos = transform.position;
        Interface_OutOfMainScreenPos = PlayerInterface.transform.position;
        CardScreen_OutOfMainScreenPos = SubScreen_Card.transform.position;
    }

    // 시작버튼 콜백
    public void StartGameButton()
    {
        // 게임 시작 누를 시 필요없는 인터페이스 비활성화
        StartButton.SetActive(false);
        
        // 모든 카드를 덱의 자식객체로 전환
        DeckOfCards.ReturnAllOfCards();

        StartCoroutine
        (
            startDelay(
            DeckOfCards.CardShuffle, // 카드를 덱의 자식객체 순서에서 섞기
            ()=> DeckOfCards.CardDistribution(PlayerInterfaceOnOff), // 카드 분배 후 플레이어 인터페이스 활성화
            2)
        );
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

    private void PlayerInterfaceOnOff()
    {
        float delay = 1.0f;

        if (isPlayerInterfaceInScreen == false)
        {
            isPlayerInterfaceInScreen = true;
            PlayerInterface.transform.DOMove(CenterPos, delay);
        }

        else if (isPlayerInterfaceInScreen == true)
        {
            isPlayerInterfaceInScreen = false;
            PlayerInterface.transform.DOMove(Interface_OutOfMainScreenPos, delay);
        }
    }

    // 버튼 콜백
    public void CardScrrenButton()
    {
        float delay = 1.0f;

        // 스크린을 센터로 불러오는 경우
        if(isCardScreenInCenter == false)
        {
            isCardScreenInCenter = true;
            SubScreen_Card.transform.DOMove(CenterPos, delay);
        }

        // 스크린을 밖으로 빼는 경우
        else if(isCardScreenInCenter == true)
        {
            isCardScreenInCenter = false;
            SubScreen_Card.transform.DOMove(CardScreen_OutOfMainScreenPos, delay);
        }
    }
}
