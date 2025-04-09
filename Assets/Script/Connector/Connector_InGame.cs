using PublicSet;
using Unity.VisualScripting;
using UnityEngine;


// æ¿∫π±ÕΩ√ ΩÃ±€≈Ê ∞¥√º¿« √≥∏Æ ¥„¥Á
public class Connector_InGame : Connector
{
    public Canvas_InGame MainCanvas_script;

    public GameObject player;
    public GameObject insideOfHouse;
    public GameObject outsideOfHouse;
    public GameObject interfaceView;
    public GameObject textWindowView;

    public EventView eventView;
    public PlayerMoneyView playerMoneyView_Script;
    public TextWindowView textWindowView_Script;
    public IconView iconView_Script;
    public PopUpView_InGame popUpView_Script;
    public YouLoseView youLoseView_Script;  
    public Joystick joystick_Script;
    public Map map_Script;
    public Box box_Script;
}
