using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class ButtonBase : MonoBehaviour
{
    private Button _button;

    protected Button button
    {
        get
        {
            if (_button == null) _button = GetComponent<Button>(); 

            if(_button != null) return _button;
            else 
            { 
                Debug.LogAssertion($"{gameObject.name} ��ü�� ��ư������Ʈ�� �������� ����"); 
                return null; 
            }
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
}
