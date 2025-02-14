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
        
        // 다른 맵에서 카지노로 들어가는 경우
        if(targetMap == eMap.Casino)
        {
            if(GameManager.Connector.canvas.Length == 3)
            {
                GameManager.Connector.canvas[0].SetActive(false);
                GameManager.Connector.canvas[2].SetActive(false);
            }
            else
            {
                Debug.LogWarning("캔버스 오류");
            }
        }

        //카지노에서 나오는 경우
        else if(GameManager.Instance.currentMap == eMap.Casino && targetMap != eMap.Casino)
        {
            GameManager.Connector.canvas[0].SetActive(true);
            GameManager.Connector.canvas[2].SetActive(true);
        }

        GameManager.Instance.currentMap = targetMap;

        Debug.Log("맵 변경 완료");
    }
}
