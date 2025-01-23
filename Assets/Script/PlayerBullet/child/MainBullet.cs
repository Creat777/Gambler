using UnityEngine;

public class MainBullet : Bullet
{
    

    private void Start()
    {
        damage = 10;
    }

    public override void OutOfScreen()
    {
        //Debug.Log(screenTop.ToString());
        //Debug.Log("ÃÑ¾Ë ¼Ò¸ê");
        BulletFactory.Instance.ReturnMainBullet(gameObject);
    }
}
