using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PublicSet
{
    //ENUM
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
        Bed,
        Cabinet,
        Clock,
        Computer,
        InsideDoor,
        Box_Empty,
        Box_Full,
        OutsideDoor,
        NPC_MunDuckBea_Acquaintance,
        NPC_MunDuckBea_Encounter,
        PlayerTutorial,

        None,
    }

    //public enum eCsvFile_PlayerMono
    //{
    //    PlayerTutorial,

    //    None,
    //}

    public enum eItemSerialNumber
    {
        None,
        TutorialQuest = 101
    }

    public enum eItemCallback
    {
        None,
        TutorialStart
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

    //Class
    public class eTextScriptInfo
    {

        public string speaker { get; set; }
        public string script { get; set; }
        public eSelection eSelect { get; set; }
        public List<string> selection { get; set; }
        public List<UnityAction> callback { get; set; }

        public eTextScriptInfo()
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
}
