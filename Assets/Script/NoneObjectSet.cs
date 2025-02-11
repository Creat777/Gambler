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
        OutsideOfHouse
    }

    public enum eStage
    {
        None,
        Stage1,
        Stage2
    }

    public enum eCsvFile_InterObj
    {
        Bed,
        Cabinet,
        Clock,
        Computer,
        InsideDoor,
        Box_Empty,
        Box_Full,
        OutsideDoor,
        NPC_MunDuckBea,

        None,
    }

    public enum eCsvFile_PlayerMono
    {
        PlayerTutorial,

        None,
    }

    public enum eSelection
    {
        NoneExist,
        Exist
    }

    public enum eTextType
    {
        Interaction,
        PlayerMonologue
    }

    //Class
    public class cIteractableInfoList
    {

        public string speaker { get; set; }
        public string script { get; set; }
        public eSelection eSelect { get; set; }
        public List<string> selection { get; set; }
        public List<UnityAction> callback { get; set; }

        public cIteractableInfoList()
        {
            selection = new List<string>();
            callback = new List<UnityAction>();
        }

    }

    public class cPlayerMonologueInfoList
    {
        public string speaker { get; set; }
        public string script { get; set; }

    }

    public class ItemInfo
    {

    }
}
