using PublicSet;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnlyOneLivesPlayer : MonoBehaviour
{
    // ������
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

    public void InitPlayerInfo(PlayerEtc inputPlayer, int clockwiseOrder , cCharacterInfo playerInfo)
    {
        // �ѹ� �������� ������ �ʴ� ��
        player = inputPlayer;

        // ��ü �̸� ����
        gameObject.name = $"{playerInfo.CharacterName}_Info";

        // �̹��� ����
        TryChangePortraitImage(playerInfo.CharaterIndex);

        // text ����
        PlayerName.text = PlayerTemplate.PlayerName + playerInfo.CharacterName;
        PlayerAge.text = PlayerTemplate.PlayerAge + playerInfo.CharacterAge;
        //
        PlayerClan.text = PlayerTemplate.PlayerClan + playerInfo.CharacterClan;
        PlayerFeature.text = PlayerTemplate.PlayerFeature + playerInfo.CharacterFeature;
        
        switch(clockwiseOrder)
        {
            case 0: CurrentPosition.text = PlayerTemplate.CurrentPosition + "������"; break;
            case 1: CurrentPosition.text = PlayerTemplate.CurrentPosition + "����"; break;
            case 2: CurrentPosition.text = PlayerTemplate.CurrentPosition + "����"; break;
            default: Debug.LogWarning("�߸��� ��"); break;
        }

        // ������ ȣ��Ǵ� �Լ�
        PlayerBalanceUpdate();
    }

    public bool TryChangePortraitImage(eCharacter characterIndex)
    {
        bool isSueccessed = false;
        Sprite sprite = PortraitResource.Instance.TryGetPortraitImage(characterIndex, out isSueccessed);
        if (isSueccessed)
        {
            PlayerImage.sprite = sprite;
            Debug.Log("�̹��� ��ȯ ����");
        }
        else
        {
            Debug.LogAssertion("�̹��� ��ȯ ����");
        }

        return isSueccessed;
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
