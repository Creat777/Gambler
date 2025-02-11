using Unity.VisualScripting;
using UnityEngine;


// æ¿∫π±ÕΩ√ ΩÃ±€≈Ê ∞¥√º¿« √≥∏Æ ¥„¥Á
public class Connector : MonoBehaviour
{
    public GameObject blackView;
    public GameObject player;
    public GameObject insideOfHouse;
    public GameObject outsideOfHouse;
    public GameObject interfaceView;
    public GameObject textWindowView;
    public GameObject iconView;
    
    public InteractionView interactionView_Script;
    public Joystick joystick_Script;
    public Map map_Script;
    public Box box_Script;

    void Start()
    {
        GameManager.Instance.Join_In_Game();
    }
}
