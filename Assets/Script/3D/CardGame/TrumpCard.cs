using PublicSet;
using UnityEngine;

public class TrumpCard : MonoBehaviour
{
    public cTrumpCardInfo trumpCardInfo {  get; private set; } 

    public void SetTrumpCard(cTrumpCardInfo value)
    {
        trumpCardInfo = value;

        // 제대로 데이터가 삽입되었는지 확인
        CsvManager.Instance.PrintProperties(trumpCardInfo);
    }
}
