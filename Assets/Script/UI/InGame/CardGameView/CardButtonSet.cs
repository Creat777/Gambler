using PublicSet;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CardButtonSet : MonoBehaviour
{
    // ������ ����
    public GameObject cardButtonPrefab;

    // ��ũ��Ʈ ����
    

    private void Awake()
    {
        
    }

    public void InitCardButton(Transform cardsParent)
    {
        // ���� ��ư ����
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        int cardCount = cardsParent.childCount;

        for (int i = 0; i < cardCount; i++)
        {
            Transform card = cardsParent.GetChild(i);

            GameObject obj = GameObject.Instantiate(cardButtonPrefab);

            // �θ�ü ����
            obj.transform.SetParent(transform, false);

            // ��ư�� ī�� ����
            CardSelectButton script = obj.GetComponent<CardSelectButton>();
            script.MappingButtonWithCard(card.gameObject);

            // �̹��� ����
            script.SetCardButtonImage(i);

            // �θ�ü�� �߽����� ��ġ ����
            float width = obj.GetComponent<RectTransform>().sizeDelta.x;
            float offset = (cardCount % 2 == 0) ? (i - cardCount / 2 + 0.5f) : (i - cardCount / 2);

            Vector3 pos = obj.transform.parent.position;
            pos.x += offset * width * 1.1f;
            pos.y += -300;
            obj.transform.position = pos;
        }

    }

    
}
