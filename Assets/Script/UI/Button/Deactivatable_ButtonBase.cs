using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class Deactivatable_ButtonBase : ButtonBase
{

    /// <summary>
    /// 버튼클릭 비활성화
    /// </summary>
    public virtual bool TryDeactivate_Button()
    {

        button.interactable = false;

        // 상호작용을 하지 않을 시 기본으로 적용되는 반투명 제거
        ColorBlock colorBlock = button.colors;
        Color color = colorBlock.disabledColor;

        color.a = 1.0f;

        colorBlock.disabledColor = color;
        button.colors = colorBlock;

        return true;
    }

    /// <summary>
    /// 버튼클릭 활성화
    /// </summary>
    public virtual bool TryActivate_Button()
    {
        if (gameObject.activeSelf == false)
        {
            Debug.Log($"{gameObject.name}은 활성화되지 않았음");
            return false;
        }

        button.interactable = true;
        return true;
    }
}
