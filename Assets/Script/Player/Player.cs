using System.Collections;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    private void MakeSingleTone()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }


    public KeyBoardView keyBoardView { get; set; }

    public Rigidbody2D rigid;
    public float moveSpeed;
    bool moveDone;

    

    private void Awake()
    {
        MakeSingleTone();
        moveDone = true;
    }


    void Start()
    {

    }

    void Update()
    {
        Move();
        InputSpaceBar();
    }

    public void Move()
    {
        if(moveDone)
        {
            // 위쪽
            if (Input.GetKey(KeyCode.W))
            {
                keyBoardView.ChangeColer((int)KeyCode.W);
                rigid.AddForceY(moveSpeed, ForceMode2D.Impulse);
            }
            else if (Input.GetKeyUp(KeyCode.W))
            {
                keyBoardView.revertColor((int)KeyCode.W);

            }

            //아래쪽
            if (Input.GetKey(KeyCode.S))
            {
                keyBoardView.ChangeColer((int)KeyCode.S);
                rigid.AddForceY(-moveSpeed, ForceMode2D.Impulse);
            }
            else if (Input.GetKeyUp(KeyCode.S))
            {
                keyBoardView.revertColor((int)KeyCode.S);
            }

            // 왼쪽
            if (Input.GetKey(KeyCode.A))
            {
                keyBoardView.ChangeColer((int)KeyCode.A);
                rigid.AddForceX(-moveSpeed, ForceMode2D.Impulse);
            }
            else if (Input.GetKeyUp(KeyCode.A))
            {
                keyBoardView.revertColor((int)KeyCode.A);
            }

            // 오른쪽
            if (Input.GetKey(KeyCode.D))
            {
                keyBoardView.ChangeColer((int)KeyCode.D);
                rigid.AddForceX(moveSpeed, ForceMode2D.Impulse);
            }
            else if (Input.GetKeyUp(KeyCode.D))
            {
                keyBoardView.revertColor((int)KeyCode.D);
            }
        }
        StartCoroutine(MoveDelay());
    }

    IEnumerator MoveDelay()
    {
        moveDone = false;
        yield return new WaitForSeconds(1/ GameManager.Instance.gameSpeed);
        moveDone = true;
    }
    

    public void InputSpaceBar()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            keyBoardView.ChangeColer((int)KeyCode.Space);
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            keyBoardView.revertColor((int)KeyCode.Space);
        }
    }
}

