using System;
using System.Collections.Generic;
using UnityEngine;

public class CSVReader : MonoBehaviour
{
    public TextAsset csvFile; // Unity���� CSV ������ ����

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
            Debug.LogError("CSV ������ �������� �ʾҽ��ϴ�.");
        }
    }

    List<string> LoadCSV(string csvText)
    {
        List<string> messages = new List<string>();
        string[] lines = csvText.Split('\n');

        for (int i = 1; i < lines.Length; i++) // ù ��(���) ����
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
        string currentDate = DateTime.Now.ToString("yyyy-MM-dd"); // ���� ��¥ ��������
        return string.Format(template, currentDate); // {0}�� ��¥�� ġȯ

        // ������ ��� �߰��ϸ� 2��° �μ�->{0}�� ġȯ, 3��° �μ� -> {1}�� ġȯ
    }
}