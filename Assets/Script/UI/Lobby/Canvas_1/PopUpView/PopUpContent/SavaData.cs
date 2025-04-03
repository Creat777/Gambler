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
        public const string savaDate = "����ð� : ";
        public const string remainingPeriod = "�����Ⱓ : ";
        public const string coin = "�����ݾ� : ";
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
