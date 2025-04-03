using PublicSet;
using UnityEngine;
using System;
using UnityEngine.UI;

public class SavaData : MonoBehaviour
{
    // TextBoxFrame
    public Text savaDate;
    public Text remainingPeriod;
    public Text coin;


    public class PlayerTemplate
    {
        public const string savaDate = "저장시간 : ";
        public const string remainingPeriod = "남은기간 : ";
        public const string coin = "소지금액 : ";
    }


    public void SetSavaDate(string value)
    {
        savaDate.text = PlayerTemplate.savaDate + value.ToString();
    }

    public void SetRemainingPeriod(string value)
    {
        remainingPeriod.text = PlayerTemplate.remainingPeriod + value.ToString();
    }

    public void SetPlayerCash(string value)
    {
        coin.text = PlayerTemplate.coin + value.ToString();
    }
}
