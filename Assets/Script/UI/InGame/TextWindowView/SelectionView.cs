using DG.Tweening;
using System;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SelectionView : MonoBehaviour
{
    // �����Ϳ��� ����
    [SerializeField] private Button[] selection;

    // ��ũ��Ʈ���� ����
    Vector3 intervalOfselection;
    Vector3[] selectionPositions;

    private void Awake()
    {
        //���������� ���� (y��)
        intervalOfselection = selection[0].transform.position - selection[1].transform.position;

        // �������� 2�� �� ���� �⺻ ������
        selectionPositions = new Vector3[selection.Length - 1];
        for (int i = 0; i < selection.Length - 1; i++)
        {
            selectionPositions[i] = selection[i].transform.position;
        }
    }

    // �Ű������� ���� �Լ��� ��ư Ŭ�� �̺�Ʈ�� ���
    // Action : �⺻�븮��
    public void RegisterButtonClick_Selection(int index, string selectionScript, UnityAction callback)
    {
        if (index >= selection.Length) return;

        selection[index].transform.GetChild(0).GetComponent<Text>().text = selectionScript;

        // ���� ������ ���� ��ϵ� ��� �ݹ��Լ��� ����
        selection[index].onClick.RemoveAllListeners();

        // �ݹ��� ������ �Ŀ� �����Ǻ䰡 �������� ��
        selection[index].onClick.AddListener(
            () =>
            {
                callback();
                gameObject.SetActive(false);
            }
            );

        // 3 ��° �������� �����
        if (index == 2)
        {
            // 3�� �������� Ȱ��ȭ, 3�� �������� ��ġ�� ���� 2�� �ڸ��� ���� Ȯ���� ��
            selection[index].gameObject.SetActive(true);
            selection[index].transform.position = selectionPositions[index - 1];

            // 1���� 2�� �������� ��ġ�� ��ĭ�� �ø�
            for (int i = 0; i < selection.Length-1; i++)
            {
                selection[i].transform.position = selectionPositions[i] + intervalOfselection;
            }
            
        }
    }

    // �������� �ݹ��Լ� ���� �� �����Ǻ�� ��Ȱ��ȭ��
    private void OnDisable()
    {
        //�������� ������ ������ ��� ����ġ
        selection[2].gameObject.SetActive(false);

        for (int i = 0; i < selection.Length - 1; i++)
        {
            selection[i].transform.position = selectionPositions[i];
        }
    }
}
