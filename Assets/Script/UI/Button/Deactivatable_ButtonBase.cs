using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class Deactivatable_ButtonBase : ButtonBase
{

    /// <summary>
    /// ��ưŬ�� ��Ȱ��ȭ
    /// </summary>
    public virtual bool TryDeactivate_Button()
    {

        if (button == null)
            InitDefault();

        if (button != null)
        {
            button.interactable = false;

            // ��ȣ�ۿ��� ���� ���� �� �⺻���� ����Ǵ� ������ ����
            ColorBlock colorBlock = button.colors;
            Color color = colorBlock.disabledColor;

            color.a = 1.0f;

            colorBlock.disabledColor = color;
            button.colors = colorBlock;

            return true;
        }
        else
        {
            Debug.LogAssertion($"{gameObject.name}�� button == null");
            return false;
        }
    }

    /// <summary>
    /// ��ưŬ�� Ȱ��ȭ
    /// </summary>
    public virtual bool TryActivate_Button()
    {
        if (button == null)
            InitDefault();

        if (gameObject.activeSelf == false)
        {
            Debug.Log($"{gameObject.name}�� Ȱ��ȭ���� �ʾ���");
            return false;
        }

        if (button != null)
        {
            button.interactable = true;
            return true;
        }
        else
        {
            Debug.LogAssertion($"{gameObject.name}�� button == null");
            return false;
        }
    }
}
