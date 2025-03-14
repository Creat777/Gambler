using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class ButtonBase : MonoBehaviour
{
    protected Button button;


    protected virtual void Awake()
    {
        InitDefault();
    }

    /// <summary>
    /// ��ư��ü�� �⺻ �ʱ�ȭ
    /// </summary>
    protected virtual void InitDefault()
    {
        button = GetComponent<Button>();
        if (button == null)
        {
            Debug.LogAssertion($"{gameObject.name} ��ü�� ��ư������Ʈ�� �������� ����");
            Destroy(gameObject);
            return;
        }
    }

    /// <summary>
    /// �ݹ��Լ��� ������ �ϳ��� ���� �� ������, ���� ���� ���� �� �����Լ��� ���� �Լ��� �������
    /// </summary>
    /// <param name="callback"></param>
    public void SetButtonCallback(UnityAction callback)
    {
        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(callback);
        }
        else
        {
            Debug.Log("button == null");
        }
    }

    /// <summary>
    /// ���� �ݹ��Լ��� �߰��ϴ� �Լ�
    /// </summary>
    /// <param name="callback"></param>
    public void AddButtonCallback(UnityAction callback)
    {
        if (button != null)
        {
            button.onClick.AddListener(callback);
        }
        else
        {
            Debug.Log("button == null");
        }
    }
}
