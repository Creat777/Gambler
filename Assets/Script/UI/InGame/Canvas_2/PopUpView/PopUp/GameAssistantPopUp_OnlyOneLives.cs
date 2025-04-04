using UnityEngine;
using System.Collections.Generic;
using PublicSet;
using UnityEngine.UIElements;

public class GameAssistantPopUp_OnlyOneLives : PopUpBase<GameAssistantPopUp_OnlyOneLives>
{
    // ��ũ��Ʈ
    public List<PlayerEtc> players {  get; private set; } 
    private List<int> SelectedIndex;


    public void InitPlayerList()
    {
        if (players == null) players = new List<PlayerEtc>();
        else players.Clear();

        Queue<CardGamePlayerBase> tempQueue = new Queue<CardGamePlayerBase>(CardGamePlayManager.Instance.playerList);

        while (tempQueue.Count > 0)
        {
            CardGamePlayerBase player = tempQueue.Dequeue();
            if (player.CompareTag("Player")) continue;
            else players.Add(player as PlayerEtc);
        }
    }

    protected void PreCheck()
    {
        if (SelectedIndex == null) SelectedIndex = new List<int>();
        else SelectedIndex.Clear();
    }

    public override void RefreshPopUp()
    {
        PreCheck();
        InitPlayerList();

        RefreshPopUp(players.Count,
            () =>
            {
                int playerIndex;
                for (int i = 0; i < ActiveObjList.Count; i++)
                {
                    OnlyOneLivesPlayerPanel playerPanelScript = ActiveObjList[i].GetComponent<OnlyOneLivesPlayerPanel>();

                    // �ùٸ� ��ü���
                    if (playerPanelScript != null)
                    {
                        do // ������ �ε����� ����
                        {
                            playerIndex = Random.Range((int)(eCharacterType.KangDoYun), (int)(eCharacterType.OhJinSoo) + 1);
                        } while (SelectedIndex.Contains(playerIndex));
                        SelectedIndex.Add(playerIndex);

                        // �ش� �ε����� ������ �ʱ�ȭ
                        cCharacterInfo info = CsvManager.Instance.GetCharacterInfo((eCharacterType)playerIndex);
                        players[i].SetCharacterInfo(info);
                        playerPanelScript.InitPlayerInfo(players[i], i, info);
                    }
                    else Debug.LogAssertion("�߸��� ������");
                }
            });
    }

    /// <summary>
    /// ������ �Ѹ��� �������� �� �ٸ� ����� �������� ���ϵ��� ����
    /// </summary>
    /// <param name="exception"></param>
    public void PlaceRestrictionToSelections(SelectAsTarget_Toggle exception)
    {
        for (int i = 0; i < ActiveObjList.Count; i++)
        {
            OnlyOneLivesPlayerPanel panel = ActiveObjList[i].GetComponent<OnlyOneLivesPlayerPanel>();

            if (panel.selectAsTarget_Toggle == exception) continue;
            else
            {
                panel.selectAsTarget_Toggle.SetInteractable(false);
            }
        }
    }

    public void PlaceRestrictionToAllSelections()
    {
        for (int i = 0; i < ActiveObjList.Count; i++)
        {
            OnlyOneLivesPlayerPanel panel = ActiveObjList[i].GetComponent<OnlyOneLivesPlayerPanel>();
            panel.selectAsTarget_Toggle.SetInteractable(false);
        }
    }

    public void LiftRestrictionToSelections(SelectAsTarget_Toggle exception)
    {
        for (int i = 0; i < ActiveObjList.Count; i++)
        {
            OnlyOneLivesPlayerPanel panel = ActiveObjList[i].GetComponent<OnlyOneLivesPlayerPanel>();

            if (panel.selectAsTarget_Toggle == exception) continue;
            else
            {
                panel.selectAsTarget_Toggle.SetInteractable(true);
            }
        }
    }

    public void LiftRestrictionToAllSelections()
    {
        for (int i = 0; i < ActiveObjList.Count; i++)
        {
            OnlyOneLivesPlayerPanel panel = ActiveObjList[i].GetComponent<OnlyOneLivesPlayerPanel>();
            panel.selectAsTarget_Toggle.SetInteractable(true);
        }
    }


}
