using UnityEngine;
using System.Collections.Generic;
using PublicSet;
using System;


public class PortraitImageResource : ImageResourceBase<PortraitImageResource,eCharacterType>
{
    // �Ϲ� ĳ����
    [SerializeField] private Sprite[] sPlayer;
    [SerializeField] private Sprite[] sMunDuckBea;
    [SerializeField] private Sprite[] sCaesar;

    // ���� ĳ����
    [SerializeField] private Sprite[] sCasinoDealer; // 0�� idle, 2�� funny, 3�� big funny, 4�� surprised
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

    // ���� �̹����� �ִ� ���
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
                Debug.LogAssertion($"{portraitIndex}�� ���ǵ��� ���� �ʻ�ȭ �ε���");
                return null;
            }
        }
        else
        {
            if (characterIndex < eCharacterType.Player) // �ý���, �����̼�, GM, ???�� ���
            {
                Debug.Log("�̹����� �⺻������ �մϴ�.");
                result = true;
                return defaultImage;
            }
            else
            {
                Debug.LogAssertion($"{characterIndex.ToString()}�� ��ųʸ��� �߰����� �ʾҽ��ϴ�.");
                result = false;
                return null;
            }
            
        }
    }
    
    // �̹����� �ϳ��� �ִ� ���
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
                Debug.LogAssertion($"{0}�� ���ǵ��� ���� �ʻ�ȭ �ε����Դϴ�.");
                result = false;
                return null;
            }
        }
        else
        {
            if(characterIndex < eCharacterType.Player) // �ý���, �����̼�, GM, ???�� ���
            {
                Debug.Log("�̹����� �⺻������ �մϴ�.");
                result = true;
                return defaultImage;
            }
            else
            {
                Debug.LogAssertion($"{characterIndex.ToString()}�� ��ųʸ��� �߰����� �ʾҽ��ϴ�.");
                result = false;
                return null;
            }
            
        }
    }
}
