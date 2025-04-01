using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMoneyAnimation : MonoBehaviour
{

    public void PlaySequnce_PlayerMoneyPlus(int value)
    {
        Sequence sequence = DOTween.Sequence();
        GetSequnce_PlayerMoneyPlus(sequence, value);

        sequence.SetLoops(1);
        sequence.Play();
    }

    public void PlaySequnce_PlayerMoneyMinus(int value)
    {
        Sequence sequence = DOTween.Sequence();
        GetSequnce_PlayerMoneyMinus(sequence, value);

        sequence.SetLoops(1);
        sequence.Play();
    }

    float delay = 5;
    float moveY = 150;

    Color Yelow = new Color(1f, 1f, 0f, 1f);
    Color clearYellow = new Color(1f, 1f, 0f, 0f);
    public void GetSequnce_PlayerMoneyPlus(Sequence sequence, int PlusMoney)
    {
        Text AmountOfChange = ChangeMemoryPool.Instance.GetObject().GetComponent<Text>();

        if(AmountOfChange != null)
        {
            // 숫자, 색상 초기화
            AmountOfChange.text = $"+{PlusMoney.ToString()}";
            AmountOfChange.color = Yelow;

            // 위로 올라가면서 점점 투명해짐
            sequence.Append(AmountOfChange.transform.DOLocalMoveY(moveY, delay));
            sequence.Join(AmountOfChange.DOColor(clearYellow, delay));

            // 다시 객체 회수
            sequence.AppendCallback(()=> ChangeMemoryPool.Instance.ReturnObject(AmountOfChange.gameObject));
        }
        else
        {
            Debug.LogAssertion("PlayerMoneyAnimation의 GetSequnce_PlayerMoneyPlus 오류확인 바람");
        }
    }

    Color Red = new Color(1f, 0f, 0f, 1f);
    Color clearRed = new Color(1f, 0f, 0f, 0f);
    public void GetSequnce_PlayerMoneyMinus(Sequence sequence, int MinusMoney)
    {
        Text AmountOfChange = ChangeMemoryPool.Instance.GetObject().GetComponent<Text>();
        if (AmountOfChange != null)
        {
            AmountOfChange.text = $" -{MinusMoney.ToString()}";
            AmountOfChange.color = Red;

            sequence.Append(AmountOfChange.transform.DOLocalMoveY(moveY, delay));
            sequence.Join(AmountOfChange.DOColor(clearRed, delay));

            // 다시 객체 회수
            sequence.AppendCallback(() => ChangeMemoryPool.Instance.ReturnObject(AmountOfChange.gameObject));
        }
        else
        {
            Debug.LogAssertion("PlayerMoneyAnimation의 GetSequnce_PlayerMoneyPlus 오류확인 바람");
        }

    }
}
