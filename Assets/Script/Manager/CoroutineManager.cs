using System;
using System.Collections;
using UnityEngine;

public class CoroutineManager : Singleton<CoroutineManager>
{
    public bool isButtonClicked { get; private set; }

    private void Start()
    {
        isButtonClicked = false;
    }

    public void SetBool_isButtonClicked_True()
    {
        isButtonClicked = true;
    }

    public IEnumerator WaitForButtonClick(Action callbackAfterButtonClick)
    {
        Debug.Log("버튼 클릭 대기 중...");

        // 버튼이 클릭될 때까지 대기
        yield return new WaitUntil(() => isButtonClicked);

        callbackAfterButtonClick();

        // 다음번에 또 클릭될 수 있도록 함
        isButtonClicked = false;

        Debug.Log("버튼이 클릭됨! 다음 로직 실행");

        // 이후 게임 진행 로직
    }
}
