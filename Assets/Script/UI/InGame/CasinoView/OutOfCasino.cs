using UnityEngine;

public class OutOfCasino : MonoBehaviour
{
    public Canvas_InGame MainCanvas;

    public void Button_OutOfCasino()
    {
        MainCanvas.CasinoViewClose();
    }
}
