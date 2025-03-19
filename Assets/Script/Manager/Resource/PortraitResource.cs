using UnityEngine;
using System.Collections.Generic;
using PublicSet;
using System;


public class PortraitResource : Singleton<PortraitResource>
{
    // 일반 캐릭터
    [SerializeField] private Sprite[] sPlayer;
    [SerializeField] private Sprite[] sMunDuckBea;
    [SerializeField] private Sprite[] sCaesar;

    // 도박 캐릭터
    [SerializeField] private Sprite[] sKangDoYun;
    [SerializeField] private Sprite[] sSeoJiHoo;
    [SerializeField] private Sprite[] sLeeHaRin;
    [SerializeField] private Sprite[] sChoiGeonWoo;
    [SerializeField] private Sprite[] sYoonChaeYoung;
    [SerializeField] private Sprite[] sParkMinSeok;
    [SerializeField] private Sprite[] sJangSeoYoon;
    [SerializeField] private Sprite[] sOhJinSoo;

    public Dictionary<eCharacter, Sprite[]> portraitsDict;

    protected override void Awake()
    {
        base.Awake();
        InitPortraitsDict();
    }

    private void InitPortraitsDict()
    {
        portraitsDict = new Dictionary<eCharacter, Sprite[]>();

        portraitsDict.Add(eCharacter.Player, sPlayer);
        portraitsDict.Add(eCharacter.MunDuckBea, sMunDuckBea);
        portraitsDict.Add(eCharacter.Caesar, sCaesar);

        portraitsDict.Add(eCharacter.KangDoYun, sKangDoYun);
        portraitsDict.Add(eCharacter.SeoJiHoo, sSeoJiHoo);
        portraitsDict.Add(eCharacter.LeeHaRin, sLeeHaRin);
        portraitsDict.Add(eCharacter.ChoiGeonWoo, sChoiGeonWoo);
        portraitsDict.Add(eCharacter.YoonChaeYoung, sYoonChaeYoung);
        portraitsDict.Add(eCharacter.ParkMinSeok, sParkMinSeok);
        portraitsDict.Add(eCharacter.JangSeoYoon, sJangSeoYoon);
        portraitsDict.Add(eCharacter.OhJinSoo, sOhJinSoo);
    }

    // 여러 이미지가 있는 경우
    public Sprite GetPortraitImage(eCharacter characterIndex, int portraitIndex)
    {
        if (portraitsDict.ContainsKey(characterIndex))
        {
            Sprite[] characterSprites = portraitsDict[(eCharacter)characterIndex];
            if (MethodManager.Instance.IsIndexInRange(characterSprites, portraitIndex))
            {
                return characterSprites[portraitIndex];
            }
            else
            {
                Debug.LogAssertion($"{portraitIndex}는 정의되지 않은 초상화 인덱스");
                return null;
            }
        }
        else
        {
            Debug.LogAssertion($"{characterIndex}가 딕셔너리에 추가되지 않았습니다.");
            return null;
        }
    }
    
    // 이미지가 하나만 있는 경우
    public Sprite TryGetPortraitImage(eCharacter characterIndex, out bool result)
    {
        if (portraitsDict.ContainsKey(characterIndex))
        {
            Sprite[] characterSprites = portraitsDict[characterIndex];
            if (MethodManager.Instance.IsIndexInRange(characterSprites, 0))
            {
                result = true;
                return characterSprites[0];
            }
            else
            {
                Debug.LogAssertion($"{0}는 정의되지 않은 초상화 인덱스입니다.");
                result = false;
                return null;
            }
        }
        else
        {
            Debug.LogAssertion($"{characterIndex}가 딕셔너리에 추가되지 않았습니다.");
            result = false;
            return null;
        }
    }


}
