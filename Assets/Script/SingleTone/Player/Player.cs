using Unity.VisualScripting;
using UnityEngine;

public class Player : Singleton<Player>
{


    // initializer ����
    public KeyBoardView keyBoardView { get; set; }

    // ������ ����
    public Rigidbody2D rigid;
    public Animator animator;
    public Vector2 moveVector;

    // ��ũ��Ʈ ����
    private float rayLength = 1f;
    public string animation_MoveKey { get; private set; }
    public float moveSpeed;
    public short Interactive;

    public string hitObjectName { get; private set; }


    //public GameObject TempImage;

    protected override void Awake()
    {
        base.Awake();
        animation_MoveKey = "isKeyDown";
        moveVector = Vector2.zero;
        Interactive = 1;
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

            // scene���� Ȯ��
            Debug.DrawRay(origin, dir, Color.red, rayLength);

            // ����ĳ��Ʈ�� Ȯ���� ���̾� ����
            int layerMask = LayerMask.GetMask("Interactive");

            // �ش� ���̾�� ó������ ����ĳ��Ʈ�� ��� ��ü�� ������ �о��
            RaycastHit2D hit = Physics2D.Raycast(origin, dir, rayLength, layerMask);

            if (hit.collider != null)
            {
                hitObjectName = hit.collider.gameObject.name;

                //TempImage.transform.position = Camera.main.WorldToScreenPoint(hit.transform.position);
                keyBoardView.Active_KeySpace();
            }
            else
            {
                hitObjectName = null;
                keyBoardView.Deactive_KeySpace();
            }
        }
    }
    public void InputSpaceBar()
    {
        if (Input.GetKeyDown(KeyCode.Space) && keyBoardView.isSpacebarOn)
        {
            keyBoardView.ChangeColer((int)KeyCode.Space);

            // ����ȭ ��������
            if(Interactive == 1)
            {
                Interactive--;
            }
            
        }
        else if (Input.GetKeyUp(KeyCode.Space) )
        {
            keyBoardView.revertColor((int)KeyCode.Space);
        }
    }
}

