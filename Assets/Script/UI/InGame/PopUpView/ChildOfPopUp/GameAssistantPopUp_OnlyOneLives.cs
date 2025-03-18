using UnityEngine;
using System.Collections.Generic;
using PublicSet;

public class GameAssistantPopUp_OnlyOneLives : PopUpBase
{
    // ������
    public PlayerEtc[] players;

    // ��ũ��Ʈ
    private List<cOnlyOneLives_PlayerInfo> playerInfoList;
    private List<int> SelectedIndex;

    private void CheckList()
    {
        if (SelectedIndex == null) SelectedIndex = new List<int>();
        else SelectedIndex.Clear();

        if (playerInfoList == null) playerInfoList = CsvManager.Instance.GetPlayerInfoList();

        if(ActiveObjList == null) ActiveObjList = new List<GameObject> ();

        if (memoryPool == null) InitializePool(players.Length);
    }
    public void InitGameAssistant()
    {
        CheckList();

        // Ȱ��ȭ�� ��� ����� �޸�Ǯ�� ȯ��
        int activeObjCount = ActiveObjList.Count;
        Debug.Log($"��ȯ����, ��ȯ�� ��ü�� ���� : {ActiveObjList.Count}");
        for (int i = 0; i< activeObjCount; i++)
        {
            ReturnObject(ActiveObjList[0]);
        }

        // �÷��̾� ���ڿ� �°� �޸�Ǯ���� ����
        int playerIndex;
        for (int i = 0; i < players.Length; i++)
        {
            GameObject obj =  GetObject();
            OnlyOneLivesPlayer playerPanelScript = obj.GetComponent<OnlyOneLivesPlayer>();

            // �ùٸ� ��ü���
            if (playerPanelScript != null)
            {
                do // ������ �ε����� ����
                {
                    playerIndex = Random.Range(0, playerInfoList.Count);
                } while (SelectedIndex.Contains(playerIndex));
                SelectedIndex.Add(playerIndex);

                // �ش� �ε����� ������ �ʱ�ȭ
                playerPanelScript.InitPlayerInfo(players[i], i, playerInfoList[playerIndex]);
            }
            
            else Debug.LogAssertion("�߸��� ������");
        }
        // �׸� �°� ����� ����
        ChangeContentRectTransform();
    }

}
