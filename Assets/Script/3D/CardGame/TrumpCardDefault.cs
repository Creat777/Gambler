using DG.Tweening;
using PublicSet;
using UnityEngine;

public class TrumpCardDefault : MonoBehaviour
{
    public TrumpCardAnimaition animationScript {  get; private set; }
    public cTrumpCardInfo trumpCardInfo {  get; private set; }
    public bool isSelected { get; private set; }

    private void Awake()
    {
        animationScript = GetComponent<TrumpCardAnimaition>();
    }

    public void SetTrumpCard(cTrumpCardInfo value)
    {
        Debug.Log($"SetTrumpCard실행, 인수 : {value}");
        trumpCardInfo = value;

        // 제대로 데이터가 삽입되었는지 확인
        CsvManager.Instance.PrintProperties(trumpCardInfo);
    }

    public float GetSequnce_TryCardOpen(Sequence sequence, Transform newParent)
    {
        float returnDelay = 0;
        if(isSelected)
        {
            trumpCardInfo.isFaceDown = false;
            gameObject.layer = 0;
            transform.SetParent(newParent);
            returnDelay += animationScript.GetSequnce_Animation_CardOpen(sequence);
        }
        else
        {
            Debug.Log($"{gameObject.name}은 선택되지 않았음");
        }

        return returnDelay;
    }

    private void CantSelectThisCard()
    {
        // 화면에 메세지를 띄워야함
        Debug.Log("현재 카드는 선택될 수 없음");
    }

    public bool TrySelectThisCard(CardGamePlayerBase player)
    {
        if(player == null)
        {
            Debug.Log($"{player.gameObject.name}의 {player.name} == null");
            return false;
        }

        if(trumpCardInfo != null)
        {
            if(player.TryDownCountPerCardType(trumpCardInfo))
            {
                Debug.Log($"선택된 카드 : {trumpCardInfo.cardName}");
                isSelected = true;
                return true;
            }
            else
            {
                CantSelectThisCard();
                return false;
            }        }
        else
        {
            Debug.LogAssertion($"{gameObject.name}의 trumpCardInfo == null");
            return false;
        }
    }

    public void UnselectThisCard(CardGamePlayerBase player)
    {
        if (player == null)
        {
            Debug.Log($"{player.gameObject.name}의 {player.name} == null");
            return;
        }

        player.UpCountPerCardType(trumpCardInfo);
        Debug.Log($"선택 취소된 카드 : {trumpCardInfo.cardName}");
        isSelected = false;
    }
}
