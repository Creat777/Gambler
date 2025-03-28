using PublicSet;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnlyOneLivesPlayerPanel : MonoBehaviour
{
    // 에디터
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
    public SelectAsTarget_Toggle selectAsTarget_Toggle;

    // 스크립트
    public PlayerEtc player {  get; private set; }

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

    public void InitPlayerInfo(PlayerEtc inputPlayer, int clockwiseOrder , cCharacterInfo playerInfo)
    {
        // 한번 설정되지 변하지 않는 값
        { 
            player = inputPlayer;
            player.SetAsisstantPanel(this);
            selectAsTarget_Toggle.SetPlayer(player);

            // 객체 이름 변경
            gameObject.name = $"{playerInfo.CharacterName}_Info";

            // 이미지 변경
            TryChangePortraitImage(playerInfo.CharaterIndex);

            // text 변경
            PlayerName.text = PlayerTemplate.PlayerName + playerInfo.CharacterName;
            PlayerAge.text = PlayerTemplate.PlayerAge + playerInfo.CharacterAge;
            //
            PlayerClan.text = PlayerTemplate.PlayerClan + playerInfo.CharacterClan;
            PlayerFeature.text = PlayerTemplate.PlayerFeature + playerInfo.CharacterFeature;
        
            switch(clockwiseOrder)
            {
                case 0: CurrentPosition.text = PlayerTemplate.CurrentPosition + "오른쪽"; break;
                case 1: CurrentPosition.text = PlayerTemplate.CurrentPosition + "정면"; break;
                case 2: CurrentPosition.text = PlayerTemplate.CurrentPosition + "왼쪽"; break;
                default: Debug.LogWarning("잘못된 값"); break;
            }
        }

        // 여러번 호출되어야 하는 함수
        { 
            PlayerBalanceUpdate();
        }
    }

    public bool TryChangePortraitImage(eCharacterType characterIndex)
    {
        bool isSueccessed = false;
        Sprite sprite = PortraitImageResource.Instance.TryGetImage(characterIndex, out isSueccessed);
        if (isSueccessed)
        {
            PlayerImage.sprite = sprite;
            Debug.Log("이미지 전환 성공");
        }
        else
        {
            Debug.LogAssertion("이미지 전환 실패");
        }

        return isSueccessed;
    }

    public void PlayerBalanceUpdate()
    {
        // 실시간으로 변하는 값
        if (player != null)
        {
            PlayerBalance.text = PlayerTemplate.PlayerBalance + player.coin.ToString();
        }
        else Debug.LogAssertion($"player == {player}, 지금 호출되면 안됨");
    }
}
