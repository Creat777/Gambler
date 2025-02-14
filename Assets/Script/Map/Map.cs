using System.Collections.Generic;
using UnityEngine;
using PublicSet;



public class Map : MonoBehaviour
{
    // �����ڿ� ���� ���� ����
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
        
        // �ٸ� �ʿ��� ī����� ���� ���
        if(targetMap == eMap.Casino)
        {
            if(GameManager.Connector.canvas.Length == 3)
            {
                GameManager.Connector.canvas[0].SetActive(false);
                GameManager.Connector.canvas[2].SetActive(false);
            }
            else
            {
                Debug.LogWarning("ĵ���� ����");
            }
        }

        //ī���뿡�� ������ ���
        else if(GameManager.Instance.currentMap == eMap.Casino && targetMap != eMap.Casino)
        {
            GameManager.Connector.canvas[0].SetActive(true);
            GameManager.Connector.canvas[2].SetActive(true);
        }

        GameManager.Instance.currentMap = targetMap;

        Debug.Log("�� ���� �Ϸ�");
    }
}
