using PublicSet;
using Unity.VisualScripting;
using UnityEngine;


// �����ͽ� �̱��� ��ü�� ó�� ���
public class Connector_InGame : Connector
{
    public Canvas0_InGame canvas0_InGame;

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
