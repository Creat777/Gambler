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

    private void CantSelectThisCard()
    {
        // ȭ�鿡 �޼����� �������
        Debug.Log("���� ī��� ���õ� �� ����");
    }

    public bool TrySelectThisCard(CardGamePlayerBase player)
    {
        if(player == null)
        {
            Debug.Log($"{player.gameObject.name}�� {player.name} == null");
            return false;
        }

        if(trumpCardInfo != null)
        {
            if(player.TryDownCountPerCardType(trumpCardInfo))
            {
                Debug.Log($"���õ� ī�� : {trumpCardInfo.cardName}");
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
            Debug.LogAssertion($"{gameObject.name}�� trumpCardInfo == null");
            return false;
        }
    }

    public void UnselectThisCard(CardGamePlayerBase player)
    {
        if (player == null)
        {
            Debug.Log($"{player.gameObject.name}�� {player.name} == null");
            return;
        }

        player.UpCountPerCardType(trumpCardInfo);
        Debug.Log($"���� ��ҵ� ī�� : {trumpCardInfo.cardName}");
        isSelected = false;
    }
}
