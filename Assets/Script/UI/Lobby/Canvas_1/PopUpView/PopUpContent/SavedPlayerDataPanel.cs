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
        // 데이터자 저장된 시간과 날짜
        {
            string savedDate = PlayerSaveManager.Instance.LoadSavedDate(savekey);
            if (savedDate != string.Empty)
            {
                //string SavedDate = DateTime.Now.ToString("yyyy년 MM월 dd일:HH시 mm분 ss초");
                string[] dateAndTime = savedDate.Split(':');
                if (dateAndTime.Length == 2)
                {
                    SetSaveDate($"{dateAndTime[0]}\n{dateAndTime[1]}");
                }
                else Debug.LogAssertion($"잘못된 데이터가 저장되었음 : {savedDate}");
            }
            else SetSaveDate("null");

        }

        // 게임 내 남은 기간
        {
            int remainingPeriod = PlayerSaveManager.Instance.LoadRemainingPeriod(savekey);
            if (remainingPeriod > 0) SetRemainingPeriod($"{remainingPeriod.ToString()}일");
            else SetRemainingPeriod("null");
        }

        // 플레이어의 재화상태
        {
            sPlayerStatus status = PlayerSaveManager.Instance.LoadPlayerStatus(savekey);
            if (status != sPlayerStatus.defaultData) SetPlayerCash($"{status.coin.ToString()}코인");
            else SetPlayerCash("null");
        }

        // 버튼 콜백에 쓰일 플레이어키를 설정
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
