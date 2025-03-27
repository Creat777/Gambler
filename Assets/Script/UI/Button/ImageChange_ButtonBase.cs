using UnityEngine;
using UnityEngine.UI;

public abstract class ImageChange_ButtonBase : ButtonBase
{
    protected Image _image;
    protected Image image
    {
        get
        {
            if (_image == null) _image = GetComponent<Image>();

            if (_image != null) return _image;
            else
            {
                Debug.LogAssertion($"{gameObject.name} ��ü�� ��ư������Ʈ�� �������� ����");
                return null;
            }
        }
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
            image.color = new Color(0.7f, 0.7f, 0.7f, 1.0f);
        }
        else
        {
            Debug.Log($"{gameObject.name} ��ü�� �̹��� ������Ʈ ����");
        }
    }
}
