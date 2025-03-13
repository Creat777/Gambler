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
        Debug.Log("��ư Ŭ�� ��� ��...");

        // ��ư�� Ŭ���� ������ ���
        yield return new WaitUntil(() => isButtonClicked);

        // �������� �� Ŭ���� �� �ֵ��� ��
        isButtonClicked = false;

        Debug.Log("��ư�� Ŭ����! ���� ���� ����");

        // ���� ���� ���� ����
    }

    public override void DefenceFromOtherPlayers(CardGamePlayerBase AttackerScript)
    {
        
    }
}
