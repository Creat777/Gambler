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
        CasinoDealer,

        // ����
        PlayerMonologue,

        None,
    }

    public enum eItemSerialNumber
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
        // �⺻����
        public eItemSerialNumber serialNumber { get; set; }
        public string name { get; set; }
        public string description { get; set; }

        // ����� ������ ��� 
        public bool isAvailable { get; set; }
        public bool isConsumable { get; set; } // �Ҹ� ������ ����
        public float value_Use { get; set; } // ��� �����ҽ� ������ ��

        // �ǸŰ� ������ ���
        public bool isForSale { get; set; }
        public int value_Sale { get; set; }

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
    }
}
