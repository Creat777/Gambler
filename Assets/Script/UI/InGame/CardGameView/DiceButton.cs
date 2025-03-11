using UnityEngine;

public class DiceButton : Deactivatable_ButtonBase
{
    public CardGamePlayManager cardGamePlayManager;

    // �ݹ�
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
