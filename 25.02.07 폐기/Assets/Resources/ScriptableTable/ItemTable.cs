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
public class ItemInfo
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
    public List<ItemInfo> itemInfoList = new List<ItemInfo>();
}

[CustomEditor(typeof(ItemTable))]
public class ItemTableEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // GUILayout.Height()�� ����Ͽ� InputField ���� ����
        ItemTable itemTable = (ItemTable)target;

        EditorGUILayout.LabelField("Description Input Field");

        // �� �κп��� InputField�� ���̸� �����ϴ� �ڵ�
        foreach (var itemInfo in itemTable.itemInfoList)
        {
            itemInfo.Description = EditorGUILayout.TextArea(itemInfo.Description, GUILayout.Height(40));
            GUILayout.Space(10);
        }

        // �⺻���� ���̾ƿ� ó��
        DrawDefaultInspector();
    }
}
