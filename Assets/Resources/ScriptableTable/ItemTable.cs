using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  ����ȭ(serialization)�� ��ü�� ������ �������� ��ȯ�ϴ� ����
///  ����Ƽ������ [System.Serializable]�� ����Ͽ� Ŀ���� Ŭ������ ����ü�� �ν����� â�� ǥ���� �� �ִ�. 
///  �̸� ����, Unity �����Ϳ��� ���� ������ ������ �� �ְ� �ȴ�.
/// </summary>
[System.Serializable]
public class ItemInfo
{
    public int SerialNumber;
    public string Name;
    public float value;
}

[CreateAssetMenu(fileName = "ItemTable", menuName = "Scriptable Objects/ItemTable")]
public class ItemTable : ScriptableObject
{
    public List<ItemInfo> enemy_info = new List<ItemInfo>();
}
