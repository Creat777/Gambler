using UnityEngine;
using UnityEngine.UI;
public class TextWindowView : MonoBehaviour
{
    // 에디터에서 연결
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
        // 변수 정리
        string objectName = Player.Instance.hitObjectName;
        CsvInfo[] csvInfos = CsvManager.Instance.InteractiveCsvInfos;
        Interactive interactive = new Interactive();

        // 플레이어가 상호작용할 수 있는 객체가 있는지 확인
        csvInfo = CsvProcessor.Instance.FindCsvInfo(objectName, csvInfos, interactive);

        // 객체가 있으면
        if(csvInfo != null)
        {
            TextSet = CsvProcessor.Instance.GetText(csvInfo, interactive);
        }
    }

    // DOTO -> 코루틴으로 텍스트를 화면에 출력

}
