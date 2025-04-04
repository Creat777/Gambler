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
        // ���� ����
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }

        // Ÿ�ٸ� Ȱ��ȭ
        mapDict[targetMap].SetActive(true);
        
        // ����� ���� gameManager�� ����
        GameManager.Instance.SetCurrentMap(targetMap);

        Debug.Log("�� ���� �Ϸ�");
    }
}
