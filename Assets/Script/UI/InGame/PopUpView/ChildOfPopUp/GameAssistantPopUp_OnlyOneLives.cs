using UnityEngine;
using System.Collections.Generic;
using PublicSet;
using static UnityEditor.Experimental.GraphView.GraphView;

public class GameAssistantPopUp_OnlyOneLives : PopUpBase<GameAssistantPopUp_OnlyOneLives>
{
    // ������
    public PlayerEtc[] players;

    // ��ũ��Ʈ
    private List<int> SelectedIndex;

    protected void PreCheck()
    {
        if (SelectedIndex == null) SelectedIndex = new List<int>();
        else SelectedIndex.Clear();
    }

    public void RefreshPopUp()
    {
        PreCheck();

        RefreshPopUp(players.Length,
            () =>
            {
                int playerIndex;
                for (int i = 0; i < ActiveObjList.Count; i++)
                {
                    OnlyOneLivesPlayer playerPanelScript = ActiveObjList[i].GetComponent<OnlyOneLivesPlayer>();

                    // �ùٸ� ��ü���
                    if (playerPanelScript != null)
                    {
                        do // ������ �ε����� ����
                        {
                            playerIndex = Random.Range((int)(eCharacterType.KangDoYun), (int)(eCharacterType.OhJinSoo) + 1);
                        } while (SelectedIndex.Contains(playerIndex));
                        SelectedIndex.Add(playerIndex);

                        // �ش� �ε����� ������ �ʱ�ȭ
                        playerPanelScript.InitPlayerInfo(players[i], i, CsvManager.Instance.GetCharacterInfo((eCharacterType)playerIndex));
                    }
                    else Debug.LogAssertion("�߸��� ������");
                }
            });
    }


    //public void RefreshPopUp()
    //{
    //    PreCheck();

    //    // �ʿ��� ��ü�� ����
    //    int requiredCount = players.Length - ActiveObjList.Count;

    //    // ��ü�� �� �ʿ��� ��� �޸�Ǯ���� ����
    //    if (requiredCount > 0) 
    //    {
    //        for(int i = 0; i < requiredCount; i++)
    //        {
    //            GetObject();
    //        }
    //    }
    //    // �ʿ���� ��ŭ ȯ����
    //    else if(requiredCount< 0)
    //    {
    //        for (int i = 0; i > (-requiredCount); i++)
    //        {
    //            ReturnObject(ActiveObjList[0]);
    //        }
    //    }

    //    // ���� Ȱ��ȭ�� ��ü���� ������ �ʱ�ȭ
    //    int playerIndex;
    //    for (int i = 0; i < ActiveObjList.Count; i++)
    //    {
    //        OnlyOneLivesPlayer playerPanelScript = ActiveObjList[i].GetComponent<OnlyOneLivesPlayer>();

    //        // �ùٸ� ��ü���
    //        if (playerPanelScript != null)
    //        {
    //            do // ������ �ε����� ����
    //            {
    //                playerIndex = Random.Range((int)(eCharacterType.KangDoYun), (int)(eCharacterType.OhJinSoo)+1);
    //            } while (SelectedIndex.Contains(playerIndex));
    //            SelectedIndex.Add(playerIndex);

    //            // �ش� �ε����� ������ �ʱ�ȭ
    //            playerPanelScript.InitPlayerInfo(players[i], i, CsvManager.Instance.GetCharacterInfo((eCharacterType)playerIndex));
    //        }
    //        else Debug.LogAssertion("�߸��� ������");
    //    }
    //    // �׸� �°� ����� ����
    //    ChangeContentRectTransform();
    //}

}
