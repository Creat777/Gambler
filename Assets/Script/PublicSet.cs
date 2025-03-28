using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PublicSet
{
    //ENUM

    public enum ePlayerSaveKey
    {
        None,
        Player_1,
        Player_2,
        Player_3,
        Player_4,
    }

    public enum eScene
    {
        Title,
        Lobby,
        InGame
    }

    public enum eMap
    {
        InsideOfHouse,
        OutsideOfHouse,
        Casino,
        UnknownIsland
    }

    public enum eStage
    {
        Defualt, // �������� ������� ����
        Stage1,
        Stage2,
        Stage3,
    }

    public enum eTextType
    {
        TextScriptFile,
        OnlyOneLivesProgress
    }

    public enum eTextScriptFile
    {
        None,
        // --------------Interactable----------------
        // �ǳ�
        Bed,
        Cabinet,
        Clock,
        Computer,
        InsideDoor,

        // �߿�
        Box_Empty,
        Box_Full,
        OutsideDoor,
        NPC_MunDuckBea_Acquaintance,
        NPC_MunDuckBea_Encounter,
        
        // ī����
        NPC_MunDuckBea_InCasino,
        NPC_Caesar,
        CasinoDoor,

        // -----------None Interactable--------------
        // �÷��̾� ȥ�㸻
        PlayerMonologue,

        // ī����
        GameMaster,
    }

    /// <summary>
    /// == onlyOneLivesProgress
    /// </summary>
    public enum eOOLProgress
    {
        num101_BeforeStartGame = 101, // ���ӿ� ���� �⺻���� ������ �ϴ� �ܰ�
        num102_BeforeRotateDiceAndDistribution, // �ֻ����� ������ ī�带 �й��ϴ� �ܰ�
        num103_BeforeChooseCardsToReveal, // �� �÷��̾ ������ ī�带 �����ϴ� �ܰ�
        num104_OnChooseFirstPlayer, // ���ӿ��� ù ������ ������ �÷��̾ �����ϴ� �ܰ�

        // ��������, computer�� ��� AttackTurn_Player�� ��ŵ
        num201_AttackTurnPlayer = 201,
        num202_Attack,

        // ��������, computer�� ��� DefenseTrun_Player�� ��ŵ
        num301_DefenseTrun_Player= 301,
        num302_Defense,


        // ����, �� ������ �� ī�带 ���ÿ� ����
        num401_CardOpenAtTheSameTime = 401, 

        // ��� ��ǥ
        num402_OnJokerAppear,
        num403_OnAttackSuccess,
        num404_OnDefenceSuccess,

        num405_OnChooseNextPlayer,


        num501_final = 501
    }

    public enum eCriteria
    {
        None = 0,
        JokerWin,
        AttakkerWin,
        DeffenderWin
    }


    public enum eCharacterType
    {
        None,

        System = 101,
        Narration,
        GameMaster,
        Unknown = 404,

        Player = 1001,
        MunDuckBea,
        Caesar,

        CasinoDealer = 10001,
        KangDoYun,
        SeoJiHoo,
        LeeHaRin,
        ChoiGeonWoo,
        YoonChaeYoung,
        ParkMinSeok,
        JangSeoYoon,
        OhJinSoo
    }

    public enum eItemType
    {
        None,

        // ����Ʈ ������
        TutorialQuest = 101,

        // �Ҹ� ������
        Meat = 1001,
        Fish = 2001,
        Egg = 3001,

        // ��Ÿ ����
        Notice_Stage1 = 10001
    }

    public enum eItemCallback
    {
        None,
        FirstQuest,
        EatMeal
    }

    public enum eHasEndCallback
    {
        No,
        yes
    }
    public enum eHasSelection
    {
        No,
        yes
    }


    public enum eIcon
    {
        Quest,
        Inventory,
        GameAssistant,
        Message
    }

    public enum ePopUpState
    {
        Open,
        Close
    }

    public enum eCardType
    {
        Joker,
        Spades,
        Clubs,
        Hearts,
        Diamonds
    }

    //Class
    public class cTextScriptInfo
    {

        public eCharacterType characterEnum { get; set; }
        public int DialogueIconIndex { get; set; }
        public string script { get; set; }
        public eHasEndCallback hasEndCallback { get; set; }
        public UnityAction endCallback { get; set; }
        public eHasSelection hasSelection { get; set; }
        public List<string> selectionScript { get; set; }
        public List<UnityAction> SelectionCallback { get; set; }

        public cTextScriptInfo()
        {
            characterEnum = eCharacterType.None;
            hasEndCallback = eHasEndCallback.No;
            endCallback = null;
            hasSelection = eHasSelection.No;
            selectionScript = new List<string>();
            SelectionCallback = new List<UnityAction>();
        }
    }


    public class cItemInfo
    {
        // �⺻����
        public eItemType serialNumber { get; set; }
        public string name { get; set; }
        public string description { get; set; }

        // ����� ������ ��� 
        public bool isAvailable { get; set; }
        public bool isConsumable { get; set; } // �Ҹ� ������ ����
        public float value_Use { get; set; } // ��� �����ҽ� ������ ��

        // �ǸŰ� ������ ���
        public bool isForSale { get; set; }
        public int value_Sale { get; set; }

        public cItemInfo()
        {
            isAvailable = false;
            isConsumable = false;
            value_Use = 0;
            isForSale = false;
            value_Sale = 0;
        }

        // ��ũ��Ʈ���� ������ �߰��� ����
        public GameObject itemPrefab { get; set; }
        public UnityAction itemCallback { get; set; }

    }

    public class cTrumpCardInfo
    {
        public int cardIndex { get; set; }
        public string cardName { get; set; }
        public eCardType cardType { get; set; }
        public int cardValue { get; set; }

        public bool isFaceDown;
    }

    public class cCharacterInfo
    {
        public eCharacterType CharaterIndex { get; set; }
        public string CharacterName { get; set; }
        public string CharacterAge { get; set; }
        public string CharacterClan { get; set; }
        public string CharacterFeature { get; set; }
    }

}
