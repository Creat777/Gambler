using PublicSet;
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class ImageResourceBase<T_Class,T_Enum> : Singleton<T_Class>
    where T_Class : MonoBehaviour
    where T_Enum : Enum
{
    // 기본 이미지
    [SerializeField] protected Sprite defaultImage;

    public Dictionary<T_Enum, Sprite[]> imageDict;

    protected override void Awake()
    {
        base.Awake();
        InitImageDict();
    }

    protected abstract void InitImageDict();

    // 여러 이미지가 있는 경우
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
                Debug.LogAssertion($"{imageIndex}는 정의되지 않은 이미지 인덱스");
                return null;
            }
        }
        else
        {
            result = false;
            Debug.LogAssertion($"{imageEnum}가 딕셔너리에 추가되지 않았습니다.");
            return null;
        }
    }

    // 이미지가 하나만 있는 경우
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
                Debug.LogAssertion($"{0}는 정의되지 않은 이미지 인덱스입니다.");
                result = false;
                return null;
            }
        }
        else
        {
            Debug.LogAssertion($"{imageEnum.ToString()}가 딕셔너리에 추가되지 않았습니다.");
            result = false;
            return null;
        }
    }
}
