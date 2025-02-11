using System.Collections.Generic;
using UnityEngine;
using PublicSet;



public class Map : MonoBehaviour
{
    // 열거자와 실제 맵을 맵핑
    Dictionary<eMap, GameObject> mapDict;

    private void Awake()
    {
        InitDict();
    }
    private void InitDict()
    {
        mapDict = new Dictionary<eMap, GameObject>();
        
        for (int i = 0; i < transform.childCount; i++)
        {
            mapDict.Add((eMap)i, transform.GetChild(i).gameObject);
        }
        
    }

    public void ChangeMapTo(eMap targetMap)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }

        mapDict[targetMap].SetActive(true);
        GameManager.Instance.currentMap = targetMap;

        //Debug.Log("맵 변경 완료");
    }
}
