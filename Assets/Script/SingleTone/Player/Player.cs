using Unity.VisualScripting;
using UnityEngine;

public class Player : Singleton<Player>
{


    public KeyBoardView keyBoardView { get; set; }
    public Rigidbody2D rigid;
    public Animator animator;
    public Vector2 moveVector;
    private float rayLength = 1f;
    public string animation_MoveKey = "isKeyDown";
    public float moveSpeed;
    public GameObject bed;
    public GameObject TempImage;
    public GameObject hitObject;

    

    protected override void Awake()
    {
        base.Awake();
        moveVector = Vector2.zero;
    }


    void Start()
    {

    }

    void Update()
    {
        Move_And_Animation();
        Interact_With_Object();
        InputSpaceBar();
    }

    private void Move_And_Animation()
    {
        int keyCodeInt = 0;

        // Ű�� ������ moveVector���� �����Ű��
        // Ű�� ���� moveVector�� �ش���ǥ���� ����

        // ����
        if (Input.GetKeyDown(KeyCode.W))
        {
            keyCodeInt = (int)KeyCode.W;
            keyBoardView.ChangeColer(keyCodeInt);
            animator.SetBool(animation_MoveKey, true);

            moveVector = Vector2.up;
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            keyCodeInt = (int)KeyCode.W;
            keyBoardView.revertColor(keyCodeInt);
            animator.SetBool(animation_MoveKey, false);

            moveVector.y -= moveVector.y;
        }

        //�Ʒ���
        else if (Input.GetKeyDown(KeyCode.S))
        {
            keyCodeInt = (int)KeyCode.S;
            keyBoardView.ChangeColer(keyCodeInt);
            animator.SetBool(animation_MoveKey, true);

            moveVector = Vector2.down;
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            keyCodeInt = (int)KeyCode.S;
            keyBoardView.revertColor(keyCodeInt);
            animator.SetBool(animation_MoveKey, false);

            moveVector.y -= moveVector.y;
        }

        // ����
        else if (Input.GetKeyDown(KeyCode.A))
        {
            keyCodeInt = (int)KeyCode.A;
            keyBoardView.ChangeColer(keyCodeInt);
            animator.SetBool(animation_MoveKey, true);

            moveVector = Vector2.left;
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            keyCodeInt = (int)KeyCode.A;
            keyBoardView.revertColor(keyCodeInt);
            animator.SetBool(animation_MoveKey, false);

            moveVector.x -= moveVector.x;
        }

        // ������
        else if (Input.GetKeyDown(KeyCode.D))
        {
            keyCodeInt = (int)KeyCode.D;
            keyBoardView.ChangeColer(keyCodeInt);
            animator.SetBool(animation_MoveKey, true);

            moveVector = Vector2.right;
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            keyCodeInt = (int)KeyCode.D;
            keyBoardView.revertColor(keyCodeInt);
            animator.SetBool(animation_MoveKey, false);

            moveVector.x -= moveVector.x;
        }

        // �ִϸ��̼� �ڵ�
        animator.SetInteger("animeCode", keyCodeInt);

        // ĳ���� ������
        rigid.AddForce(moveVector * moveSpeed/10, ForceMode2D.Impulse);
        if(moveVector == Vector2.zero)
        {
            rigid.linearVelocity = Vector2.zero;
        }
    }

    private void Interact_With_Object()
    {
        //Vector3 direction = transform.position + new Vector3(moveVector.x, moveVector.y, 0);

        if(moveVector!= Vector2.zero)
        {
            Vector3 dir = moveVector.normalized;
            Vector3 origin = transform.position;
            Debug.DrawRay(origin, dir, Color.red, rayLength);
            int layerMask = LayerMask.GetMask("Interactive");
            RaycastHit2D hit = Physics2D.Raycast(origin, dir, rayLength, layerMask);

            if (hit.collider != null)
            {
                hitObject = hit.collider.gameObject;
                //TempImage.transform.position = Camera.main.WorldToScreenPoint(hit.transform.position);
                keyBoardView.Active_KeySpace();
            }
            else
            {
                hitObject = null;
                keyBoardView.Deactive_KeySpace();
            }
        }
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

