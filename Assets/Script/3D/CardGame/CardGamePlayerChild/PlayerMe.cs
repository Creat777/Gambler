using PublicSet;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMe : CardGamePlayerBase
{
    //에디터 연결
    [SerializeField] private SelectCompleteButton selectCompleteButton;
    public SelectCompleteButton m_SelectCompleteButton
    {
        get 
        {
            if (m_SelectCompleteButton == null)
            {
                Debug.LogAssertion("SelectCompleteButton 연결안됨");
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
        // 버튼 클릭시 콜백을 추가
        m_SelectCompleteButton.AddButtonCallback(CoroutineManager.Instance.SetBool_isButtonClicked_True);

        // 해당 버튼콜백이 실행되면 다음의 콜백이 실행됨
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
