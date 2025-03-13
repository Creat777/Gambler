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

    public float GetSequnce_TryCardOpen(Sequence sequence, CardGamePlayerBase playerScript)
    {
        float returnDelay = 0;
        if(isSelected)
        {
            trumpCardInfo.isFaceDown = false;
            gameObject.layer = 0;
            playerScript.SetParent_OpenBox(gameObject);
            returnDelay += animationScript.GetSequnce_Animation_CardOpen(sequence);
        }
        else
        {
            //Debug.Log($"{gameObject.name}�� ���õ��� �ʾ���");
        }

        return returnDelay;
    }

    private void CantSelectThisCard()
    {
        // ȭ�鿡 �޼����� �������
        Debug.Log("���� ī��� ���õ� �� ����");
    }

    public bool TrySelectThisCard_OnGameSetting(CardGamePlayerBase player)
    {
        if(player == null)
        {
            Debug.Log($"{player.gameObject.name}�� {player.name} == null");
            return false;
        }
        if(trumpCardInfo == null)
        {
            Debug.LogAssertion($"{gameObject.name}�� trumpCardInfo == null");
            return false;
        }

        if (player.TryDownCountPerCardType(trumpCardInfo))
        {
            Debug.Log($"���õ� ī�� : {trumpCardInfo.cardName}");
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
        // ������ ������� ������

        // �÷��̾��� ��� ������ ���ѵ�
        if(player.tag == "Player")
        {
            PlayerMe playerMe = (player as PlayerMe);
            if (playerMe.isCompleteSelect_OnPlayTime == false)
            {
                isSelected = true;
                playerMe.Set_isCompleteSelect_OnPlayTime(isSelected);
                Debug.Log($"���õ� ī�� : {trumpCardInfo.cardName}");
                return isSelected;
            }
            else
            {
                Debug.Log($"{player.gameObject.name}�� �̹� ī�带 ��������");
                Debug.Log("�ش� ������ ���ӿ��� �˷��� �ʿ䰡 ����");
                return isSelected;
            }
        }

        // ��ǻ���� ��� �ѹ��� ���õ��״� �ٷ� ����
        else
        {
            isSelected = true;
            return true;
        }
        
    }

    public void UnselectThisCard_OnStartTime(CardGamePlayerBase player)
    {
        player.UpCountPerCardType(trumpCardInfo);
        Debug.Log($"���� ��ҵ� ī�� : {trumpCardInfo.cardName}");
        isSelected = false;
    }

    public void UnselectThisCard_OnPlayTime(CardGamePlayerBase player)
    {
        if(player.tag == "Player")
        {
            PlayerMe playerMe = (player as PlayerMe);
            if (playerMe.isCompleteSelect_OnPlayTime == true)
            {
                isSelected = false;
                playerMe.Set_isCompleteSelect_OnPlayTime(isSelected);
                Debug.Log($"���� ��ҵ� ī�� : {trumpCardInfo.cardName}");
            }
            else
            {
                Debug.LogAssertion($"{player.gameObject.name}�� ī�带 ������ ���� ����");
            }
        }

        else
        {
            isSelected = false;
        }

    }
}
