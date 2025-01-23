using UnityEngine;

public class CoinMemoryPool : MemoryPool
{
    public static CoinMemoryPool Instance { get; private set; }
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
        // 씬을 옮길때 객체가 파괴되지 않으면 원래 씬으로 복귀할때 새로운 객체가 만들어진다.
        // 싱글톤에서 이 경우 Destroy(gameObject);로 복사가 되는 불상사를 막을 수 있다.
    }

    AudioSource audioSource;

    protected override void Awake()
    {
        base.Awake();
        audioSource = GetComponent<AudioSource>();
    }

    public override void ReturnObject(GameObject obj)
    {
        audioSource.volume = ButtonClickAudio.Instance.LoadVolumeData();
        audioSource.Play();
        obj.SetActive(false);
        memoryPool.Enqueue(obj);
    }
}
