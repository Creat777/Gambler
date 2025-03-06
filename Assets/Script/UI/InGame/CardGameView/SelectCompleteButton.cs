using DG.Tweening;
using UnityEngine;

public class SelectCompleteButton : Deactivatable_Button_Base
{
    public CardGamePlayManager cardGamePlayManager;

    // ��ư�ݹ�
    // �����ϴ� �������� ��ǻ�ʹ� �̹� ������ �Ϸ��߾����
    public void CompleteCardSelect()
    {
        float returnDelay = 0;
        if (cardGamePlayManager != null)
        {
            Sequence sequence = DOTween.Sequence();
            cardGamePlayManager.cardGameView.GetSequnce_CardScrrenClose(sequence);
            
            foreach (var player in cardGamePlayManager.players)
            {
                if (player.gameObject.layer == cardGamePlayManager.layerOfMe)
                {
                    sequence.AppendCallback(()=>cardGamePlayManager.cardButtonSet.InitCardButton(player.closeBox.transform));
                }
                returnDelay += player.GetSequnce_CompleteSelectCard(sequence);
                returnDelay += player.GetSequnece_CardSpread(sequence);
            }
            Debug.Log($"�ִϸ��̼� �� �ð� : {returnDelay}");
            sequence.SetLoops(1);
            sequence.Play();
        }
        else
        {
            Debug.LogAssertion("cardGamePlayManager == null");
        }
    }
}
