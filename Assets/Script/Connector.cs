using PublicSet;
using Unity.VisualScripting;
using UnityEngine;


// æ¿∫π±ÕΩ√ ΩÃ±€≈Ê ∞¥√º¿« √≥∏Æ ¥„¥Á
public class Connector : MonoBehaviour
{
    public Canvas_InGame MainCanvas_script;

    public GameObject blackView;
    public GameObject StageView;

    public GameObject player;
    public GameObject insideOfHouse;
    public GameObject outsideOfHouse;
    public GameObject interfaceView;
    public GameObject textWindowView;

    public PlayerMoneyView playerMoneyView_Script;
    public TextWindowView textWindowView_Script;
    public IconView iconView_Script;
    public PopUpView popUpView_Script;
    public YouLoseView youLoseView_Script;  
    public Joystick joystick_Script;
    public Map map_Script;
    public Box box_Script;

    void Start()
    {
        if(GameManager.Instance.currentScene == eScene.InGame)
        {
            GameManager.Instance.Join_In_Game();
        }
    }
}
