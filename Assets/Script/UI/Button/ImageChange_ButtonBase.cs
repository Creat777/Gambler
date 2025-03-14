using UnityEngine;
using UnityEngine.UI;

public abstract class ImageChange_ButtonBase : ButtonBase
{
    protected Image image;

    protected override void InitDefault()
    {
        base.InitDefault();
        image = GetComponent<Image>();
    }

    protected virtual void ChangeOn()
    {
        if(image != null)
        {
            image.color = Color.white;
        }
        else
        {
            Debug.Log($"{gameObject.name} 객체는 이미지 컴포넌트 없음");
        }
    }

    protected virtual void ChangeOff()
    {
        if (image != null)
        {
            image.color = new Color(0.5f, 0.5f, 0.5f, 1.0f);
        }
        else
        {
            Debug.Log($"{gameObject.name} 객체는 이미지 컴포넌트 없음");
        }
    }
}
