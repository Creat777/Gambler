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
        Defualt, // 스테이지 상관없이 실행
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
        // 실내
        Bed,
        Cabinet,
        Clock,
        Computer,
        InsideDoor,

        // 야외
        Box_Empty,
        Box_Full,
        OutsideDoor,
        NPC_MunDuckBea_Acquaintance,
        NPC_MunDuckBea_Encounter,
        
        // 카지노
        NPC_MunDuckBea_InCasino,
        NPC_Caesar,
        CasinoDoor,

        // -----------None Interactable--------------
        // 플레이어 혼잣말
        PlayerMonologue,

        // 카지노
        GameMaster,
    }

    /// <summary>
    /// == onlyOneLivesProgress
    /// </summary>
    public enum eOOLProgress
    {
        num101_BeforeStartGame = 101, // 게임에 대한 기본적인 설명을 하는 단계
        num102_BeforeRotateDiceAndDistribution, // 주사위를 던지고 카드를 분배하는 단계
        num103_BeforeChooseCardsToReveal, // 각 플레이어가 공개할 카드를 선택하는 단계
        num104_OnChooseFirstPlayer, // 게임에서 첫 공격을 실행할 플레이어를 선택하는 단계

        // 공격차례, computer의 경우 AttackTurn_Player를 스킵
        num201_AttackTurnPlayer = 201,
        num202_Attack,

        // 수비차례, computer의 경우 DefenseTrun_Player를 스킵
        num301_DefenseTrun_Player= 301,
        num302_Defense,


        // 공격, 방어를 진행한 후 카드를 동시에 오픈
        num401_CardOpenAtTheSameTime = 401, 

        // 결과 발표
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

        // 퀘스트 아이템
        TutorialQuest = 101,

        // 소모성 아이템
        Meat = 1001,
        Fish = 2001,
        Egg = 3001,

        // 기타 잡템
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
        // 기본정보
        public eItemType serialNumber { get; set; }
        public string name { get; set; }
        public string description { get; set; }

        // 사용이 가능한 경우 
        public bool isAvailable { get; set; }
        public bool isConsumable { get; set; } // 소모성 아이템 여부
        public float value_Use { get; set; } // 사용 가능할시 적용할 값

        // 판매가 가능한 경우
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

        // 스크립트에서 별도로 추가할 값들
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
