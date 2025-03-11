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
            transform.SetParent(newParent); Debug.Log($"{trumpCardInfo.cardName}의 부모객체를 {newParent.gameObject.name}으로 변경");
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

    public bool TrySelectThisCard_OnStartTime(CardGamePlayerBase player)
    {
        if(player == null)
        {
            Debug.Log($"{player.gameObject.name}의 {player.name} == null");
            return false;
        }
        if(trumpCardInfo == null)
        {
            Debug.LogAssertion($"{gameObject.name}의 trumpCardInfo == null");
            return false;
        }

        if (player.TryDownCountPerCardType(trumpCardInfo))
        {
            Debug.Log($"선택된 카드 : {trumpCardInfo.cardName}");
            isSelected = true;
            return true;
        }
        else
        {
            CantSelectThisCard();
            return false;
        }

    }
    public bool TrySelectThisCard_OnPlayTime(CardGamePlayerBase player)
    {
        // 동일한 디버깅은 생략함

        if(player.isSelectCard_ToPresent == false)
        {
            isSelected = true;
            player.Set_isSelectCard_ToPresent(isSelected);
            Debug.Log($"선택된 카드 : {trumpCardInfo.cardName}");
            return isSelected;
        }
        else
        {
            Debug.Log($"{player.gameObject.name}은 이미 카드를 선택했음");
            Debug.Log("해당 내용을 게임에서 알려줄 필요가 있음");
            return false;
        }
    }

    public void UnselectThisCard_OnStartTime(CardGamePlayerBase player)
    {
        player.UpCountPerCardType(trumpCardInfo);
        Debug.Log($"선택 취소된 카드 : {trumpCardInfo.cardName}");
        isSelected = false;
    }

    public void UnselectThisCard_OnPlayTime(CardGamePlayerBase player)
    {
        if (player.isSelectCard_ToPresent == true)
        {
            isSelected = false;
            player.Set_isSelectCard_ToPresent(isSelected);
            Debug.Log($"선택 취소된 카드 : {trumpCardInfo.cardName}");
        }
        else
        {
            Debug.LogAssertion($"{player.gameObject.name}은 카드를 선택한 적이 없음");
        }
    }
}
