using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class BulletFactory : MonoBehaviour
{
    
    public GameObject mainBulletPrefab;
    public GameObject subBulletPrefab;
    private Queue<GameObject> mainBulletPool;
    private Queue<GameObject> subBulletPool;

    // 씬 전환시 MainInitilizer에서 연결
    public GameObject player;
    public float playerHeight;
    public float playerWidth;

    public static BulletFactory Instance { get; private set; }
    void makeSingleTone()
    {
        if (Instance == null)
        {
            Instance = this; // 스크립트 컴포넌트
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Awake()
    {
        makeSingleTone();
        mainBulletPool = new Queue<GameObject>();
        subBulletPool = new Queue<GameObject>();
    }
    void Start()
    {
        if (player != null)
        {
            
        }
    }

    public void InitializePool(int startSize = 5)
    {
        mainBulletPool.Clear();
        subBulletPool.Clear();
        for (int i = 0; i < startSize; i++)
        {
            CreateBullet();
        }
    }

    private void CreateBullet()
    {
        CreateMainBullet();
        CreateSubBullet();
    }

    private void CreateMainBullet()
    {
        GameObject bulletObject = Instantiate(mainBulletPrefab);
        bulletObject.GetComponent<Bullet>().SetDeActive();
        mainBulletPool.Enqueue(bulletObject);
    }
    private void CreateSubBullet()
    {
        GameObject subLeft = Instantiate(subBulletPrefab);
        GameObject subRight = Instantiate(subBulletPrefab);
        subLeft.GetComponent<Bullet>().SetDeActive();
        subRight.GetComponent<Bullet>().SetDeActive();
        subBulletPool.Enqueue(subLeft);
        subBulletPool.Enqueue(subRight);
    }

    public void EnableBullets()
    {
        if(player)
        if (mainBulletPool.Count == 0 || subBulletPool.Count == 0)
        {
            CreateBullet();
        }

        EnableMainBullet();
        EnableSubBullet();
    }
    private void EnableMainBullet()
    {
        if (mainBulletPool.Count == 0)
        {
            CreateBullet();
        }
        GameObject mainBullet = mainBulletPool.Dequeue();
        mainBullet.GetComponent<Bullet>().SetActive();
        mainBullet.transform.position = player.transform.position + new Vector3(0, playerHeight / 2, 0);
    }

    private void EnableSubBullet()
    {
        if (subBulletPool.Count <= 2)
        {
            CreateBullet();
        }
        GameObject subLeft = subBulletPool.Dequeue();
        GameObject subRight = subBulletPool.Dequeue();
        subLeft.GetComponent<Bullet>().SetActive();
        subRight.GetComponent<Bullet>().SetActive();
        subLeft.transform.position = player.transform.position + new Vector3(-playerWidth / 2, playerHeight / 2, 0);
        subRight.transform.position = player.transform.position + new Vector3(playerWidth / 2, playerHeight / 2, 0);
    }

    public void ReturnMainBullet(GameObject obj)
    {
        obj.SetActive(false);
        mainBulletPool.Enqueue(obj);
    }

    public void ReturnSubBullet(GameObject obj)
    {
        obj.SetActive(false);
        subBulletPool.Enqueue(obj);
    }

    void Update()
    {

    }


}
