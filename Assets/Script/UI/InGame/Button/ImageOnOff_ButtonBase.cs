using UnityEngine;
using UnityEngine.UI;

public abstract class ImageOnOff_ButtonBase : ButtonBase
{
    protected Image image;

    protected override void InitDefault()
    {
        base.InitDefault();
        image = GetComponent<Image>();
    }

    protected virtual bool ChangeOnColor()
    {
        if(image != null)
        {
            image.color = Color.white;
            return true;
        }
        else
        {
            Debug.Log($"{gameObject.name} 객체는 이미지 컴포넌트 없음");
            return false;
        }
    }

    protected virtual bool ChangeOffColor()
    {
        if (image != null)
        {
            image.color = new Color(0.5f, 0.5f, 0.5f, 1.0f);
            return true;
        }
        else
        {
            Debug.Log($"{gameObject.name} 객체는 이미지 컴포넌트 없음");
            return false;
        }
    }
}
