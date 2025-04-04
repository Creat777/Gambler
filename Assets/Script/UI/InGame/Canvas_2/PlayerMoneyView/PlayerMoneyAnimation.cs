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
            // ����, ���� �ʱ�ȭ
            AmountOfChange.text = $"+{PlusMoney.ToString()}";
            AmountOfChange.color = Yelow;

            // ���� �ö󰡸鼭 ���� ��������
            sequence.Append(AmountOfChange.transform.DOLocalMoveY(moveY, delay));
            sequence.Join(AmountOfChange.DOColor(clearYellow, delay));

            // �ٽ� ��ü ȸ��
            sequence.AppendCallback(()=> ChangeMemoryPool.Instance.ReturnObject(AmountOfChange.gameObject));
        }
        else
        {
            Debug.LogAssertion("PlayerMoneyAnimation�� GetSequnce_PlayerMoneyPlus ����Ȯ�� �ٶ�");
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

            // �ٽ� ��ü ȸ��
            sequence.AppendCallback(() => ChangeMemoryPool.Instance.ReturnObject(AmountOfChange.gameObject));
        }
        else
        {
            Debug.LogAssertion("PlayerMoneyAnimation�� GetSequnce_PlayerMoneyPlus ����Ȯ�� �ٶ�");
        }

    }
}
