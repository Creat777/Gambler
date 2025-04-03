using System;
using System.Collections.Generic;
using UnityEngine;

public class CSVReader : MonoBehaviour
{
    public TextAsset csvFile; // Unity에서 CSV 파일을 지정

    void Start()
    {
        if (csvFile != null)
        {
            List<string> messages = LoadCSV(csvFile.text);
            foreach (var msg in messages)
            {
                Debug.Log(msg);
            }
        }
        else
        {
            Debug.LogError("CSV 파일이 지정되지 않았습니다.");
        }
    }

    List<string> LoadCSV(string csvText)
    {
        List<string> messages = new List<string>();
        string[] lines = csvText.Split('\n');

        for (int i = 1; i < lines.Length; i++) // 첫 줄(헤더) 제외
        {
            string[] values = lines[i].Split(',');

            if (values.Length >= 2)
            {
                string messageTemplate = values[1].Trim();
                string formattedMessage = FormatMessage(messageTemplate);
                messages.Add(formattedMessage);
            }
        }

        return messages;
    }

    string FormatMessage(string template)
    {
        string currentDate = DateTime.Now.ToString("yyyy-MM-dd"); // 현재 날짜 가져오기
        return string.Format(template, currentDate); // {0}을 날짜로 치환

        // 변수를 계속 추가하면 2번째 인수->{0}을 치환, 3번째 인수 -> {1}을 치환
    }
}