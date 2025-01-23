using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class ScoreCanvas : MonoBehaviour
{
    public GameObject ScorePanel;
    public GameObject button_BackToMenu;
    public GameObject button_ScoreClear;
    public GameObject[] ScoreTexts;
    
    
    float panelHeight;
    
    void Start()
    {
        //PanelPositioning();
        ScoreExpression();
    }

    public void ScoreExpression()
    {
        string[] RankersIDs = PlayerInfoManager.Instance.GetRankersID(GameManager.Instance.playerRankKeys);
        float[] RankersScores = PlayerInfoManager.Instance.GetRankScores(RankersIDs);
        // 점수판에 점수를 입력
        for (int i = 0; i < ScoreTexts.Length; i++)
        {
            Text textComp = ScoreTexts[i].GetComponentInChildren<Text>();
            if (textComp != null)
            {
                // 플레이어 아이디에서 필요한 것만 추출
                string playerName = "";
                if (RankersIDs[i].Split('_').Length >=2)
                {
                    playerName = RankersIDs[i].Split('_')[1];
                }
                else
                {
                    foreach (var ID in RankersIDs)
                    {
                        playerName = "NonePlayer";
                        Debug.LogError($"ID : {ID}");
                    }
                }

                if(playerName == "NonePlayer")
                {
                    textComp.text = $"{i + 1}위\n{playerName}";
                }
                else
                {
                    // ~위 Name(점수)
                    textComp.text = $"{playerName}\nscore : {RankersScores[i].ToString("0")}";
                }
                
            }
            else
            {
                Debug.LogError("점수판을 찾을 수 없음");
            }
        }
    }

    public void ButtonScoreClear()
    {
        ButtonClickAudio.Instance.PlayClip();
        PlayerInfoManager.Instance.ResetAllRank(GameManager.Instance.playerRankKeys);
        ScoreExpression();
    }

    public void ButtonBackToMenu()
    {
        ButtonClickAudio.Instance.PlayClip();
        SceneManager.Instance.Button_GoToMenu();
    }
}
