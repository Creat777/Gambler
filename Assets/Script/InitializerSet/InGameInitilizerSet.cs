using Unity.VisualScripting;
using UnityEngine;


// æ¿∫π±ÕΩ√ ΩÃ±€≈Ê ∞¥√º¿« √≥∏Æ ¥„¥Á
public class InGameInitilizerSet : MonoBehaviour
{
    public GameObject insideOfHouse;
    public GameObject outsideOfHouse;
    public GameObject interfaceView;
    public GameObject textWindowView;
    public GameObject IconView;

    public JoyStickView joyStickView;
    public Joystick joystick;
    public GameObject blackView;

    void Start()
    {
        Init_PlayerMoveAndAnime();
        Init_CallBackManager();
        GameManager.Instance.Join_In_Game();
    }

    private void Init_PlayerMoveAndAnime()
    {
        PlayerMoveAndAnime.Instance.__interfaceView = joyStickView;
        PlayerMoveAndAnime.Instance.__joystick = joystick;
    }

    private void Init_CallBackManager()
    {
        CallBackManager.Instance.__insideOfHouse = insideOfHouse;
        CallBackManager.Instance.__outsideOfHouse = outsideOfHouse;

        CallBackManager.Instance.__interfaceView = interfaceView;
        CallBackManager.Instance.__textWindowView = textWindowView;
        CallBackManager.Instance.__iconView = IconView;

        CallBackManager.Instance.__BalckView = blackView;

        CallBackManager.Instance.__player = PlayerMoveAndAnime.Instance.gameObject;
    }
}
