using UnityEngine;
using System.Collections;

public class EnemyShooter : EnemyMonster
{
    float delay;
    bool isDelayed;

    public override void InitBundle()
    {
        base.InitBundle();
        damage = 5;
        hp = 30;
        moveSpeed = 0.1f;
        defeatScore = 10;
        delay = 3.0f;
        isDelayed = false;
    }

    //public override void WhenDamaged(GameObject bullet)
    //{
    //    if (bullet.tag == "MainBullet" || bullet.tag == "SubBullet")
    //        HpDown(bullet.GetComponent<Bullet>().damage);
    //    animator.SetTrigger("isDamaged");
    //    //if (animator.GetBool("isDamaged") == false)
    //    //    AnimeChange();
    //}

    public override void Move()
    {
        Vector3 pos = transform.position;
        if (pos.y > 3)
        {
            pos.y -= moveSpeed * Time.deltaTime * GameManager.Instance.gameSpeed;
        }

        float direction = Mathf.Sign(player.transform.position.x - pos.x); // 플레이어위치의 x축 방향성분 정규화
        pos.x += direction * moveSpeed * Time.deltaTime * GameManager.Instance.gameSpeed; // 플레이어와 같은 x축으로 이동
        gameObject.transform.position = pos;
    }


    protected override void Update()
    {
        //기본 동작
        base.Update();

        // 총알 발사 조건
        if (isDelayed == false && GameManager.Instance.gameTime > 3.0f)
        {
            try { StartCoroutine(BulletAttack()); }
            catch { Debug.LogError("객체없음"); }
        }
    }

    IEnumerator BulletAttack()
    {
        isDelayed = true;
        for (int i = 0; i < GameManager.Instance.gameDifficulty; i++)
        {
            EnemyBulletFactory.Instance.EnableShooterBullet(transform.position);
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(delay);
        isDelayed = false;
    }

    public override void OutOfScreen()
    {
        EnemyPoolShooter.Instance.ReturnObject(gameObject);
    }
}
