using UnityEngine;
using System.Collections.Generic;
using PublicSet;
using System;


public class PortraitResource : Singleton<PortraitResource>
{
    // �Ϲ� ĳ����
    [SerializeField] private Sprite[] sPlayer;
    [SerializeField] private Sprite[] sMunDuckBea;
    [SerializeField] private Sprite[] sCaesar;

    // ���� ĳ����
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

    // ���� �̹����� �ִ� ���
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
                Debug.LogAssertion($"{portraitIndex}�� ���ǵ��� ���� �ʻ�ȭ �ε���");
                return null;
            }
        }
        else
        {
            Debug.LogAssertion($"{characterIndex}�� ��ųʸ��� �߰����� �ʾҽ��ϴ�.");
            return null;
        }
    }
    
    // �̹����� �ϳ��� �ִ� ���
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
                Debug.LogAssertion($"{0}�� ���ǵ��� ���� �ʻ�ȭ �ε����Դϴ�.");
                result = false;
                return null;
            }
        }
        else
        {
            Debug.LogAssertion($"{characterIndex}�� ��ųʸ��� �߰����� �ʾҽ��ϴ�.");
            result = false;
            return null;
        }
    }


}
