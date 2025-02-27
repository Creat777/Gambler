using UnityEngine;

public class DiceChecker : MonoBehaviour
{
    public int diceValue {  get; private set; }


    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Dice")
        {
            //Debug.Log($"Ãæµ¹! {collision.gameObject.name}");
            if (int.TryParse(collision.gameObject.name, out int value))
            {
                diceValue = value;
            }
        }
    }
}
