using PublicSet;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMe : CardGamePlayerBase
{
    public bool isCompleteSelect_OnGameSetting {  get; private set; }
    public bool isCompleteSelect_OnPlayTime { get; private set; }
    public bool isButtonClicked { get; private set; }
    
    public void InitAttribute_PlayerMe()
    {
        isCompleteSelect_OnGameSetting = false;
        isButtonClicked = false;
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
    public void Set_BoolButtonClick_True()
    {
        isButtonClicked = true;
    }


    public override void AttackOtherPlayers(int currentOrder, List<CardGamePlayerBase> orderdPlayerList)
    {
        StartCoroutine(WaitForButtonClick());
    }

    IEnumerator WaitForButtonClick()
    {
        Debug.Log("버튼 클릭 대기 중...");

        // 버튼이 클릭될 때까지 대기
        yield return new WaitUntil(() => isButtonClicked);

        // 다음번에 또 클릭될 수 있도록 함
        isButtonClicked = false;

        Debug.Log("버튼이 클릭됨! 다음 로직 실행");

        // 이후 게임 진행 로직
    }

    public override void DefenceFromOtherPlayers(CardGamePlayerBase AttackerScript)
    {
        
    }
}
