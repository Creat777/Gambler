using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting.ReorderableList;
using System.Collections;
using UnityEngine.Audio;

public class EnemyBoss : EnemyMonster
{
    TextAsset csvFile;
    List<List<float>> positionCsv;
    public AudioSource audioSource;
    public AudioClip[] clips;

    // 게임 실행중 고정변수
    float moveDelay;
    float nextMoveDelay;
    int maxHp;
    int MaxExplosionCount;

    // 조건변수(변경)
    int explosionCountDown;
    bool isNextMoveReady;
    bool isExplosion;

    private void Awake()
    {
        NormalVarInit();
        ConditionVarInit();
        gameObject.SetActive(false);
    }

    private void NormalVarInit()
    {
        float level = GameManager.Instance.gameDifficulty;
        maxHp = hp = (int)(500 * ((level - 1 / 2) + 1)); // 1배 ~ 3배
        damage = 20;
        defeatScore = 1000;
        MaxExplosionCount = 20;

        moveDelay = 1.0f;
        nextMoveDelay = 3.0f;
    }

    private void ConditionVarInit()
    {
        // 조건변수 초기화
        isNextMoveReady = false;
        isExplosion = false;
        explosionCountDown = MaxExplosionCount;
    }

    protected override void OnEnable()
    {
        ConditionVarInit();

        // 기본처리
        base.OnEnable();

        //csv파일 처리
        CsvManageFromGameManager();

        //등장 모션처리
        Vector3 appearancePosition = GameManager.Instance.FloatListToVector3(positionCsv[0]);
        StartCoroutine(moving(appearancePosition));
    }

    protected override void Update()
    {
        
        if (explosionCountDown == MaxExplosionCount && hp < maxHp * ((float)2 / 3))
        {
            explosionCountDown--;
            Vector3 effactPosition = transform.position + Vector3.left;
            AudioProcess();
            EffactPoolManager.Instance.GetObject(effactPosition);
            EffactPoolManager.Instance.GetObject(effactPosition + Vector3.up * 0.5f);
            EffactPoolManager.Instance.GetObject(effactPosition + Vector3.left * 0.5f);
            
        }
        else if (explosionCountDown == MaxExplosionCount-1 && hp < maxHp * ((float)1 / 3))
        {
            explosionCountDown--;
            Vector3 effactPosition = transform.position + Vector3.right;
            AudioProcess();
            EffactPoolManager.Instance.GetObject(effactPosition);
            EffactPoolManager.Instance.GetObject(effactPosition + Vector3.up * 0.5f);
            EffactPoolManager.Instance.GetObject(effactPosition + Vector3.right * 0.5f);
            
        }
        else if (hp <= 0) // 보스 사망시 점수처리하고 게임 종료됨
        {
            // 점수처리만 처리함
            if(explosionCountDown == MaxExplosionCount - 2)
            {
                MonsterDefeated();
            }

            // 게임 정지
            GameManager.Instance.isGamePause = true;

            // 이펙트 및 오디오 처리
            if(isExplosion == false)
            {
                AudioProcess();
                StartCoroutine(BossDefeatEffact(transform.position, 1));
            }

            // 이펙트 처리가 끝나면 게임 종료
            if(explosionCountDown == 0)
            {
                OutOfScreen();
            }
        }

        // 게임 종료시
        if (GameManager.Instance.isGameStart == false)
        {
            OutOfScreen();
        }

        // 보스 무브는 다른 Enemy랑 다르게 작동함
        if (isNextMoveReady && GameManager.Instance.isGamePause == false && hp>0)
        {
            Move();
        }

        // IdlE 복귀
        animationCount -= Time.deltaTime;
        if (animationCount < 0)
        {
            animator.Play("EnemyIdle");
        }
    }

    public override void MonsterDefeated()
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
    }

    IEnumerator BossDefeatEffact(Vector3 center, float radius)
    {
        isExplosion = true;

        // 각도 계산 (45도씩 회전)
        float angle = explosionCountDown * (360 / 8);
        float radian = angle * Mathf.Deg2Rad;  // 각도를 라디안으로 변환

        // X, Y 좌표 계산
        float x = center.x + radius * Mathf.Cos(radian); // 빗변*cos@
        float y = center.y + radius * Mathf.Sin(radian); // 빗변*sin@

        // 계산된 점 출력
        Vector3 point = new Vector3(x, y, center.z); // z값은 그대로 유지
        EffactPoolManager.Instance.GetObject(point);

        yield return new WaitForSeconds(0.1f);

        explosionCountDown--;
        isExplosion = false;
    }

    public void AudioProcess()
    {
        int index = explosionCountDown % 3;
        audioSource.volume = ButtonClickAudio.Instance.LoadVolumeData();
        audioSource.clip = clips[index];
        audioSource.Play();
    }

    public override void Move()
    {
        int randomIndex = Random.Range(0, positionCsv.Count);
        Vector3 movePosition = GameManager.Instance.FloatListToVector3(positionCsv[randomIndex]);
        if(transform.position != movePosition) 
        {
            StartCoroutine(moving(movePosition));
        }
    }

    IEnumerator moving(Vector3 movePosition)
    {
        isNextMoveReady = false;
        gameObject.transform.DOMove(movePosition, moveDelay);
        yield return new WaitForSeconds(nextMoveDelay);
        isNextMoveReady = true;
    }

    private void CsvManageFromGameManager()
    {
        csvFile = Resources.Load<TextAsset>("Enemy_Boss_1");
        positionCsv = new List<List<float>>();
        GameManager.Instance.PositionCsvLoading(csvFile, positionCsv);
    }


    public override void OutOfScreen()
    {
        StartCoroutine(StartDelay(3.0f));
    }

    IEnumerator StartDelay(float delay)
    {
        Vector3 position = GameManager.Instance.FloatListToVector3(positionCsv[0]);
        
        // 지정된 위치로 이동후 이펙트 표현
        
        yield return new WaitForSeconds(delay / 2);

        // 지정된 위치로 후퇴
        transform.DOMove(position + Vector3.up * 3, delay/2);
        yield return new WaitForSeconds(delay / 2);

        // 후퇴했으면 게임 종료
        gameObject.SetActive(false);
        GameManager.Instance.GameOver();
    }

    //public override void WhenDamaged(GameObject bullet)
    //{
    //    if (bullet.tag == "MainBullet" || bullet.tag == "SubBullet")
    //        HpDown(bullet.GetComponent<Bullet>().damage);

    //    if (animator.GetBool("isDamaged") == false && hp > 0)
    //        AnimeChange();
    //}
}
