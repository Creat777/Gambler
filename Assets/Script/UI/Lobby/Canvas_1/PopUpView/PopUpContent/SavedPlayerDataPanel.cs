using PublicSet;
using UnityEngine;
using UnityEngine.UI;

public class SavedPlayerDataPanel : MonoBehaviour
{
    // TextBoxFrame
    public Text saveDate;
    public Text remainingPeriod;
    public Text coin;

    public SaveAndContinue_ButtonBase button;

    public void SetPanel(ePlayerSaveKey savekey)
    {
        // �������� ����� �ð��� ��¥
        {
            string savedDate = PlayerSaveManager.Instance.LoadSavedDate(savekey);
            if (savedDate != string.Empty)
            {
                //string SavedDate = DateTime.Now.ToString("yyyy�� MM�� dd��:HH�� mm�� ss��");
                string[] dateAndTime = savedDate.Split(':');
                if (dateAndTime.Length == 2)
                {
                    SetSaveDate($"{dateAndTime[0]}\n{dateAndTime[1]}");
                }
                else Debug.LogAssertion($"�߸��� �����Ͱ� ����Ǿ��� : {savedDate}");
            }
            else SetSaveDate("null");

        }

        // ���� �� ���� �Ⱓ
        {
            int remainingPeriod = PlayerSaveManager.Instance.LoadRemainingPeriod(savekey);
            if (remainingPeriod > 0) SetRemainingPeriod($"{remainingPeriod.ToString()}��");
            else SetRemainingPeriod("null");
        }

        // �÷��̾��� ��ȭ����
        {
            sPlayerStatus status = PlayerSaveManager.Instance.LoadPlayerStatus(savekey);
            if (status != sPlayerStatus.defaultData) SetPlayerCash($"{status.coin.ToString()}����");
            else SetPlayerCash("null");
        }

        // ��ư �ݹ鿡 ���� �÷��̾�Ű�� ����
        button.SetPlayerSaveKey(savekey);
    }
    public void SetSaveDate(string value)
    {
        saveDate.text = value.ToString();
    }

    public void SetRemainingPeriod(string value)
    {
        remainingPeriod.text = value.ToString();
    }

    public void SetPlayerCash(string value)
    {
        coin.text = value.ToString();
    }
}
