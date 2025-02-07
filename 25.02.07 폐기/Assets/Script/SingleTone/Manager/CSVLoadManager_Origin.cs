using System;
using System.Collections.Generic;
using UnityEngine;

public class CSVLoadManager_Origin : MonoBehaviour
{
    public class ItemInfo
    {
        public int item_number;
        public int ability;
    }

    public class MonsterInfo
    {
        public int id;
        public string name;
        public int attack;
        public int defence;
        public int HP;
        public int dropItem;
    }

    public class QuestInfo
    {

    }

    private List<List<string>> csvData = new List<List<string>>();

    private List<ItemInfo> itemInfo = new List<ItemInfo>();         // 아이템 정보
    private List<MonsterInfo> monsterInfo = new List<MonsterInfo>();// 몬스터 정보
    private List<QuestInfo> questInfo = new List<QuestInfo>();      // 퀘스트 정보

    private void Awake()
    {
        // 아이템 정보 읽어오기
        LoadItemCsv();
        // 몬스터 정보 읽어오기
        LoadMonsterCsv();
        // 퀘스트 정보 읽어오기
        LoadQuestCsv();
        
    }

    public List<ItemInfo> GetItemList()
    {
        return itemInfo;
    }

    public List<MonsterInfo> GetMonsterList()
    {
        return monsterInfo;
    }

    void LoadItemCsv()
    {
        // List<ItemInfo> itemInfo 를 List<T> dataList로 전달하면서 ItemInfo를 T에 전달
        // == LoadCsv<ItemInfo>
        LoadCsv("Item", itemInfo, 
            (row, itemInfo) =>
        {
            if (itemInfo == null) return;

            int field_num = 0;
            foreach (string field in row)
            {
                Debug.Log("field : " + field);
                switch (field_num)
                {
                    case 0: itemInfo.item_number = int.Parse(field); break;
                    case 1: break; // 이름
                    case 2: itemInfo.ability = int.Parse(field); break;
                }
                field_num++;
            }
        });

        
    }

    void LoadMonsterCsv()
    {
        LoadCsv("Monster", monsterInfo, 
            (row, monsterInfo) =>
        {
            //MonsterInfo monster = info as MonsterInfo;
            if (monsterInfo == null) return;

            int field_num = 0;
            foreach (string field in row)
            {
                Debug.Log("field : " + field);
                switch (field_num)
                {
                    // 필요한 데이터 파싱 추가
                    case 0: monsterInfo.id = int.Parse(field); break;
                    case 1: monsterInfo.name = field; break;
                    case 2: monsterInfo.attack = int.Parse(field); break;
                    case 3: monsterInfo.defence = int.Parse(field); break;
                    case 4: monsterInfo.HP = int.Parse(field); break;
                    case 5: monsterInfo.dropItem = int.Parse(field); break;
                }
                field_num++;
            }
        });
    }

    void LoadQuestCsv()
    {
        csvData.Clear();
    }

    void LoadCsv<T>(string resourceName, List<T> dataList, Action<List<string>, T> processRow) where T : new()
        // where T : new() : 제네릭 타입 T가 매개변수 없는 기본 생성자를 가진 클래스여야 한다는 조건을 의미
    {
        csvData.Clear();

        TextAsset csvFile = Resources.Load<TextAsset>(resourceName);
        if (csvFile != null)
        {
            Debug.Log($"{resourceName} 파일이 존재합니다.");
            string[] rows = csvFile.text.Split('\n');

            foreach (string row in rows)
            {
                string[] fields = row.Split(',');
                List<string> rowData = new List<string>(fields);
                csvData.Add(rowData);
            }

            int row_num = 0;
            foreach (List<string> row in csvData)
            {
                if (row_num == 0) // 첫 번째 행(헤더) 스킵
                {
                    row_num++;
                    continue;
                }

                Debug.Log($"[{row_num}]");
                T info = new T();

                processRow(row, info); // 전달된 델리게이트 실행

                dataList.Add(info);
                row_num++;
            }
        }
        else
        {
            Debug.Log($"{resourceName} 파일이 존재하지 않습니다.");
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
