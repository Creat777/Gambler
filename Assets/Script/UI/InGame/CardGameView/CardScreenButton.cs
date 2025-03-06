using DG.Tweening;
using UnityEngine;

public class CardScreenButton : Deactivatable_Button_Base
{
    private void Awake()
    {
        SetButtonCallback(subScreenOpen);
    }

    public CardGameView cardGameView;
    public void subScreenClose()
    {
        if(cardGameView != null)
        {
            Sequence sequence = DOTween.Sequence();
            cardGameView.GetSequnce_CardScrrenClose(sequence);
            sequence.SetLoops(1);
            sequence.Play();
        }
        else
        {
            Debug.LogAssertion("CardScrrenButton == null");
        }
    }

    public void subScreenOpen()
    {
        if (cardGameView != null)
        {
            Sequence sequence = DOTween.Sequence();
            cardGameView.GetSequnce_CardScrrenOpen(sequence);
            sequence.SetLoops(1);
            sequence.Play();
        }
        else
        {
            Debug.LogAssertion("CardScrrenButton == null");
        }
    }

}
