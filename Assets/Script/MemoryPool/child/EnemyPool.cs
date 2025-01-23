using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public abstract class EnemyPool : MemoryPool
{
    protected AudioSource audioSource { get; set; }

    protected override void Awake()
    {
        base.Awake();
        audioSource = GetComponent<AudioSource>();
    }

    protected override void CreateNewObject()
    {
        GameObject enemy = Instantiate(Prefab);
        EnemyMonster enemyScript = enemy.GetComponent<EnemyMonster>();
        enemyScript.Deactive();
        if (memoryPool != null)
        {
            memoryPool.Enqueue(enemy);
            //Debug.Log("enemyPool 객체가 존재함");
        }
        else
        {
            Debug.LogError("enemyPool 객체가 존재하지 않음");
        }
    }

    public override GameObject GetObject(Vector3 position)
    {
        if (memoryPool != null)
        {
            if (memoryPool.Count == 0)
            {
                CreateNewObject();
            }
            //Debug.Log(enemyPool.Count);
            GameObject enemy = memoryPool.Dequeue();
            enemy.GetComponent<EnemyMonster>().Initalize(position);
            return enemy;
        }
        else
        {
            Debug.LogError("enemyPool 객체가 존재하지 않음");
            return null;
        }
    }

    public override void ReturnObject(GameObject obj)
    {
        EffactPoolManager.Instance.GetObject(obj.transform.position);
        audioSource.volume = ButtonClickAudio.Instance.LoadVolumeData();
        audioSource.Play();
        obj.GetComponent<EnemyMonster>().Deactive();
        memoryPool.Enqueue(obj);
    }

}
