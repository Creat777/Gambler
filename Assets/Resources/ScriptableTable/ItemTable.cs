using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

/// <summary>
///  ����ȭ(serialization)�� ��ü�� ������ �������� ��ȯ�ϴ� ����
///  ����Ƽ������ [System.Serializable]�� ����Ͽ� Ŀ���� Ŭ������ ����ü�� �ν����� â�� ǥ���� �� �ִ�. 
///  �̸� ����, Unity �����Ϳ��� ���� ������ ������ �� �ְ� �ȴ�.
/// </summary>
[System.Serializable]
public class CsvToPrefabs
{
    // ������ ���� ��ȣ
    public int SerialNumber;

    // ������ �̸�
    public string Name;

    // �κ��丮���� ���� ������ ������
    [SerializeField] private GameObject itemPrefab;
    public GameObject __itemPrefab { get { return itemPrefab; } private set { itemPrefab = value; } }


    // �ο��� ��
    public float value;

    // ����
    // internal: ���� ����� ������ ���� ����
    [SerializeField] internal string Description;
}

[CreateAssetMenu(fileName = "ItemTable", menuName = "Scriptable Objects/ItemTable")]
public class ItemTable : ScriptableObject
{
    public List<CsvToPrefabs> itemInfoList = new List<CsvToPrefabs>();
}
