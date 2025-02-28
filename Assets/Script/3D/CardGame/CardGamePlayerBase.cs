using UnityEngine;

public abstract class CardGamePlayerBase : MonoBehaviour
{
    public bool diceDone {  get; private set; }
    public int myDiceValue { get; private set; }

    public virtual void SetDiceValue(int diceValue)
    {
        myDiceValue = diceValue;
        diceDone = true;

        Debug.Log($"{gameObject.name}의 눈금은 {myDiceValue}입니다" +
            $"-> diceDone == {diceDone}");
    }


    public virtual void InitAttribute()
    {
        diceDone = false;
        myDiceValue = 0;

        Debug.Log($"{gameObject.name}의 속성 초기화 ->" +
            $"diceDone == {diceDone} " +
            $"myDiceValue == {myDiceValue}");
    }
}
