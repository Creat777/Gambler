using UnityEngine;

public class CardGameStartButton : MonoBehaviour
{
    public CardGameView GameView;

    public void OnButtonClick()
    {
        GameView.StartGame();
    }
}
