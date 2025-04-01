using DG.Tweening;
using PublicSet;
using UnityEditor.Build;
using UnityEngine;

public class TrumpCardDefault : MonoBehaviour
{
    public TrumpCardAnimaition animationScript {  get; private set; }
    public cTrumpCardInfo trumpCardInfo {  get; private set; }
    public bool isSelected { get; private set; }
    public bool isFaceDown { get; private set; }

    private void Awake()
    {
        animationScript = GetComponent<TrumpCardAnimaition>();
    }

    public void SetTrumpCard(cTrumpCardInfo value)
    {
        //Debug.Log($"SetTrumpCard����, {gameObject.name}(��ü�̸�) : {value.cardName}(������ ī�� �̸�)");
        trumpCardInfo = value;

        // ����� �����Ͱ� ���ԵǾ����� Ȯ��
        //CsvManager.Instance.PrintProperties(trumpCardInfo);
    }

    public void InitAttribute()
    {
        isSelected = false;
        isFaceDown = true;
    }

    public bool GetSequnce_TryCardOpen(Sequence sequence, CardGamePlayerBase playerScript)
    {
        if(isSelected)
        {
            isFaceDown = true;
            gameObject.layer = 0;
            playerScript.SetParent_OpenBox(gameObject);
            animationScript.GetSequnce_Animation_CardOpen(sequence);

            return true;
        }
        else
        {
            //Debug.Log($"{gameObject.name}�� ���õ��� �ʾ���");
            return false;
        }
    }

    public bool GetSequnce_TryCardClose(Sequence sequence, CardGamePlayerBase playerScript)
    {
        if (isSelected == false)
        {
            isFaceDown = true;
            
            // ���̾� �缳��
            if(playerScript.tag == "Player") gameObject.layer = CardGamePlayManager.Instance.layerOfMe;
            else gameObject.layer = 0;
            
            playerScript.SetParent_CloseBox(gameObject);
            animationScript.GetSequnce_Animation_CardClose(sequence);

            return true;
        }
        else
        {
            //Debug.Log($"{gameObject.name}�� ���õ��� �ʾ���");
            return false;
        }
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
        if (player.tag == "Player")
        {
            PlayerMe playerMe = (player as PlayerMe);
            if (playerMe.isCompleteSelect_OnPlayTime == false)
            {
                isSelected = true;

                playerMe.TyrSetPresentedCard(this);

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

            isSelected = false;
            playerMe.Set_isCompleteSelect_OnPlayTime(isSelected);
            Debug.Log($"���� ��ҵ� ī�� : {trumpCardInfo.cardName}");
        }

        else
        {
            isSelected = false;
        }

    }
}
