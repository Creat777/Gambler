using UnityEngine;

public class JoyStickView : InterfaceView
{
    public GameObject InteractButton;
    

    private void Start()
    {

    }

    public override void InteractButton_Off()
    {
        InteractButton.SetActive(false);
        isInteractiveOn = false;
    }

    public override void InteractButton_On()
    {
        InteractButton.SetActive(true);
        isInteractiveOn = true;
    }
}
