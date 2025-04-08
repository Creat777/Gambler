using PublicSet;
using System;
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
public class ItemPlusInfo
{
    // ������ ���� ��ȣ
    public eItemType type;

    // ������ ��� Ŭ���� ó���� �ݹ��Լ��� ��ȣ
    public eItemCallback itemCallbackIndex;

}

[CreateAssetMenu(fileName = "ItemTable", menuName = "Scriptable Objects/ItemTable")]
public class ItemTable : ScriptableObject
{
    public List<ItemPlusInfo> item_PlusInfoList = new List<ItemPlusInfo>();
}
