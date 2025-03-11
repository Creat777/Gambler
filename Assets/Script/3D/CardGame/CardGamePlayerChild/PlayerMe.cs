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
        Debug.Log("버튼 클릭 대기 중...");

        // 버튼이 클릭될 때까지 대기
        yield return new WaitUntil(() => isButtonClicked);

        // 다음번에 또 클릭될 수 있도록 함
        isButtonClicked = false;

        Debug.Log("버튼이 클릭됨! 다음 로직 실행");

        // 이후 게임 진행 로직
    }
}
