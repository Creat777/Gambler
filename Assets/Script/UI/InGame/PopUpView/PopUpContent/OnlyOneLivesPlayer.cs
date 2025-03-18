using PublicSet;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnlyOneLivesPlayer : MonoBehaviour
{
    // 에디터
    public Sprite[] PlayerSpriteArr;
    public Image PlayerImage;

    // TextBoxFrame
    public Text PlayerName;
    public Text PlayerAge;
    public Text PlayerBalance;

    // FeatureAndSelectBoxFrame
    public Text PlayerClan;
    public Text PlayerFeature;
    public Text CurrentPosition;
    public Text SelectAsTarget_Label;
    public Toggle SelectAsTarget_Toggle;

    // 스크립트
    public PlayerEtc player;

    public class PlayerTemplate
    {
        public const string PlayerName = "이름 : ";
        public const string PlayerAge = "나이 : ";
        public const string PlayerBalance = "남은 코인 : ";
        //
        public const string PlayerClan = "소속 : ";
        public const string PlayerFeature = "특징 : ";
        public const string CurrentPosition = "현재 테이블 위치 : ";

        public const string SelectAsTarget = "해당 플레이어를 선택";
    }

    private void Start()
    {
        //SaveTemplate();
    }

    // 에디터로 설정한 템플릿을 저장
    //private void SaveTemplate()
    //{
    //    cTemplate.PlayerName = PlayerName.text;
    //    cTemplate.PlayerAge = PlayerAge.text;
    //    cTemplate.PlayerBalance = PlayerBalance.text;
    //    //
    //    cTemplate.PlayerClan = PlayerClan.text;
    //    cTemplate.PlayerFeature = PlayerFeature.text;
    //    cTemplate.CurrentPosition = CurrentPosition.text;
    //    cTemplate.SelectAsTarget = SelectAsTarget_Label.text;
    //}

    public void InitPlayerInfo(PlayerEtc inputPlayer, int clockwiseOrder , cOnlyOneLives_PlayerInfo playerInfo)
    {
        // 한번 설정되지 변하지 않는 값
        player = inputPlayer;
        //
        gameObject.name = $"{playerInfo.playerIndex}번 객체 + {playerInfo.PlayerName}";
        PlayerImage.sprite = PlayerSpriteArr[playerInfo.playerIndex];
        PlayerName.text = PlayerTemplate.PlayerName + playerInfo.PlayerName;
        PlayerAge.text = PlayerTemplate.PlayerAge + playerInfo.PlayerAge;
        //
        PlayerClan.text = PlayerTemplate.PlayerClan + playerInfo.PlayerClan;
        PlayerFeature.text = PlayerTemplate.PlayerFeature + playerInfo.PlayerFeature;
        
        switch(clockwiseOrder)
        {
            case 0: CurrentPosition.text = PlayerTemplate.CurrentPosition + "오른쪽"; break;
            case 1: CurrentPosition.text = PlayerTemplate.CurrentPosition + "정면"; break;
            case 2: CurrentPosition.text = PlayerTemplate.CurrentPosition + "왼쪽"; break;
            default: Debug.LogWarning("잘못된 값"); break;
        }

        // 실시간으로 변하는 값
        PlayerBalanceUpdate();
    }

    private void PlayerBalanceUpdate()
    {
        // 실시간으로 변하는 값
        if (player != null)
        {
            PlayerBalance.text = PlayerTemplate.PlayerBalance + player.coin.ToString();
        }
        else Debug.LogAssertion($"player == {player}, 지금 호출되면 안됨");
    }
}
