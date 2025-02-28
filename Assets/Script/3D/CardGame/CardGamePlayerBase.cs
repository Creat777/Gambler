using UnityEngine;

public abstract class CardGamePlayerBase : MonoBehaviour
{
    public bool diceDone {  get; private set; }
    public int myDiceValue { get; private set; }

    public virtual void SetDiceValue(int diceValue)
    {
        myDiceValue = diceValue;
        diceDone = true;

        Debug.Log($"{gameObject.name}�� ������ {myDiceValue}�Դϴ�" +
            $"-> diceDone == {diceDone}");
    }


    public virtual void InitAttribute()
    {
        diceDone = false;
        myDiceValue = 0;

        Debug.Log($"{gameObject.name}�� �Ӽ� �ʱ�ȭ ->" +
            $"diceDone == {diceDone} " +
            $"myDiceValue == {myDiceValue}");
    }
}
