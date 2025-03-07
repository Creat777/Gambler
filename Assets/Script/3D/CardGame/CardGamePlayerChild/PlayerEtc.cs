using UnityEngine;

public class PlayerEtc : CardGamePlayerBase
{
    public void SelectStartCard()
    {
        for (int i = 0; i <Cards.Count; i++)
        {
            TrumpCardDefault card = Cards[i].GetComponent<TrumpCardDefault>();
            if(card != null)
            {
                card.TrySelectThisCard(this);
            }
        }
    }
}
