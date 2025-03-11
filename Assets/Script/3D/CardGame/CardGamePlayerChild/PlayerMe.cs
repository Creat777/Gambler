using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMe : CardGamePlayerBase
{
    bool isButtonClicked;

    private void Start()
    {
        isButtonClicked = false;
    }

    public void SetBoolButtonClickTrue()
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
}
