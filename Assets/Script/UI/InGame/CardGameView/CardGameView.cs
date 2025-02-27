using UnityEngine;
using System;
using System.Collections;
using DG.Tweening;

public class CardGameView : MonoBehaviour
{
    // ������ ����
    public CardGamePlayManager cardGamePlayManager;
    public GameObject PlayerInterface;
    public CardScreenButton cardScreenButton;
    public DiceButton diceButton;

    public GameObject SubScreen_Card;
    public GameObject StartButton;

    public DeckOfCards DeckOfCards;

    // ��ũ��Ʈ ����
    private bool isPlayerInterfaceInScreen;
    private bool isCardScreenInCenter;
    private Vector3 CenterPos;
    private Vector3 Interface_OutOfMainScreenPos;
    private Vector3 CardScreen_OutOfMainScreenPos;

    private void Awake()
    {
        cardScreenButton.Deactivate_Button();
        diceButton.Deactivate_Button();
    }



    private void Start()
    {
        isPlayerInterfaceInScreen = false;
        isCardScreenInCenter = false;
        CenterPos = transform.position;
        Interface_OutOfMainScreenPos = PlayerInterface.transform.position;
        CardScreen_OutOfMainScreenPos = SubScreen_Card.transform.position;

        if (cardGamePlayManager == null)
            Debug.LogAssertion($"cardGamePlayManager == null ");
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            Debug.LogWarning("�׽�Ʈ");
            StartGameButton();
        }
    }

    // ���۹�ư �ݹ�
    public void StartGameButton()
    {
        // ���� ���� ���� �� �ʿ���� �������̽� ��Ȱ��ȭ
        Sequence sequence = DOTween.Sequence();

        // dotween�� ������ ������ ������ ����
        Vector3 scaleOrigin = StartButton.transform.localScale;

        // dotween �ִϸ��̼� �ð�
        float delay = 0.5f;

        // ȭ�鿡�� ��ư�� ũ�⸦ ������ ���δ��� ���� ũ��� ���Ϳ� ���ÿ� ��Ȱ��ȭ
        sequence.Append(StartButton.transform.DOScale(Vector3.zero, delay));
        sequence.AppendCallback(()=>StartButton.transform.localScale = scaleOrigin);
        sequence.AppendCallback(() => StartButton.SetActive(false));

        // ��� ī�带 ���� �ڽİ�ü�� ��ȯ
        sequence.AppendCallback(() => DeckOfCards.ReturnAllOfCards());

        // ī�� ���� �ϰ� ���� ���� ����� �̵�
        sequence.AppendCallback(() => DeckOfCards.CardShuffle());
        sequence.AppendInterval(2f); // ī�尡 �߷����� �ٴڿ� ������������ ��ٸ�

        // �������̽� Ȱ��ȭ
        sequence.AppendCallback(() => PlayerInterfaceOnOff());
        diceButton.Activate_Button();
    }

    // ���ư �ݹ�
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

    // ��ư �ݹ�
    public void CardScrrenButton()
    {
        float delay = 1.0f;

        // ��ũ���� ���ͷ� �ҷ����� ���
        if(isCardScreenInCenter == false)
        {
            isCardScreenInCenter = true;
            SubScreen_Card.transform.DOMove(CenterPos, delay);
        }

        // ��ũ���� ������ ���� ���
        else if(isCardScreenInCenter == true)
        {
            isCardScreenInCenter = false;
            SubScreen_Card.transform.DOMove(CardScreen_OutOfMainScreenPos, delay);
        }
    }
}
