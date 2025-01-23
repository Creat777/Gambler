using UnityEngine;
using UnityEngine.Analytics;
using System.Collections;
using System;
using Unity.VisualScripting;
using System.Threading;
using UnityEngine.Audio;

public abstract class EnemyMonster : Enemy
{
    [SerializeField] protected int hp;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float defeatScore;
    [SerializeField] public GameObject scorePanel;
    [SerializeField] public Animator animator;

    protected float animationCount;
    protected float animationDelay;
    protected virtual void Start()
    {
        InitBundle();
    }

    protected virtual void OnEnable()
    {
        InitBundle();
    }
    public virtual void InitBundle()
    {
        player = GameObject.FindWithTag("Player");
        scorePanel = GameObject.Find("ScorePanel");
        animator = gameObject.GetComponent<Animator>();
        animationCount = animationDelay = 1.0f;
    }

    public void Initalize(Vector3 position)
    {
        gameObject.SetActive(true);
        transform.position = position;
    }

    public void Deactive()
    {
        gameObject.SetActive(false);
    }

    public void HpDown(int damage)
    {
        hp -= damage;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "MainBullet" || collision.gameObject.tag == "SubBullet")
        {
            WhenDamaged(collision.gameObject);
        }
    }
   

    public void AnimeChange()
    {
        StartCoroutine(SwitchAnimation());
    }

    IEnumerator SwitchAnimation()
    {
        //animator.SetBool("isDamaged", true);
        yield return new WaitForSeconds(animationDelay); // WaitForSeconds은 코루틴에서만 사용 가능
        // 지정된 시간(animationDelay)이 지난 후 이 지점에서 다시 코루틴 실행
        //animator.SetBool("isDamaged", false);
    }


    public virtual void MonsterDefeated()
    {
        if (scorePanel != null)
        {
            scorePanel.GetComponent<Score>().bonusScore += defeatScore;
            //Debug.Log($"점수 추가 : {defeatScore}");
        }
        else
        {
            Debug.LogError("scorePanel 참조 실패");
        }
        OutOfScreen();
    }


    protected virtual void Update()
    {
        Move();

        // 몬스터 사망 시
        if (hp <= 0)
        {
            MonsterDefeated();
        }

        // 게임 종료시
        if(GameManager.Instance.isGameStart == false)
        {
            OutOfScreen();
        }

        // IdlE 복귀
        animationCount -= Time.deltaTime;
        if (animationCount < 0)
        {
            animator.Play("EnemyIdle");
        }
    }

    public abstract void Move();
    
    public virtual void WhenDamaged(GameObject bullet)
    {
        if (hp > 0)
        {
            animationCount = animationDelay;
            HpDown(bullet.GetComponent<Bullet>().damage);
            animator.Play("EnemyDamaged");
        }
    }




}
