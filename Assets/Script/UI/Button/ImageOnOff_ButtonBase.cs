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
            Debug.Log($"{gameObject.name} ��ü�� �̹��� ������Ʈ ����");
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
            Debug.Log($"{gameObject.name} ��ü�� �̹��� ������Ʈ ����");
            return false;
        }
    }
}
