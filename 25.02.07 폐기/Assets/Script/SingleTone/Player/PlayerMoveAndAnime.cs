using UnityEngine;

public enum StateCode
{
    Idle = 0,
    Up = 1,
    Down = 2,
    Left = 3,
    Right = 4,
}

public class PlayerMoveAndAnime : Singleton<PlayerMoveAndAnime>
{

    // 에디터 편집
    public Rigidbody2D rigid;
    public Animator animator;
    public Vector2 dirVec;
    public Vector2 curMoveVec;
    public Vector2 joystickVec;

    // 스크립트 편집
    private float rayLength = 1f;
    //public string animation_MoveKey { get; private set; }
    public float moveSpeed;
    public short Interactive;
    bool isStop = false;
    public StateCode curState = StateCode.Idle;
    StateCode lastState = StateCode.Idle;

    public GameObject hitObject { get; private set; }


    //public GameObject TempImage;

    protected override void Awake()
    {
        base.Awake();
        //animation_MoveKey = "isKeyDown";
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
            Move_And_Animation_Joystick();
            Interact_With_Object();
        }
    }
    private void Interact_With_Object()
    {
        // 마지막 현재상태에 따라 방향벡터가 결정됨
        switch (curState)
        {
            case StateCode.Up : dirVec = Vector2.up; break;
            case StateCode.Down: dirVec = Vector2.down; break;
            case StateCode.Left: dirVec = Vector2.left; break;
            case StateCode.Right: dirVec = Vector2.right; break;
        }

        Vector3 origin = transform.position;

        // scene으로 확인
        //Debug.DrawRay(origin, dir, Color.red, rayLength);

        // 레이캐스트로 확인할 레이어 선택
        int layerMask = LayerMask.GetMask("Interactable");

        // 해당 레이어에서 처음으로 레이캐스트에 닿는 객체의 정보를 읽어옴
        RaycastHit2D hit = Physics2D.Raycast(origin, dirVec, rayLength, layerMask);

        // 움직여서 정면에 상호작용한 객체가 있으면 그 객체의 이름을 읽고 상호작용이 옵션을 켬
        if (hit.collider != null)
        {
            hitObject = hit.collider.gameObject;

            //TempImage.transform.position = Camera.main.WorldToScreenPoint(hit.transform.position);
            GameManager.Instance.connector.joyStickView.InteractButton_On();
        }
        // 그렇지 않으면 상호작용 옵션을 끔
        else
        {
            GameManager.Instance.connector.joyStickView.InteractButton_Off();
        }
    }

    private void Move_And_Animation_Joystick()
    {
        // 조이스틱 벡터 입력
        joystickVec = GameManager.Instance.connector.joystick.Direction;

        // 멈춤 조건
        isStop = joystickVec.magnitude <= 0.9f ? true : false;

        // X축 또는 Y축 성분만 남기기
        joystickVec = (joystickVec.x * joystickVec.x > joystickVec.y * joystickVec.y) ?
            new Vector2(joystickVec.x, 0) : new Vector2(0, joystickVec.y);

        // 남은 성분의 크기를 1로 만들어서 현재 무브벡터로 사용
        curMoveVec = joystickVec.normalized;
            //(joystickVec.x * joystickVec.x > joystickVec.y * joystickVec.y) ? // x축의 절대값이 y축의 절대값보다 크면
            //new Vector2(joystickVec.x/ Mathf.Abs(joystickVec.x), 0) : new Vector2(0, joystickVec.y / Mathf.Abs(joystickVec.y));

        // 캐릭터 이동
        // 이동 조건은 초기 joystickVec으로 하나 가해지는 힘은 curMoveVec을 사용
        if (GameManager.Instance.connector.joystick.Direction.magnitude >= 0.9f)
        {
            //Debug.Log("addPulse 적용");
            rigid.AddForce(curMoveVec * moveSpeed / 10, ForceMode2D.Impulse);
        }


        // 상태코드 전환 및 비교로 애니메이션 전환
        animationProc();

        // 속도가 줄었거나 이전상태랑 달라졌으면
        if (curState != lastState || isStop)
        {
            rigid.linearVelocity = Vector2.zero;
        }

        // 모든 처리가 끝나고 변화된 상태를 저장
        lastState = curState;
    }
    
    public void animationProc()
    {
        // 한쪽 성분이라도 존재하면 그쪽으로 상태변환
        // 그렇지 않다면 idle로 상태 변환
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

        // 상태코드가 결정됐으면 애니메이터에 반영
        animator.SetInteger("stateCode", (int)curState);

        // 캐릭터가 이동중 멈춘경우
        if ( isStop && animator.GetBool("isMove"))
        {
            animator.SetBool("isMove", false);
        }

        // 다른 상태의 idle 실행
        if (curState != lastState)
        {
            animator.SetTrigger("start");
        }

        // 캐릭터가 실제로 움직이고 있는 경우
        if (rigid.linearVelocity.magnitude >= 0.1f)
        {
            // 캐릭터가 이동중 방향을 바꾼경우 다른 애니메이션으로 교체
            if (curState != lastState)
            {
                animator.SetTrigger("start");
            }
            // 멈춘 상태에서 이동을 시작하는 경우 애니메이션 교체
            else if (isStop == false && animator.GetBool("isMove") == false)
            {
                animator.SetBool("isMove", true);
                animator.SetTrigger("start");
            }
        } 
        // 실제로 움직이지 않았으면 정지
        else if(rigid.linearVelocity.magnitude < 0.1f)
        {
            animator.SetBool("isMove", false);
        }
    }

}

