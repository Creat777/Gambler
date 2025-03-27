using PublicSet;
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
        CardGamePlayManager.Instance.NextProgress(); // 201을 실행

        // 버튼 클릭시 콜백을 변경
        isAttack = true;
        m_SelectCompleteButton.SetButtonCallback(1);
        // 상대를 지목하고 카드 선택을 완료하면 202를 실행해야함
    }

    

    public override void DeffenceFromOtherPlayers(CardGamePlayerBase AttackerScript)
    {
        CardGamePlayManager.Instance.NextProgress(); // 301을 실행

        // 버튼 클릭시 콜백을 변경
        isAttack = false;
        m_SelectCompleteButton.SetButtonCallback(1);
        // 상대를 지목하고 카드 선택을 완료하면 302를 실행해야함
    }
}
