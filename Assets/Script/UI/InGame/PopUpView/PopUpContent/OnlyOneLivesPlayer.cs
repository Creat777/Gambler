using PublicSet;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnlyOneLivesPlayer : MonoBehaviour
{
    // ������
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

    // ��ũ��Ʈ
    public PlayerEtc player;

    public class PlayerTemplate
    {
        public const string PlayerName = "�̸� : ";
        public const string PlayerAge = "���� : ";
        public const string PlayerBalance = "���� ���� : ";
        //
        public const string PlayerClan = "�Ҽ� : ";
        public const string PlayerFeature = "Ư¡ : ";
        public const string CurrentPosition = "���� ���̺� ��ġ : ";

        public const string SelectAsTarget = "�ش� �÷��̾ ����";
    }

    private void Start()
    {
        //SaveTemplate();
    }

    // �����ͷ� ������ ���ø��� ����
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
        // �ѹ� �������� ������ �ʴ� ��
        player = inputPlayer;
        //
        gameObject.name = $"{playerInfo.playerIndex}�� ��ü + {playerInfo.PlayerName}";
        PlayerImage.sprite = PlayerSpriteArr[playerInfo.playerIndex];
        PlayerName.text = PlayerTemplate.PlayerName + playerInfo.PlayerName;
        PlayerAge.text = PlayerTemplate.PlayerAge + playerInfo.PlayerAge;
        //
        PlayerClan.text = PlayerTemplate.PlayerClan + playerInfo.PlayerClan;
        PlayerFeature.text = PlayerTemplate.PlayerFeature + playerInfo.PlayerFeature;
        
        switch(clockwiseOrder)
        {
            case 0: CurrentPosition.text = PlayerTemplate.CurrentPosition + "������"; break;
            case 1: CurrentPosition.text = PlayerTemplate.CurrentPosition + "����"; break;
            case 2: CurrentPosition.text = PlayerTemplate.CurrentPosition + "����"; break;
            default: Debug.LogWarning("�߸��� ��"); break;
        }

        // �ǽð����� ���ϴ� ��
        PlayerBalanceUpdate();
    }

    private void PlayerBalanceUpdate()
    {
        // �ǽð����� ���ϴ� ��
        if (player != null)
        {
            PlayerBalance.text = PlayerTemplate.PlayerBalance + player.coin.ToString();
        }
        else Debug.LogAssertion($"player == {player}, ���� ȣ��Ǹ� �ȵ�");
    }
}
