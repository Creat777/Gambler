using UnityEngine;
using System.Collections.Generic;
using PublicSet;

public class GameAssistantPopUp_OnlyOneLives : PopUpBase
{
    // 에디터
    public PlayerEtc[] players;

    // 스크립트
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

        // 활성화된 모든 목록을 메모리풀로 환수
        int activeObjCount = ActiveObjList.Count;
        Debug.Log($"반환시작, 반환될 객체의 개수 : {ActiveObjList.Count}");
        for (int i = 0; i< activeObjCount; i++)
        {
            ReturnObject(ActiveObjList[0]);
        }

        // 플레이어 숫자에 맞게 메모리풀에서 꺼냄
        int playerIndex;
        for (int i = 0; i < players.Length; i++)
        {
            GameObject obj =  GetObject();
            OnlyOneLivesPlayer playerPanelScript = obj.GetComponent<OnlyOneLivesPlayer>();

            // 올바른 객체라면
            if (playerPanelScript != null)
            {
                do // 유일한 인덱스를 선택
                {
                    playerIndex = Random.Range(0, playerInfoList.Count);
                } while (SelectedIndex.Contains(playerIndex));
                SelectedIndex.Add(playerIndex);

                // 해당 인덱스로 정보를 초기화
                playerPanelScript.InitPlayerInfo(players[i], i, playerInfoList[playerIndex]);
            }
            
            else Debug.LogAssertion("잘못된 프리팹");
        }
        // 항목에 맞게 사이즈를 변경
        ChangeContentRectTransform();
    }

}
