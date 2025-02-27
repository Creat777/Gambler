using UnityEngine;

public abstract class CardGamePlayerBase : MonoBehaviour
{
    public bool diceDone {  get; private set; }
    public int myDiceValue { get; private set; }

    public virtual void SetDiceValue(int diceValue)
    {
        myDiceValue = diceValue;
        diceDone = true;
    }


    public virtual void InitAttribute()
    {
        diceDone = false;
        myDiceValue = 0;
    }
}
