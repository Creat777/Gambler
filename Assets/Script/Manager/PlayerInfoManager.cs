using UnityEngine;
using System.Collections.Generic;

public class PlayerInfoManager : MonoBehaviour
{
    public static PlayerInfoManager Instance { get; private set; }

    private void MakeSingleTone()
    {
        if(Instance == null)
        { 
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
    private void Awake()
    {
        MakeSingleTone();
    }

    public void SaveData(string key, object value)
    {
        // 해당 key 리셋
        //PlayerPrefs.DeleteKey(key); 

        // 해당 key에 Value를 저장
        // 같은 키에 서로다른 자료형을 저장하는 경우 마지막에 저장된 데이터만 유효함
        if (value is int)
        {
            PlayerPrefs.SetInt(key, (int)value);
        }
        else if (value is float)
        {
            PlayerPrefs.SetFloat(key, (float)value);
        }
        else if (value is string)
        {
            PlayerPrefs.SetString(key, (string)value);
        }
        else
        {
            Debug.LogError("지원되지 않는 데이터 타입입니다.");
            return;
        }

        PlayerPrefs.Save();
        //Debug.Log($"Data saved : {key} = {value}");
    }

    public int LoadData(string key, int defaultValue)
    {
        return PlayerPrefs.GetInt(key, defaultValue);
    }

    public float LoadData(string key, float defaultValue)
    {
        return PlayerPrefs.GetFloat(key, defaultValue);
    }

    public string LoadData(string key, string defaultValue)
    {
        return PlayerPrefs.GetString(key, defaultValue);
    }

    // 특정 키에 해당하는 데이터를 삭제하는 함수
    public void DeleteData(string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            PlayerPrefs.DeleteKey(key);
            Debug.Log($"Data Deleted : {key}");
        }
        else
        {
            Debug.Log($"No data found for key : {key}");
        }
    }

    // 모든 데이터를 삭제하는 함수
    public void ResetAllData()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("Succes : Reset all data");
    }

    public void ResetAllRank(string[] RankKeys)
    {
        foreach(var key in RankKeys)
        {
            DeleteData(key);
        }
    }

    public void SetNewRank(string[] RankKeys, string[] RankersID)
    {
        Debug.LogWarning("RankKeys의 크기 : "+RankKeys.Length);
        Debug.LogWarning("scoreValueList 크기 : " + RankersID.Length);
        for (int i = 0; i < RankKeys.Length; i++)
        {
            // 범위 내에서만 저장
            if (i<RankersID.Length) 
                SaveData(RankKeys[i], RankersID[i]);
        }
    }

    public float[] GetRankScores(string[] RankersIDs)
    {
        float[] rankScores = new float[RankersIDs.Length];
        for (int i = 0; i < RankersIDs.Length; i++)
        {
            rankScores[i] = LoadData(RankersIDs[i], 0f);
        }
        return rankScores;
    }

    public string[] GetRankersID(string[] RankKeys)
    {
        string[] PlayerNameOfRank = new string[RankKeys.Length];
        for(int i = 0;i < RankKeys.Length;i++)
        {
            // 로그인 없이 접속해서 플레이한 사례가 있으면 쓰레기값을 반환할 수도 있음
            // 같은 랭커키에 동일한 플레이어ID가 반환되면 이것을 키로 활용할 때 오류가 생김
            PlayerNameOfRank[i] = PlayerPrefs.GetString(RankKeys[i], $"ID_NonePlayer_{i}");
        }
        return PlayerNameOfRank; ;
        
    }
    void Start()
    {
        
    }
}
