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
        None,
        Stage1,
        Stage2,
        Stage3
    }

    public enum eTextScriptFile
    {
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
        CasinoDealer,

        // 독백
        PlayerMonologue,

        None,
    }

    public enum eItemSerialNumber
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
        TutorialStart,
        EatMeal
    }

    public enum eSelection
    {
        NoneExist,
        Exist
    }


    public enum eIcon
    {
        Inventory,
        Quest,
        Status,
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
        Diamonds,
        Hearts
    }

    //Class
    public class cTextScriptInfo
    {

        public string speaker { get; set; }
        public string script { get; set; }
        public eSelection eSelect { get; set; }
        public List<string> selection { get; set; }
        public List<UnityAction> callback { get; set; }

        public cTextScriptInfo()
        {
            selection = new List<string>();
            callback = new List<UnityAction>();
        }
    }

    //public class cPlayerMonologueInfo
    //{
    //    public string speaker { get; set; }
    //    public string script { get; set; }
    //}

    public class cItemInfo
    {
        // 기본정보
        public eItemSerialNumber serialNumber { get; set; }
        public string name { get; set; }
        public string description { get; set; }

        // 사용이 가능한 경우 
        public bool isAvailable { get; set; }
        public bool isConsumable { get; set; } // 소모성 아이템 여부
        public float value_Use { get; set; } // 사용 가능할시 적용할 값

        // 판매가 가능한 경우
        public bool isForSale { get; set; }
        public int value_Sale { get; set; }

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
    }
}
