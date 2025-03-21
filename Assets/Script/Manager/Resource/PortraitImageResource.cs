using UnityEngine;
using System.Collections.Generic;
using PublicSet;
using System;


public class PortraitImageResource : ImageResourceBase<PortraitImageResource,eCharacterType>
{

    // 일반 캐릭터
    [SerializeField] private Sprite[] sPlayer;
    [SerializeField] private Sprite[] sMunDuckBea;
    [SerializeField] private Sprite[] sCaesar;

    // 도박 캐릭터
    [SerializeField] private Sprite[] sCasinoDealer;
    [SerializeField] private Sprite[] sKangDoYun;
    [SerializeField] private Sprite[] sSeoJiHoo;
    [SerializeField] private Sprite[] sLeeHaRin;
    [SerializeField] private Sprite[] sChoiGeonWoo;
    [SerializeField] private Sprite[] sYoonChaeYoung;
    [SerializeField] private Sprite[] sParkMinSeok;
    [SerializeField] private Sprite[] sJangSeoYoon;
    [SerializeField] private Sprite[] sOhJinSoo;


    protected override void InitImageDict()
    {
        imageDict = new Dictionary<eCharacterType, Sprite[]>();

        imageDict.Add(eCharacterType.Player, sPlayer);
        imageDict.Add(eCharacterType.MunDuckBea, sMunDuckBea);
        imageDict.Add(eCharacterType.Caesar, sCaesar);

        imageDict.Add(eCharacterType.CasinoDealer, sCasinoDealer);
        imageDict.Add(eCharacterType.KangDoYun, sKangDoYun);
        imageDict.Add(eCharacterType.SeoJiHoo, sSeoJiHoo);
        imageDict.Add(eCharacterType.LeeHaRin, sLeeHaRin);
        imageDict.Add(eCharacterType.ChoiGeonWoo, sChoiGeonWoo);
        imageDict.Add(eCharacterType.YoonChaeYoung, sYoonChaeYoung);
        imageDict.Add(eCharacterType.ParkMinSeok, sParkMinSeok);
        imageDict.Add(eCharacterType.JangSeoYoon, sJangSeoYoon);
        imageDict.Add(eCharacterType.OhJinSoo, sOhJinSoo);
    }

    // 여러 이미지가 있는 경우
    public override Sprite TryGetImage(eCharacterType characterIndex, int portraitIndex, out bool result)
    {
        if (imageDict.ContainsKey(characterIndex))
        {
            Sprite[] characterSprites = imageDict[(eCharacterType)characterIndex];
            if (MethodManager.Instance.IsIndexInRange(characterSprites, portraitIndex))
            {
                result = true;
                return characterSprites[portraitIndex];
            }
            else
            {
                result = false;
                Debug.LogAssertion($"{portraitIndex}는 정의되지 않은 초상화 인덱스");
                return null;
            }
        }
        else
        {
            result = false;
            Debug.LogAssertion($"{characterIndex}가 딕셔너리에 추가되지 않았습니다.");
            return null;
        }
    }
    
    // 이미지가 하나만 있는 경우
    public override Sprite TryGetImage(eCharacterType characterIndex, out bool result)
    {
        if (imageDict.ContainsKey(characterIndex))
        {
            Sprite[] characterSprites = imageDict[characterIndex];
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
            if(characterIndex < eCharacterType.Player) // 시스템, 나레이션, GM, ???의 경우
            {
                Debug.Log("이미지를 기본값으로 합니다.");
                result = true;
                return defaultImage;
            }
            else
            {
                Debug.LogAssertion($"{characterIndex.ToString()}가 딕셔너리에 추가되지 않았습니다.");
                result = false;
                return null;
            }
            
        }
    }
}
