using DG.Tweening;
using PublicSet;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventManager : Singleton<EventManager>
{
    public Connector_InGame connector_InGame { get { return GameManager.connector_InGame; } }
    


    Dictionary<eStage, string> StageMessageDict;

    protected override void Awake()
    {
        base.Awake();
        Init_StageMessageDict();
    }
    public void Init_StageMessageDict()
    {
        StageMessageDict = new Dictionary<eStage, string>();
        StageMessageDict.Add(eStage.Stage1, "STAGE 1\n여기가 대체 어디야?");
        StageMessageDict.Add(eStage.Stage2, "STAGE 2\n카지노에 입성하자");
    }


    readonly float intervalDelay = 0.5f; // 이벤트 종료후 스테이지화면 등장 대기시간
    readonly float delay = 1f; // 화면 등장시간
    readonly Color colorClearAlpha = new Color(1, 1, 1, 0);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sequence"></param>
    /// <param name="middleCallback">이벤트에 쓰일 문장</param>
    /// <param name="eventText">이벤트 문장이 적힌 텍스트</param>
    public void GetSequnce_EventAnimation(Sequence sequence, Text eventText)
    {
        //Debug.Log("stage 애니메이션 시작");
        

        // 이벤트화면 활성화
        connector_InGame.eventView.gameObject.SetActive(true);

        // 이미지 색깔 초기화
        Image stateViewImage = connector_InGame.eventView.GetComponent<Image>();
        stateViewImage.color = colorClearAlpha;
        connector_InGame.eventView.SetTextColer(Color.clear);

        // 이벤트 끝난 후 간격
        sequence.AppendInterval(intervalDelay);

        // 이벤트 화면 페이드인
        sequence.Append(stateViewImage.DOColor(Color.white, delay));
        sequence.Join(eventText.DOColor(Color.black, delay));

        // 유지
        sequence.AppendInterval(delay);

        // 이벤트 화면 페이드아웃
        sequence.Append(stateViewImage.DOColor(colorClearAlpha, delay));
        sequence.Join(eventText.DOColor(Color.clear, delay));

        // 이벤트 화면 비활성화
        sequence.AppendCallback(() => connector_InGame.eventView.gameObject.SetActive(false));
    }


    public void SetEventMessage(string message)
    {
        connector_InGame.eventView.SetTextContent(message); // 이미지 내부 텍스트 설정
    }

    public void PlaySequnce_EventAnimation()
    {
        Sequence sequence = DOTween.Sequence();
        GetSequnce_EventAnimation(sequence, connector_InGame.eventView.eventText);

        sequence.SetLoops(1);
        sequence.Play();
    }
}
