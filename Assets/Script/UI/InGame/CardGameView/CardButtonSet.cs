using PublicSet;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CardButtonSet : MonoBehaviour
{
    // 에디터 연결
    public GameObject cardButtonPrefab;
    public PlayerMe playerMe;

    // 스크립트 편집
    public List<CardSelectButton> cardSelectButtonList;

    private void Awake()
    {
        cardSelectButtonList = new List<CardSelectButton>();
    }

    public void InitCardButton(Transform cardsParent)
    {
        // 기존 버튼 삭제
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        cardSelectButtonList.Clear(); // 버튼을 참조하던 리스트도 초기화

        int cardCount = cardsParent.childCount;

        for (int i = 0; i < cardCount; i++)
        {
            Transform card = cardsParent.GetChild(i);

            GameObject obj = GameObject.Instantiate(cardButtonPrefab);

            // 부모객체 설정
            obj.transform.SetParent(transform, false);

            // 버튼과 카드 연결
            CardSelectButton Buttonscript = obj.GetComponent<CardSelectButton>();
            Buttonscript.MappingButtonWithCard(card.gameObject);

            // 버튼을 리스트에 추가
            cardSelectButtonList.Add(Buttonscript);

            // 이미지 설정
            Buttonscript.SetCardButtonImage(i);

            // 부모객체를 중심으로 위치 설정
            float width = obj.GetComponent<RectTransform>().sizeDelta.x;
            float offset = (cardCount % 2 == 0) ? (i - cardCount / 2 + 0.5f) : (i - cardCount / 2);

            Vector3 pos = obj.transform.parent.position;
            pos.x += offset * width * 1.1f;
            pos.y += -300;
            obj.transform.position = pos;
        }

    }

    
}
