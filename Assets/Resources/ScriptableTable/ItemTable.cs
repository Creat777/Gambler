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
public class ItemInfo
{
    // 아이템 고유 번호
    public int SerialNumber;

    // 아이템 이름
    public string Name;

    // 인벤토리에서 쓰일 아이템 프리팹
    [SerializeField] private GameObject itemPrefab;
    public GameObject __itemPrefab { get { return itemPrefab; } private set { itemPrefab = value; } }


    // 부여된 값
    public float value;

    // 설명
    // internal: 같은 어셈블리 내에서 접근 가능
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
        // GUILayout.Height()를 사용하여 InputField 높이 설정
        ItemTable itemTable = (ItemTable)target;

        EditorGUILayout.LabelField("Description Input Field");

        // 이 부분에서 InputField의 높이를 조정하는 코드
        foreach (var itemInfo in itemTable.itemInfoList)
        {
            itemInfo.Description = EditorGUILayout.TextArea(itemInfo.Description, GUILayout.Height(40));
            GUILayout.Space(10);
        }

        // 기본적인 레이아웃 처리
        DrawDefaultInspector();
    }
}
