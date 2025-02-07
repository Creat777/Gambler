using System.Collections.Generic;
using UnityEngine;



public class Map_InGame : MonoBehaviour
{
    [SerializeField] private GameObject[] mapArr;

    public enum eMap
    {
        InsideOfHouse,
        OutsideOfHouse
    }

    Dictionary<eMap, GameObject> mapDict;

    private void Awake()
    {
        InitDict();
    }
    private void InitDict()
    {
        mapDict = new Dictionary<eMap, GameObject>();
        for(int i = 0; i < mapArr.Length; i++)
        {
            mapDict.Add((eMap)i, mapArr[i]);
        }
        
    }

    public GameObject[] Map
    { 
        get { return mapArr; } 
        private set { mapArr = value; }
    }

    private void MapOnlyOneActive(eMap targetMap)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        mapDict[targetMap].SetActive(true);
        GameManager.Instance.currentMap = targetMap;
    }

    public void ChangeMapToOutSide()
    {
        // �� ����
        MapOnlyOneActive(eMap.OutsideOfHouse);
    }

    public void ChangeMapToInSide()
    {
        // �� ����
        MapOnlyOneActive(eMap.InsideOfHouse);
        
    }
}
