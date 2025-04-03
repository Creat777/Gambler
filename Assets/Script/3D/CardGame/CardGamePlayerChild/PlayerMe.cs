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

    public override void AddCoin(int value)
    {
        coin += value;
        PlayManager.Instance.AddPlayerMoney(value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value">양수를 값으로 받음</param>
    /// <returns></returns>
    public override int TryMinusCoin(int value, out bool isBankrupt)
    {
        if (coin <= value) //파산
        {
            value = coin; // 남은돈에 한해서 상대방에게 지급함
            coin = 0;
            isBankrupt = true;
        }
        else
        {
            coin = coin - value;
            isBankrupt = false;
        }
        PlayManager.Instance.AddPlayerMoney(-value);

        // 플레이어가 소지한 금액이 최대값
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
        CardGamePlayManager.Instance.NextProgress(); // 201을 실행

        // 게임어시스턴트로 선택이 가능하도록 만듬
        GameAssistantPopUp_OnlyOneLives.Instance.LiftRestrictionToAllSelections();

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
