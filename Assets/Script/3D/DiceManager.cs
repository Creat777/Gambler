using DG.Tweening;
using UnityEngine;

public class DiceManager : MonoBehaviour
{
    int currentDiceValue;

    public GameObject diceChecker;
    public GameObject whiteDice;

    public void RotateDice()
    {
        // ȸ�� �����ð��� �������� ����
        float delay;
        delay = Random.Range(3.0f, 5.0f);

        // x�� ȸ��
        int[] randomValues = new int[3];
        for (int i = 0; i < randomValues.Length; i++)
        {
            randomValues[i] = (int)Random.Range(15f, 30f) * 90 ;
            Debug.Log($"randomValues[{i}] : {randomValues[i]}");
        }

        Sequence sequence = DOTween.Sequence();

        sequence.Append(whiteDice.transform.DORotate(new Vector3(randomValues[0], randomValues[1], randomValues[2]), delay, RotateMode.FastBeyond360));
        sequence.AppendCallback(() => GetDiceValue());
        sequence.AppendCallback(() => Debug.Log($"���� �ֻ��� ���� {currentDiceValue}�Դϴ�."));

        sequence.SetLoops(0);
        sequence.Play();
    }

    public int GetDiceValue()
    {
        currentDiceValue = diceChecker.GetComponent<DiceChecker>().diceValue;

        return currentDiceValue;
    }
}
