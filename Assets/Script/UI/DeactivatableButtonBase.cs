using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class Deactivatable_Button_Base : MonoBehaviour
{
    Button button;

    public virtual void Deactivate_Button()
    {
        if(button == null)
        {
            button = GetComponent<Button>();
        }

        if(button != null)
        {
            button.interactable = false;

            // 상호작용을 하지 않을 시 반투명 제거
            ColorBlock colorBlock = button.colors;
            Color color = colorBlock.disabledColor;

            color.a = 1.0f;

            colorBlock.disabledColor = color;
            button.colors = colorBlock;
        }
        else
        {
            Debug.Log("button == null");
        }
    }

    public virtual void Activate_Button()
    {
        if (button == null)
        {
            button = GetComponent<Button>();
        }

        if (button != null)
        {
            button.interactable = true;
        }
        else
        {
            Debug.Log("button == null");
        }
    }

    public void SetButtonCallback(UnityAction callback)
    {
        if (button == null)
        {
            button = GetComponent<Button>();
        }

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
