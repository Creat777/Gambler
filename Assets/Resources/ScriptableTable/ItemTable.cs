using PublicSet;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

/// <summary>
///  직렬화(serialization)는 객체를 데이터 형식으로 변환하는 과정
///  유니티에서는 [System.Serializable]을 사용하여 커스텀 클래스나 구조체를 인스펙터 창에 표시할 수 있다. 
///  이를 통해, Unity 에디터에서 직접 값들을 수정할 수 있게 된다.
/// </summary>
[System.Serializable]
public class ItemPlusInfo
{
    // 아이템 고유 번호
    public eItemSerialNumber serialNumber;

    // 아이템 프리팹
    public GameObject itemPrefab;

    // 아이템 사용 클릭시 처리될 콜백함수의 번호
    public eItemCallback itemCallbackIndex;

}

[CreateAssetMenu(fileName = "ItemTable", menuName = "Scriptable Objects/ItemTable")]
public class ItemTable : ScriptableObject
{
    public List<ItemPlusInfo> item_PlusInfoList = new List<ItemPlusInfo>();
}
