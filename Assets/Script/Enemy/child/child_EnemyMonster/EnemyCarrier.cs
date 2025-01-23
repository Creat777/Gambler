using System.Drawing;
using UnityEngine;

public class EnemyCarrier : EnemyMonster
{
    private Camera mainCamera;
    //float screenBottom;

    // 사라질 때 소환될 suicideBomber의 위치를 결정하기 위한 데이터
    public float radius; // 원의 반지름

    public override void InitBundle()
    {
        base.InitBundle();
        damage = 10;
        hp = 30;
        moveSpeed = 0.1f;
        defeatScore = 10;
        radius = 1f;

        mainCamera = Camera.main;
        //screenBottom = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, 0)).y; // 카메라의 하단 좌표
        //Debug.Log("carrier screen bottom : " + screenBottom);
    }

    //public override void WhenDamaged(GameObject bullet)
    //{
    //    if (bullet.tag == "MainBullet")
    //        HpDown(bullet.GetComponent<MainBullet>().damage);
    //    else if (bullet.tag == "SubBullet")
    //        HpDown(bullet.GetComponent<SubBullet>().damage);
    //    if (animator.GetBool("isDamaged") == false)
    //        AnimeChange();
    //}

    protected override void Update()
    {
        // 기본 동작
        base.Update();

        // 목적지에 도착했을 때
        if (player.transform.position.y > gameObject.transform.position.y)
        {
            OutOfScreen();
        }
    }

    public void CallSuicideBomber(Vector3 center, float radius)
    {
        int numOfSuicideBomber = 8;
        // 8개의 점을 구하기 위한 반복문
        for (int i = 0; i < numOfSuicideBomber; i++)
        {
            // 각도 계산 (45도씩 회전)
            float angle = i * (360 / numOfSuicideBomber);
            float radian = angle * Mathf.Deg2Rad;  // 각도를 라디안으로 변환

            // X, Y 좌표 계산
            float x = center.x + radius * Mathf.Cos(radian); // 빗변*cos@
            float y = center.y + radius * Mathf.Sin(radian); // 빗변*sin@

            // 계산된 점 출력
            Vector3 point = new Vector3(x, y, center.z); // z값은 그대로 유지
            //Debug.Log("Point " + i + ": " + point);
            EnemyPoolSuicideBomber.Instance.GetObject(point);
        }
        
    }

    public override void Move()
    {
        Vector3 pos = transform.position;
        pos.y -= Time.deltaTime * moveSpeed * GameManager.Instance.gameSpeed;
        gameObject.transform.position = pos;
    }

    public override void OutOfScreen()
    {
        CallSuicideBomber(transform.position, radius);
        EnemyPoolCarrier.Instance.ReturnObject(gameObject);
    }
}
