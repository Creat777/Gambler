using PublicSet;
using UnityEngine;

public class TrumpCard : MonoBehaviour
{
    public cTrumpCardInfo trumpCardInfo {  get; private set; } 

    public void SetTrumpCard(cTrumpCardInfo value)
    {
        trumpCardInfo = value;

        // ����� �����Ͱ� ���ԵǾ����� Ȯ��
        CsvManager.Instance.PrintProperties(trumpCardInfo);
    }
}
