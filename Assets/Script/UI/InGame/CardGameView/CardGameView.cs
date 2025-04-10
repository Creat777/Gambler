using UnityEngine;
using System;
using System.Collections;
using DG.Tweening;
using PublicSet;

public class CardGameView : MonoBehaviour
{
    // ������ ����
    public PlayerInterface_CardGame playerInterface;
    public CardGamePlayManager cardGamePlayManager;
    public CardScreenButton cardScreenButton;
    public DiceButton diceButton;
    public SelectCompleteButton selectCompleteButton;
    public CasinoView casinoView;

    public GameObject SubScreen_Card;
    public GameObject StartButton;

    public DeckOfCards deckOfCards;

    // ��ũ��Ʈ ����
    
    private bool isCardScreenInCenter;
    private Vector3 CenterPos;
    
    private Vector3 CardScreen_OutOfMainScreenPos;

    private Vector3 StartButtonScaleOrigin;

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

        StartButtonScaleOrigin = StartButton.transform.localScale;
    }

    private void OnEnable()
    {
        cardGamePlayManager.EnterCardGame();
    }

    private void OnDisable()
    {
        casinoView.onlyOneLivesButton.SetPlayerCantPlayThis();
    }


    public void StartGame()
    {
        Sequence sequence = DOTween.Sequence();

        // �������̽� �ʱ�ȭ
        playerInterface.returnInterface();

        // start��ư ����
        GetSequnce_StartButtonFadeOut(sequence);
        
        // ��� ī�带 ���� �ڽİ�ü�� ��ȯ
        sequence.AppendCallback(() => deckOfCards.ReturnAllOfCards());

        // ī�� ���� �ϰ� ���� ���� ����� �̵�
        sequence.AppendCallback(() => deckOfCards.CardShuffle());
        sequence.AppendInterval(2f); // ī�尡 �߷����� �ٴڿ� ������������ ��ٸ�

        // �������̽� Ȱ��ȭ
        playerInterface.GetSequnce_InterfaceOn(sequence);

        sequence.AppendCallback(
            () =>
            {
                // progress ��ȯ
                CardGamePlayManager.Instance.NextProgress();

                // ���� �ʱ�ȭ
                CardGamePlayManager.Instance.InitCurrentGame();
                diceButton.TryActivate_Button();
                
            });
        
        sequence.SetLoops(1);
        sequence.Play();
    }

    public void InitStartButton()
    {
        StartButton.transform.localScale = Vector3.zero;
        StartButton.gameObject.SetActive(false);
    }
    
    public void GetSequnce_StartButtonFadeOut(Sequence sequence)
    {
        // dotween �ִϸ��̼� �ð�
        float delay = 0.5f;

        // ���� ���� ���� �� �ʿ���� �������̽� ��Ȱ��ȭ
        // ȭ�鿡�� ��ư�� ũ�⸦ ������ ���δ��� ���� ũ��� ���Ϳ� ���ÿ� ��Ȱ��ȭ
        sequence.Append(StartButton.transform.DOScale(Vector3.zero, delay));
        sequence.AppendCallback(() => StartButton.SetActive(false));
    }

    public void PlaySequnce_StartButtonFadeIn()
    {
        Sequence sequence = DOTween.Sequence();
        GetSequnce_StartButtonFadeIn(sequence);
        sequence.SetLoops(1);
        sequence.Play();
    }
    public void GetSequnce_StartButtonFadeIn(Sequence sequence)
    {
        // dotween �ִϸ��̼� �ð�
        float delay = 0.5f;

        sequence.AppendCallback(() => StartButton.SetActive(true));
        sequence.Append(StartButton.transform.DOScale(StartButtonScaleOrigin, delay));
        
    }

    // ���ư �ݹ�
    public void ReturnCasino()
    {
        CallbackManager.Instance.EnterCasino();
    }

    public float GetSequnce_CardScrrenOpen(Sequence sequence)
    {
        float delay = 1.0f;
        float returnDelay = 0;

        // ��ũ���� ���ͷ� �ҷ����� ���
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
        Debug.Log("GetSequnce_CardScrrenClose ����");
        float delay = 1.0f;
        float returnDelay = 0;

        // ��ũ���� ������ ���� ���
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
