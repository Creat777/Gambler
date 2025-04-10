using DG.Tweening;
using UnityEngine;

public class DiceManager : MonoBehaviour
{
    // ������ ����
    public CardGamePlayManager cardGamePlayManager;

    // �ڽİ�ü ����
    public GameObject diceChecker;
    public GameObject whiteDice;

    // ��ũ��Ʈ ����
    int currentDiceValue;

    private void Start()
    {
        if (cardGamePlayManager == null)
            Debug.LogAssertion($"cardGamePlayManager == null ");
    }

    // DiceButton���� ���� ��ư�ݹ�, ���� GameSetting�� ���� ����
    public void RotateDice(GameObject currentPlayer)
    {
        // ȸ�� �����ð��� �������� ����
        float delay;
        delay = Random.Range(1.0f, 2.0f);

        // x�� ȸ��
        int[] randomValues = new int[3];
        for (int i = 0; i < randomValues.Length; i++)
        {
            // ȸ�������ð���ŭ �� ���� ȸ���ϵ��� ����
            randomValues[i] = (int)Random.Range(5f, 10f) * 90 * Mathf.RoundToInt(delay);
            Debug.Log($"randomValues[{i}] : {randomValues[i]}");
        }

        Sequence sequence = DOTween.Sequence();

        // ȸ���� ����
        sequence.Append(whiteDice.transform.DORotate(new Vector3(randomValues[0], randomValues[1], randomValues[2]), delay, RotateMode.FastBeyond360));

        // ���̽� üĿ�� �浹�Ͽ� �˾Ƴ� ���� ���簪���� ����
        sequence.AppendCallback(() => GetDiceValue());

        // ���簪�� �÷��̾ �����ϰ� �ֻ����� �������� ������
        sequence.AppendCallback(() => Debug.Log($"���� �ֻ��� ���� {currentDiceValue}�Դϴ�."));
        CardGamePlayerBase playerScript = currentPlayer.GetComponent<CardGamePlayerBase>();
        if(playerScript != null)
        {
            sequence.AppendCallback(() => playerScript.SetDiceValue(currentDiceValue));
        }
        else
        {
            Debug.LogAssertion($"playerScript == {playerScript} \n" +
                $"�ֻ��� ���� ������� �ʾ���");
        }
        
        // �ֻ��� ���� ������ �� ī��й踦 ����
        sequence.AppendCallback(() => cardGamePlayManager.deckOfCards.CardDistribution(currentPlayer, currentDiceValue));


        // Player(Me)�� ��� �ѹ��� ��ư�� ���� �� �ֵ��� ����
        if (currentPlayer.layer == LayerMask.NameToLayer("Me"))
        {
            cardGamePlayManager.cardGameView.diceButton.TryDeactivate_Button();
        }

        sequence.SetLoops(1);
        sequence.Play();
    }

    public int GetDiceValue()
    {
        currentDiceValue = diceChecker.GetComponent<DiceChecker>().diceValue;

        return currentDiceValue;
    }
}
