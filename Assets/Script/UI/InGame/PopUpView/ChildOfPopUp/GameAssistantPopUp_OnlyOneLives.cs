using UnityEngine;
using System.Collections.Generic;
using PublicSet;
using static UnityEditor.Experimental.GraphView.GraphView;

public class GameAssistantPopUp_OnlyOneLives : PopUpBase<GameAssistantPopUp_OnlyOneLives>
{
    // 에디터
    public PlayerEtc[] players;

    // 스크립트
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

                    // 올바른 객체라면
                    if (playerPanelScript != null)
                    {
                        do // 유일한 인덱스를 선택
                        {
                            playerIndex = Random.Range((int)(eCharacterType.KangDoYun), (int)(eCharacterType.OhJinSoo) + 1);
                        } while (SelectedIndex.Contains(playerIndex));
                        SelectedIndex.Add(playerIndex);

                        // 해당 인덱스로 정보를 초기화
                        playerPanelScript.InitPlayerInfo(players[i], i, CsvManager.Instance.GetCharacterInfo((eCharacterType)playerIndex));
                    }
                    else Debug.LogAssertion("잘못된 프리팹");
                }
            });
    }


    //public void RefreshPopUp()
    //{
    //    PreCheck();

    //    // 필요한 객체의 개수
    //    int requiredCount = players.Length - ActiveObjList.Count;

    //    // 객체가 더 필요한 경우 메모리풀에서 꺼냄
    //    if (requiredCount > 0) 
    //    {
    //        for(int i = 0; i < requiredCount; i++)
    //        {
    //            GetObject();
    //        }
    //    }
    //    // 필요없는 만큼 환수함
    //    else if(requiredCount< 0)
    //    {
    //        for (int i = 0; i > (-requiredCount); i++)
    //        {
    //            ReturnObject(ActiveObjList[0]);
    //        }
    //    }

    //    // 현재 활성화된 객체에서 정보를 초기화
    //    int playerIndex;
    //    for (int i = 0; i < ActiveObjList.Count; i++)
    //    {
    //        OnlyOneLivesPlayer playerPanelScript = ActiveObjList[i].GetComponent<OnlyOneLivesPlayer>();

    //        // 올바른 객체라면
    //        if (playerPanelScript != null)
    //        {
    //            do // 유일한 인덱스를 선택
    //            {
    //                playerIndex = Random.Range((int)(eCharacterType.KangDoYun), (int)(eCharacterType.OhJinSoo)+1);
    //            } while (SelectedIndex.Contains(playerIndex));
    //            SelectedIndex.Add(playerIndex);

    //            // 해당 인덱스로 정보를 초기화
    //            playerPanelScript.InitPlayerInfo(players[i], i, CsvManager.Instance.GetCharacterInfo((eCharacterType)playerIndex));
    //        }
    //        else Debug.LogAssertion("잘못된 프리팹");
    //    }
    //    // 항목에 맞게 사이즈를 변경
    //    ChangeContentRectTransform();
    //}

}
