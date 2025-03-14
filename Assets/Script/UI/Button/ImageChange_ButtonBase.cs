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
            Debug.Log($"{gameObject.name} ��ü�� �̹��� ������Ʈ ����");
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
            Debug.Log($"{gameObject.name} ��ü�� �̹��� ������Ʈ ����");
        }
    }
}
