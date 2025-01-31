using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  직렬화(serialization)는 객체를 데이터 형식으로 변환하는 과정
///  유니티에서는 [System.Serializable]을 사용하여 커스텀 클래스나 구조체를 인스펙터 창에 표시할 수 있다. 
///  이를 통해, Unity 에디터에서 직접 값들을 수정할 수 있게 된다.
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
