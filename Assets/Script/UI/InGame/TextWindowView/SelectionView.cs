using DG.Tweening;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SelectionView : MonoBehaviour
{
    // �̱��� �ƴϰ� ���ϰ� �����ϱ� ���ؼ� ���
    [SerializeField] private SelectionView instance;
    public SelectionView Instance_ { get { return instance; } private set { } }

    // �����Ϳ��� ����
    [SerializeField] private Button selection1;
    [SerializeField] private Button selection2;
    public Button __selection1 {  get { return selection1; } set { selection1 = value; } }
    public Button __selection2 { get { return selection2; } set { selection2 = value; } }

    // ��ũ��Ʈ���� ����


    void Start()
    {
        
    }

    // �Ű������� ���� �Լ��� ��ư Ŭ�� �̺�Ʈ�� ���
    // Action : �⺻�븮��
    public void RegisterButtonClick_Selection1(UnityAction callback)
    {
        // ������� �ٸ� �ʿ���� �Լ��� ������ ����
        __selection1.onClick.RemoveAllListeners();

        __selection1.onClick.AddListener(
            () =>
            {
                callback();
                // �ݹ��� ����� �� ��ư�� �߰��� �ݹ��Լ��� ������
                __selection1.onClick.RemoveAllListeners();
            }
            );
    }

    public void RegisterButtonClick_Selection2(UnityAction callback)
    {
        __selection2.onClick.RemoveAllListeners();

        __selection2.onClick.AddListener(
            ()=>
            {
                callback();
                __selection2.onClick.RemoveAllListeners();
            }
            );
    }
}
