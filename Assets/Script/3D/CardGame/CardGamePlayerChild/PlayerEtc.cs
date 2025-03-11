using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class PlayerEtc : CardGamePlayerBase
{
    public void SelectCard_OnStartTime()
    {
        for (int i = 0; i <CardList.Count; i++)
        {
            TrumpCardDefault card = CardList[i].GetComponent<TrumpCardDefault>();
            if(card != null)
            {
                card.TrySelectThisCard_OnStartTime(this);
            }
        }
    }

    public void SelectCardAndTarget_OnPlayTime(List<CardGamePlayerBase> playerList)
    {
        Debug.Log($"��ǻ�� \"{gameObject.name}\"�� ������ ���� ī�带 ���� ������");

        int randomPlayerIndex = Random.Range(0, playerList.Count);
        int randomCardIndex = Random.Range(0, closedCardList.Count);

        SetAttackTarget(playerList[randomPlayerIndex]);
        closedCardList[randomCardIndex].TrySelectThisCard_OnPlayTime(this);
    }
}
