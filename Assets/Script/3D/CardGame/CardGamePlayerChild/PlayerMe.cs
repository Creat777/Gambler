using PublicSet;
using System.Collections;
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
            if (m_SelectCompleteButton == null)
            {
                Debug.LogAssertion("SelectCompleteButton ����ȵ�");
                return null;
            }
            return selectCompleteButton;
        }
    }
    public bool isCompleteSelect_OnGameSetting {  get; private set; }
    public bool isCompleteSelect_OnPlayTime { get; private set; }
    
    
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
    


    public override void AttackOtherPlayers(int currentOrder, List<CardGamePlayerBase> orderdPlayerList)
    {
        // ��ư Ŭ���� �ݹ��� �߰�
        m_SelectCompleteButton.AddButtonCallback(CoroutineManager.Instance.SetBool_isButtonClicked_True);

        // �ش� ��ư�ݹ��� ����Ǹ� ������ �ݹ��� �����
        StartCoroutine(CoroutineManager.Instance.WaitForButtonClick(AttackPanelProcess));
    }
    public override void AttackPanelProcess()
    {
        throw new System.NotImplementedException();
    }

    

    public override void DefenceFromOtherPlayers(CardGamePlayerBase AttackerScript)
    {
        
    }

    
}
