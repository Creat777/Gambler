using UnityEngine;
using System;
using System.Collections;
using DG.Tweening;

public class CardGameView : MonoBehaviour
{
    // ������ ����
    public GameObject PlayerInterface;
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
        
    }

    private void Start()
    {
        isPlayerInterfaceInScreen = false;
        isCardScreenInCenter = false;
        CenterPos = transform.position;
        Interface_OutOfMainScreenPos = PlayerInterface.transform.position;
        CardScreen_OutOfMainScreenPos = SubScreen_Card.transform.position;
    }

    // ���۹�ư �ݹ�
    public void StartGameButton()
    {
        // ���� ���� ���� �� �ʿ���� �������̽� ��Ȱ��ȭ
        StartButton.SetActive(false);
        
        // ��� ī�带 ���� �ڽİ�ü�� ��ȯ
        DeckOfCards.ReturnAllOfCards();

        StartCoroutine
        (
            startDelay(
            DeckOfCards.CardShuffle, // ī�带 ���� �ڽİ�ü �������� ����
            ()=> DeckOfCards.CardDistribution(PlayerInterfaceOnOff), // ī�� �й� �� �÷��̾� �������̽� Ȱ��ȭ
            2)
        );
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
