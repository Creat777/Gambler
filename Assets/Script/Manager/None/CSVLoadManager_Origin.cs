//using System;
//using System.Collections.Generic;
//using UnityEngine;

//public class CSVLoadManager_Origin : MonoBehaviour
//{
//    public class ItemInfo
//    {
//        public int item_number;
//        public int ability;
//    }

//    public class MonsterInfo
//    {
//        public int id;
//        public string name;
//        public int attack;
//        public int defence;
//        public int HP;
//        public int dropItem;
//    }

//    public class QuestInfo
//    {

//    }

//    private List<List<string>> csvData = new List<List<string>>();

//    private List<ItemInfo> itemInfo = new List<ItemInfo>();         // ������ ����
//    private List<MonsterInfo> monsterInfo = new List<MonsterInfo>();// ���� ����
//    private List<QuestInfo> questInfo = new List<QuestInfo>();      // ����Ʈ ����

//    private void Awake()
//    {
//        // ������ ���� �о����
//        LoadItemCsv();
//        // ���� ���� �о����
//        LoadMonsterCsv();
//        // ����Ʈ ���� �о����
//        LoadQuestCsv();
//    }

//    public List<ItemInfo> GetItemList()
//    {
//        return itemInfo;
//    }

//    public List<MonsterInfo> GetMonsterList()
//    {
//        return monsterInfo;
//    }

//    void LoadItemCsv()
//    {
//        // List<CsvToPrefabs> itemInfo �� List<T> dataList�� �����ϸ鼭 ItemInfo�� T�� ����
//        // == LoadCsv<CsvToPrefabs>
//        LoadCsv("Item", itemInfo, 
//            (row, itemInfo) =>
//        {
//            if (itemInfo == null) return;

//            int field_num = 0;
//            foreach (string field in row)
//            {
//                Debug.Log("field : " + field);
//                switch (field_num)
//                {
//                    case 0: itemInfo.item_number = int.Parse(field); break;
//                    case 1: break; // �̸�
//                    case 2: itemInfo.ability = int.Parse(field); break;
//                }
//                field_num++;
//            }
//        });

//        /*
//        csvData.Clear();

//        TextAsset csvFile = Resources.Load<TextAsset>("Item");
//        if (csvFile != null)
//        {
//            Debug.Log("������ �����մϴ�.");

//            string[] rows = csvFile.text.Split('\n');

//            foreach (string row in rows)
//            {
//                string[] fields = row.Split(',');
//                List<string> rowData = new List<string>(fields);
//                csvData.Add(rowData);
//            }

//            int row_num = 0;
//            foreach (List<string> row in csvData)
//            {
//                if(row_num == 0)
//                {
//                    row_num++;
//                    continue;
//                }

//                Debug.Log("[" + row_num + "]");
//                CsvToPrefabs itemInfo = new CsvToPrefabs();

//                int field_num = 0;
//                foreach (string field in row)
//                {
//                    Debug.Log("field : " + field);
//                    switch (field_num)
//                    {
//                        case 0: itemInfo.item_number = int.Parse(field); break;
//                        case 1: break; //�̸�
//                        case 2: itemInfo.ability = int.Parse(field); break;

//                    }
//                    field_num++;
//                }

//                itemInfo.Add(itemInfo);
//                row_num++;
//            }
//        }
//        else
//        {
//            Debug.Log("������ �������� �ʽ��ϴ�.");
//        }
//        */
//    }

//    void LoadMonsterCsv()
//    {
//        LoadCsv("Monster", monsterInfo, 
//            (row, monsterInfo) =>
//        {
//            //MonsterInfo monster = info as MonsterInfo;
//            if (monsterInfo == null) return;

//            int field_num = 0;
//            foreach (string field in row)
//            {
//                Debug.Log("field : " + field);
//                switch (field_num)
//                {
//                    // �ʿ��� ������ �Ľ� �߰�
//                    case 0: monsterInfo.id = int.Parse(field); break;
//                    case 1: monsterInfo.name = field; break;
//                    case 2: monsterInfo.attack = int.Parse(field); break;
//                    case 3: monsterInfo.defence = int.Parse(field); break;
//                    case 4: monsterInfo.HP = int.Parse(field); break;
//                    case 5: monsterInfo.dropItem = int.Parse(field); break;
//                }
//                field_num++;
//            }
//        });
//    }

//    void LoadQuestCsv()
//    {
//        csvData.Clear();
//    }

//    void LoadCsv<T>(string resourceName, List<T> dataList, Action<List<string>, T> processRow) where T : new()
//        // where T : new() : ���׸� Ÿ�� T�� �Ű����� ���� �⺻ �����ڸ� ���� Ŭ�������� �Ѵٴ� ������ �ǹ�
//    {
//        csvData.Clear();

//        TextAsset csvFile = Resources.Load<TextAsset>(resourceName);
//        if (csvFile != null)
//        {
//            Debug.Log($"{resourceName} ������ �����մϴ�.");
//            string[] rows = csvFile.text.Split('\n');

//            foreach (string row in rows)
//            {
//                string[] fields = row.Split(',');
//                List<string> rowData = new List<string>(fields);
//                csvData.Add(rowData);
//            }

//            int row_num = 0;
//            foreach (List<string> row in csvData)
//            {
//                if (row_num == 0) // ù ��° ��(���) ��ŵ
//                {
//                    row_num++;
//                    continue;
//                }

//                Debug.Log($"[{row_num}]");
//                T info = new T();

//                processRow(row, info); // ���޵� ������ ����

//                dataList.Add(info);
//                row_num++;
//            }
//        }
//        else
//        {
//            Debug.Log($"{resourceName} ������ �������� �ʽ��ϴ�.");
//        }
//    }

//    void Start()
//    {
        
//    }

//    // Update is called once per frame
//    void Update()
//    {
        
//    }
//}
