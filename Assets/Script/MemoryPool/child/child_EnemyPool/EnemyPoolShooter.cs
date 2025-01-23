using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


// 싱글톤 : 게임 내에서 1개만 존재해야 하는 객체
public class EnemyPoolShooter : EnemyPool 

{
    public static EnemyPoolShooter Instance { get; private set; }
    protected override void MakeSingleTone()
    {
        // 싱글톤
        if (Instance == null)
        {
            Instance = this; // 스크립트 컴포넌트
        }
        else
        {   // 이미 인스턴스, 즉 싱글톤이 완성됐으면 현재의 객체는 싱글톤을 방해하는 객체이니 소멸시킴
            Destroy(gameObject); // 가비지컬렉터에 메모리 반납
            return;
        }
        DontDestroyOnLoad(gameObject); // 다른 씬으로 갈때 파괴되지 않음
    }

    //// 가비지 컬렉터 - 메모리 단편화 문제가 있을 수 있음
    //// 메모리풀 : 설정해놓은 범위 내에서 메모리를 사용하겠다.

    //// 싱글톤
    //public static EnemyPoolShooter Instance { get; private set; }

    //public GameObject enemyPrefab;

    //private Queue<GameObject> enemyPool;

    //void MakeSingleTone()
    //{
    //    // 싱글톤
    //    if (Instance == null)
    //    {
    //        Instance = this; // 스크립트 컴포넌트
    //    }
    //    else
    //    {   // 이미 인스턴스, 즉 싱글톤이 완성됐으면 현재의 객체는 싱글톤을 방해하는 객체이니 소멸시킴
    //        Destroy(gameObject); // 가비지컬렉터에 메모리 반납
    //        return;
    //    }
    //    DontDestroyOnLoad(gameObject); // 다른 씬으로 갈때 파괴되지 않음
    //    // 씬을 옮길때 객체가 파괴되지 않으면 원래 씬으로 복귀할때 새로운 객체가 만들어진다.
    //    // 싱글톤에서 이 경우 Destroy(gameObject);로 복사가 되는 불상사를 막을 수 있다.
    //}
    //private void Awake()
    //{
    //    MakeSingleTone();
    //    enemyPool = new Queue<GameObject>();
    //}


    //public void InitializePool(int size = 5)
    //{
    //    enemyPool.Clear();
    //    for (int i = 0; i < size; i++)
    //        CreateNewEnemy();
    //}

    //private void CreateNewEnemy()
    //{
    //    GameObject enemyObject = Instantiate(enemyPrefab);
    //    EnemyMonster enemyScript = enemyObject.GetComponent<EnemyMonster>();
    //    enemyScript.Deactive();
    //    enemyPool.Enqueue(enemyObject);
    //}

    //public GameObject GetEnemy(Vector3 position)
    //{
    //    if(enemyPool.Count == 0)
    //    {
    //        CreateNewEnemy();
    //    }

    //    GameObject enemy = enemyPool.Dequeue();
    //    enemy.GetComponent<EnemyMonster>().Initalize(position);
    //    return enemy;
    //}

    //public void ReturnEnemy(GameObject obj, AudioClip clip)
    //{
    //    EffactPoolManager.instance.GetBombEffact(obj.transform.position, clip);
    //    obj.GetComponent<EnemyMonster>().Deactive();
    //    enemyPool.Enqueue(obj);
    //}
}
