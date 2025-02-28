using DG.Tweening;
using UnityEngine;

public class DiceManager : MonoBehaviour
{
    // 에디터 연결
    public CardGamePlayManager cardGamePlayManager;

    // 자식객체 연결
    public GameObject diceChecker;
    public GameObject whiteDice;

    // 스크립트 편집
    int currentDiceValue;

    private void Start()
    {
        if (cardGamePlayManager == null)
            Debug.LogAssertion($"cardGamePlayManager == null ");
    }

    // 버튼콜백(매개변수 Me) 및 스크립트 호출
    public void RotateDice(GameObject currentPlayer)
    {
        // 회전 지연시간을 랜덤으로 설정
        float delay;
        Debug.Log("다이스 딜레이 수정했음");
        //delay = Random.Range(3.0f, 6.0f);
        delay = Random.Range(2.0f, 3.0f);

        // x축 회전
        int[] randomValues = new int[3];
        for (int i = 0; i < randomValues.Length; i++)
        {
            // 회전지연시간만큼 더 많이 회전하도록 만듬
            randomValues[i] = (int)Random.Range(5f, 10f) * 90 * Mathf.RoundToInt(delay);
            Debug.Log($"randomValues[{i}] : {randomValues[i]}");
        }

        Sequence sequence = DOTween.Sequence();

        // 회전값 적용
        sequence.Append(whiteDice.transform.DORotate(new Vector3(randomValues[0], randomValues[1], randomValues[2]), delay, RotateMode.FastBeyond360));

        // 다이스 체커가 충돌하여 알아낸 값을 현재값으로 저장
        sequence.AppendCallback(() => GetDiceValue());

        // 현재값을 플레이어가 저장하고 주사위를 굴렸음을 저장함
        sequence.AppendCallback(() => Debug.Log($"현재 주사위 값은 {currentDiceValue}입니다."));
        CardGamePlayerBase playerScript = currentPlayer.GetComponent<CardGamePlayerBase>();
        if(playerScript != null)
        {
            sequence.AppendCallback(() => playerScript.SetDiceValue(currentDiceValue));
        }
        else
        {
            Debug.LogAssertion($"playerScript == {playerScript} \n" +
                $"주사위 값이 저장되지 않았음");
        }
        
        // 주사위 눈금에 따라서 카드분배를 시작
        sequence.AppendCallback(() => cardGamePlayManager.deckOfCards.CardDistribution(currentPlayer, currentDiceValue));


        // Player(Me)의 경우 한번만 누를 수 있도록 설정
        if (currentPlayer.layer == LayerMask.NameToLayer("Me"))
        {
            cardGamePlayManager.cardGameView.diceButton.Deactivate_Button();
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
