using UnityEngine;

public class DiceButton : Deactivatable_ButtonBase
{
    public CardGamePlayManager cardGamePlayManager;

    // ฤÝน้
    public void PlayerDice(GameObject playerMe)
    {
        if(cardGamePlayManager != null)
        {
            cardGamePlayManager.diceManager.RotateDice(playerMe);
        }
        else
        {
            Debug.LogAssertion("cardGamePlayManager == null");
        }
        
    }

}
