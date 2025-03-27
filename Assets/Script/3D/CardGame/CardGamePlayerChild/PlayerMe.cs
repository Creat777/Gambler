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

    public void InitAttribute_PlayerMe()
    {
        isCompleteSelect_OnGameSetting = false;
        isCompleteSelect_OnPlayTime = false;
    }


    public void Set_isCompleteSelect_OnGameSetting(bool value)
    {
        isCompleteSelect_OnGameSetting = value;
    }
    public virtual void Set_isCompleteSelect_OnPlayTime(bool setValue)
    {
        isCompleteSelect_OnPlayTime = setValue;
    }
    


    public override void AttackOtherPlayers(List<CardGamePlayerBase> playerList)
    {
        CardGamePlayManager.Instance.NextProgress(); // 201�� ����

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
