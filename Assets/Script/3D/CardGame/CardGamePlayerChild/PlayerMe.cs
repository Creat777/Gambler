using PublicSet;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMe : CardGamePlayerBase
{

    //������ ����
    [SerializeField] private SelectCompleteButton selectCompleteButton;
    public SelectCompleteButton m_SelectCompleteButton
    {
        get 
        {
            if (selectCompleteButton == null)
            {
                selectCompleteButton = CardGamePlayManager.Instance.cardGameView.selectCompleteButton;
            }
            return selectCompleteButton;
        }
    }
    public bool isCompleteSelect_OnGameSetting {  get; private set; }
    public bool isCompleteSelect_OnPlayTime { get; private set; }
    public bool isAttack {  get; private set; }

    private void Start()
    {
        cCharacterInfo info = CsvManager.Instance.GetCharacterInfo(eCharacterType.Player);
        SetCharacterInfo(info);
    }

    public override void AddCoin(int value)
    {
        coin += value;
        PlayManager.Instance.AddPlayerMoney(value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value">����� ������ ����</param>
    /// <returns></returns>
    public override int TryMinusCoin(int value, out bool isBankrupt)
    {
        if (coin <= value) //�Ļ�
        {
            value = coin; // �������� ���ؼ� ���濡�� ������
            coin = 0;
            isBankrupt = true;
        }
        else
        {
            coin = coin - value;
            isBankrupt = false;
        }
        PlayManager.Instance.AddPlayerMoney(-value);

        // �÷��̾ ������ �ݾ��� �ִ밪
        return value;
    }


    public override void InitAttribute_All()
    {
        base.InitAttribute_All();
        isCompleteSelect_OnGameSetting = false;
    }

    public override void InitAttribute_ForNextOrder()
    {
        base.InitAttribute_ForNextOrder();
        isCompleteSelect_OnPlayTime = false;
    }


    public void Set_isCompleteSelect_OnGameSetting(bool value)
    {
        isCompleteSelect_OnGameSetting = value;
    }
    public virtual void Set_isCompleteSelect_OnPlayTime(bool value)
    {
        isCompleteSelect_OnPlayTime = value;
    }
    


    public override void AttackOtherPlayers(List<CardGamePlayerBase> playerList)
    {
        CardGamePlayManager.Instance.NextProgress(); // 201�� ����

        // ���Ӿ�ý���Ʈ�� ������ �����ϵ��� ����
        GameAssistantPopUp_OnlyOneLives.Instance.LiftRestrictionToAllSelections();

        // ��ư Ŭ���� �ݹ��� ����
        isAttack = true;
        m_SelectCompleteButton.SetButtonCallback(1);
        // ��븦 �����ϰ� ī�� ������ �Ϸ��ϸ� 202�� �����ؾ���
    }

    

    public override void DeffenceFromOtherPlayers(CardGamePlayerBase AttackerScript)
    {
        CardGamePlayManager.Instance.NextProgress(); // 301�� ����

        // ��ư Ŭ���� �ݹ��� ����
        isAttack = false;
        m_SelectCompleteButton.SetButtonCallback(1);
        // ��븦 �����ϰ� ī�� ������ �Ϸ��ϸ� 302�� �����ؾ���
    }

    
}
