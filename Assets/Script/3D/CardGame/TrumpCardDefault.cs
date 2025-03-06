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
        Debug.Log($"SetTrumpCard����, �μ� : {value}");
        trumpCardInfo = value;

        // ����� �����Ͱ� ���ԵǾ����� Ȯ��
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
            Debug.Log($"{gameObject.name}�� ���õ��� �ʾ���");
        }

        return returnDelay;
    }

    public void SelectThisCard()
    {
        if(trumpCardInfo != null)
        {
            Debug.Log($"���õ� ī�� : {trumpCardInfo.cardName}");
            isSelected = true;
        }
        else
        {
            Debug.LogAssertion("trumpCardInfo == null");
        }
    }

    public void UnselectThisCard()
    {
        Debug.Log($"���� ��ҵ� ī�� : {trumpCardInfo.cardName}");
        isSelected = false;
    }
}
