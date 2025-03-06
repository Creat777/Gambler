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

    public void SelectThisCard()
    {
        if(trumpCardInfo != null)
        {
            Debug.Log($"선택된 카드 : {trumpCardInfo.cardName}");
            isSelected = true;
        }
        else
        {
            Debug.LogAssertion("trumpCardInfo == null");
        }
    }

    public void UnselectThisCard()
    {
        Debug.Log($"선택 취소된 카드 : {trumpCardInfo.cardName}");
        isSelected = false;
    }
}
