using UnityEngine;
using UnityEngine.UI;
public class TextWindowView : MonoBehaviour
{
    // �����Ϳ��� ����
    public Text textWindow;
    CsvInfo csvInfo;
    private string[] TextSet;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // ���� ����
        string objectName = Player.Instance.hitObjectName;
        CsvInfo[] csvInfos = CsvManager.Instance.InteractiveCsvInfos;
        Interactive interactive = new Interactive();

        // �÷��̾ ��ȣ�ۿ��� �� �ִ� ��ü�� �ִ��� Ȯ��
        csvInfo = CsvProcessor.Instance.FindCsvInfo(objectName, csvInfos, interactive);

        // ��ü�� ������
        if(csvInfo != null)
        {
            TextSet = CsvProcessor.Instance.GetText(csvInfo, interactive);
        }
    }

    // DOTO -> �ڷ�ƾ���� �ؽ�Ʈ�� ȭ�鿡 ���

}
