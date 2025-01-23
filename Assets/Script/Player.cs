using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private Rigidbody2D rigid;
    public Animator animator;
    public RectTransform uiHpBar;
    private Slider hpBarSlider;
    public Sprite plyaerSprite;
    private float playerHeight;
    public int hp;
    public int hpMax;

    public Joystick joystick;

    private void Awake()
    {
        //makeSingleTone();
    }
    void Start()
    {
        rigid = gameObject.GetComponent<Rigidbody2D>();
        playerHeight = plyaerSprite.bounds.size.y;
        hpMax = 100;
        hpBarSlider = uiHpBar.GetComponent<Slider>();
    }

    void Update()
    {
        PlyaerAnimation();
        //UpDownClamp();
        PlyaerMove();
        PlayerHpUi();
        if(hp<0 && GameManager.Instance.isGameStart == true)
        {
            GameManager.Instance.GameOver();
        }
        if(Input.GetKeyUp(KeyCode.Space))
        {
            BulletFactory.Instance.EnableBullets();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject enemy = collision.gameObject;
        if (enemy.tag == "Enemy" || enemy.tag == "EnemyBullet")
        {
            OnDamaged(enemy);
            if (enemy.tag == "EnemyBullet")
                enemy.GetComponent<EnemyBullet>().OutOfScreen();
        }
    }

    public void OnDamaged(GameObject obj)
    {
        hp -= obj.GetComponent<Enemy>().damage;
    }
    public void PlayerHpUi()
    {
        // 플레이어의 world 좌표가 UI 좌표로 변환
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, playerHeight, 0));

        uiHpBar.position = screenPos;
        hpBarSlider.value = (float)hp/hpMax;
    }

    void PlyaerMove()
    {
        // 방향
        float moveX, moveY;
        Vector2 moveDirection;
        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        { 
            moveX = Input.GetAxisRaw("Horizontal");
            moveY = Input.GetAxisRaw("Vertical");
            moveDirection = new Vector2(moveX, moveY).normalized;
        }
        else
        {
            moveDirection = joystick.Direction;
        }
        // 적용
        rigid.AddForce(moveDirection * GameManager.Instance.gameSpeed * Time.deltaTime, ForceMode2D.Impulse);
    }

    void PlyaerAnimation()
    {
        if(joystick.Horizontal < 0 || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            //Debug.Log("왼쪽 입력");
            animator.SetInteger("direction_int", -1);
        }
        else if (joystick.Horizontal > 0 || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            //Debug.Log("오른쪽 입력");
            animator.SetInteger("direction_int", 1);
        }
        else animator.SetInteger("direction_int", 0);
    }

    //void UpDownClamp()
    //{
    //    Vector3 pos = transform.position;

    //    // 화면의 상단 하단
    //    float screenTop = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y; // 카메라의 상단 좌표
    //    float screenBottom = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).y; // 카메라의 하단 좌표

    //    // 위치 제한 (화면 밖으로 나가지 않게 제한)
    //    pos.y = Mathf.Clamp(pos.y, screenBottom, screenTop);

    //    transform.position = pos;
    //}
    
}
