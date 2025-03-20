using PublicSet;
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class ImageResourceBase<T_Class,T_Enum> : Singleton<T_Class>
    where T_Class : MonoBehaviour
    where T_Enum : Enum
{
    // �⺻ �̹���
    [SerializeField] protected Sprite defaultImage;

    public Dictionary<T_Enum, Sprite[]> imageDict;

    protected override void Awake()
    {
        base.Awake();
        InitImageDict();
    }

    protected abstract void InitImageDict();

    // ���� �̹����� �ִ� ���
    public virtual Sprite TryGetImage(T_Enum imageEnum, int imageIndex, out bool result) 
    {
        if (imageDict.ContainsKey(imageEnum))
        {
            Sprite[] characterSprites = imageDict[(T_Enum)imageEnum];
            if (MethodManager.Instance.IsIndexInRange(characterSprites, imageIndex))
            {
                result = true;
                return characterSprites[imageIndex];
            }
            else
            {
                result = false;
                Debug.LogAssertion($"{imageIndex}�� ���ǵ��� ���� �̹��� �ε���");
                return null;
            }
        }
        else
        {
            result = false;
            Debug.LogAssertion($"{imageEnum}�� ��ųʸ��� �߰����� �ʾҽ��ϴ�.");
            return null;
        }
    }

    // �̹����� �ϳ��� �ִ� ���
    public virtual Sprite TryGetImage(T_Enum imageEnum, out bool result)
    {
        if (imageDict.ContainsKey(imageEnum))
        {
            Sprite[] characterSprites = imageDict[imageEnum];
            if (MethodManager.Instance.IsIndexInRange(characterSprites, 0))
            {
                result = true;
                return characterSprites[0];
            }
            else
            {
                Debug.LogAssertion($"{0}�� ���ǵ��� ���� �̹��� �ε����Դϴ�.");
                result = false;
                return null;
            }
        }
        else
        {
            Debug.LogAssertion($"{imageEnum.ToString()}�� ��ųʸ��� �߰����� �ʾҽ��ϴ�.");
            result = false;
            return null;
        }
    }
}
