using PublicSet;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CardButtonSet : MonoBehaviour
{
    // 에디터 연결
    public GameObject cardButtonPrefab;

    // 스크립트 편집
    

    private void Awake()
    {
        
    }

    public void InitCardButton(Transform cardsParent)
    {
        // 기존 버튼 삭제
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        int cardCount = cardsParent.childCount;

        for (int i = 0; i < cardCount; i++)
        {
            Transform card = cardsParent.GetChild(i);

            GameObject obj = GameObject.Instantiate(cardButtonPrefab);

            // 부모객체 설정
            obj.transform.SetParent(transform, false);

            // 버튼과 카드 연결
            CardSelectButton script = obj.GetComponent<CardSelectButton>();
            script.MappingButtonWithCard(card.gameObject);

            // 이미지 설정
            script.SetCardButtonImage(i);

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
