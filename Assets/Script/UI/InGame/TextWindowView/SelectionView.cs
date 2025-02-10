using DG.Tweening;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SelectionView : MonoBehaviour
{
    // �����Ϳ��� ����
    [SerializeField] private Button[] selection;

    // ��ũ��Ʈ���� ����


    void Start()
    {
        
    }

    // �Ű������� ���� �Լ��� ��ư Ŭ�� �̺�Ʈ�� ���
    // Action : �⺻�븮��
    public void RegisterButtonClick_Selection(int i, string selectionScript, UnityAction callback)
    {
        for (int j = 0; j < selection.Length; j++)
        {
            selection[i].transform.GetChild(0).GetComponent<Text>().text = selectionScript;
            selection[i].onClick.RemoveAllListeners();
            selection[i].onClick.AddListener(callback);
        }
    }
}
