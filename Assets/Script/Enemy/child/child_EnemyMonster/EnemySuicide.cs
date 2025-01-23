using UnityEngine;

public class EnemySuicide : EnemyMonster
{
    public override void InitBundle()
    {
        base.InitBundle();
        damage = 1;
        hp = 3;
        moveSpeed = 0.3f;
        defeatScore = 1;
    }

    protected override void Update()
    {
        //기본 동작
        base.Update();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.gameObject == player)
        {
            EnemyPoolSuicideBomber.Instance.ReturnObject(gameObject);
        }
    }

    public override void Move()
    {
        Vector3 pos = transform.position;
        Vector3 direction = (player.transform.position - pos).normalized; // 플레이어방향의 단위벡터
        pos += direction * moveSpeed * Time.deltaTime * GameManager.Instance.gameSpeed;
        gameObject.transform.position = pos;
    }

    //public override void WhenDamaged(GameObject bullet)
    //{
    //    if (bullet.tag == "MainBullet")
    //        HpDown(bullet.GetComponent<MainBullet>().damage);
    //    else if (bullet.tag == "SubBullet")
    //        HpDown(bullet.GetComponent<SubBullet>().damage);
    //}

    public override void OutOfScreen()
    {
        EnemyPoolSuicideBomber.Instance.ReturnObject(gameObject);
    }
}
