using UnityEngine;
using PublicSet;
public enum StateCode
{
    Idle = 0,
    Up = 1,
    Down = 2,
    Left = 3,
    Right = 4,
}

public class Player_MoveAndAnime : MonoBehaviour
{
    // ������ ����
    [SerializeField] private InteractionButton interactionButton;
    [SerializeField] private Joystick joystick;

    // ������ ����
    public Rigidbody2D rigid;
    public Animator animator;
    public Vector2 dirVec;
    public Vector2 curMoveVec;
    public Vector2 joystickVec;

    // ��ũ��Ʈ ����
    private float rayLength = 1f;
    //public string animation_MoveKey { get; private set; }
    public float moveSpeed;
    public short Interactive;
    bool isStop = false;
    public StateCode curState = StateCode.Idle;
    StateCode lastState = StateCode.Idle;

    public GameObject hitObject { get; private set; }


    protected void Awake()
    {
        curMoveVec = Vector2.zero;
        Interactive = 1;
    }


    void Start()
    {

    }

    private void FixedUpdate()
    {
        if(GameManager.Instance.currentScene == eScene.InGame)
        {
            if(GameManager.Instance.isGamePause == false)
            {
                Move_And_Animation_Joystick();
                Interact_With_Object();
            }
        }
    }
    private void Interact_With_Object()
    {
        // ������ ������¿� ���� ���⺤�Ͱ� ������
        switch (curState)
        {
            case StateCode.Up : dirVec = Vector2.up; break;
            case StateCode.Down: dirVec = Vector2.down; break;
            case StateCode.Left: dirVec = Vector2.left; break;
            case StateCode.Right: dirVec = Vector2.right; break;
        }

        Vector3 origin = transform.position;

        // scene���� Ȯ��
        //Debug.DrawRay(origin, dir, Color.red, rayLength);

        // ����ĳ��Ʈ�� Ȯ���� ���̾� ����
        int layerMask = LayerMask.GetMask("Interactable");

        // �ش� ���̾�� ó������ ����ĳ��Ʈ�� ��� ��ü�� ������ �о��
        RaycastHit2D hit = Physics2D.Raycast(origin, dirVec, rayLength, layerMask);

        // �������� ���鿡 ��ȣ�ۿ��� ��ü�� ������ �� ��ü�� �̸��� �а� ��ȣ�ۿ��� �ɼ��� ��
        if (hit.collider != null)
        {
            hitObject = hit.collider.gameObject;

            //TempImage.transform.position = Camera.main.WorldToScreenPoint(hit.transform.position);
            interactionButton.TryActivate_Button();
        }
        // �׷��� ������ ��ȣ�ۿ� �ɼ��� ��
        else
        {
            interactionButton.TryDeactivate_Button();
        }
    }

    private void Move_And_Animation_Joystick()
    {
        // ���̽�ƽ ���� �Է�
        joystickVec = joystick.Direction;

        // ���� ����
        isStop = joystickVec.magnitude <= 0.9f ? true : false;

        // X�� �Ǵ� Y�� ���и� �����
        joystickVec = (joystickVec.x * joystickVec.x > joystickVec.y * joystickVec.y) ?
            new Vector2(joystickVec.x, 0) : new Vector2(0, joystickVec.y);

        // ���� ������ ũ�⸦ 1�� ���� ���� ���꺤�ͷ� ���
        curMoveVec = joystickVec.normalized;
            //(joystickVec.x * joystickVec.x > joystickVec.y * joystickVec.y) ? // x���� ���밪�� y���� ���밪���� ũ��
            //new Vector2(joystickVec.x/ Mathf.Abs(joystickVec.x), 0) : new Vector2(0, joystickVec.y / Mathf.Abs(joystickVec.y));

        // ĳ���� �̵�
        // �̵� ������ �ʱ� joystickVec���� �ϳ� �������� ���� curMoveVec�� ���
        if (joystick.Direction.magnitude >= 0.9f)
        {
            if(moveSpeed>1)
            {
                rigid.AddForce(curMoveVec * moveSpeed / 10, ForceMode2D.Impulse);
            }
            else
            {
                Debug.LogWarning($"addPulse ���� -> �̵��ӵ� : {moveSpeed}");
            }
        }


        // �����ڵ� ��ȯ �� �񱳷� �ִϸ��̼� ��ȯ
        animationProc();

        // �ӵ��� �پ��ų� �������¶� �޶�������
        if (curState != lastState || isStop)
        {
            rigid.linearVelocity = Vector2.zero;
        }

        // ��� ó���� ������ ��ȭ�� ���¸� ����
        lastState = curState;
    }
    
    public void animationProc()
    {
        // ���� �����̶� �����ϸ� �������� ���º�ȯ
        // �׷��� �ʴٸ� idle�� ���� ��ȯ
        if (joystickVec.y >= 0.1f)
        {
            curState = StateCode.Up;
        }
        else if (joystickVec.y <= -0.1f)
        {
            curState = StateCode.Down;
        }
        else if (joystickVec.x <= -0.1f)
        {
            curState = StateCode.Left;
        }
        else if (joystickVec.x >= 0.1f)
        {
            curState = StateCode.Right;
        }
        else //if(joystickVec.magnitude < 0.1f)
        {
            curState = StateCode.Idle;
        }

        // �����ڵ尡 ���������� �ִϸ����Ϳ� �ݿ�
        animator.SetInteger("stateCode", (int)curState);

        // ĳ���Ͱ� �̵��� ������
        if ( isStop && animator.GetBool("isMove"))
        {
            animator.SetBool("isMove", false);
        }

        // �ٸ� ������ idle ����
        if (curState != lastState)
        {
            animator.SetTrigger("start");
        }

        // ĳ���Ͱ� ������ �����̰� �ִ� ���
        if (rigid.linearVelocity.magnitude >= 0.1f)
        {
            // ĳ���Ͱ� �̵��� ������ �ٲ۰�� �ٸ� �ִϸ��̼����� ��ü
            if (curState != lastState)
            {
                animator.SetTrigger("start");
            }
            // ���� ���¿��� �̵��� �����ϴ� ��� �ִϸ��̼� ��ü
            else if (isStop == false && animator.GetBool("isMove") == false)
            {
                animator.SetBool("isMove", true);
                animator.SetTrigger("start");
            }
        } 
        // ������ �������� �ʾ����� ����
        else if(rigid.linearVelocity.magnitude < 0.1f)
        {
            animator.SetBool("isMove", false);
        }
    }
}

