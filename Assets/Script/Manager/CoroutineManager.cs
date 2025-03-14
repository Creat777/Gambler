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
        Debug.Log("��ư Ŭ�� ��� ��...");

        // ��ư�� Ŭ���� ������ ���
        yield return new WaitUntil(() => isButtonClicked);

        callbackAfterButtonClick();

        // �������� �� Ŭ���� �� �ֵ��� ��
        isButtonClicked = false;

        Debug.Log("��ư�� Ŭ����! ���� ���� ����");

        // ���� ���� ���� ����
    }
}
