using Unity.VisualScripting;
using UnityEngine;

public abstract class Singleton<T_Class> : MonoBehaviour where T_Class : MonoBehaviour
{
    // 프로세스 내에 오직 하나만 존재하는 객체 == 싱글톤  : 0개도 아니고 2개 이상도 아니고 오직 1개
    // 왜? 만들까?
    //  -> 1번 메모리 단편화 방지
    //  -> 2번 프로시저 처리 중복 방지
    //  -> 3번 클래스이름으로 프로시저에 접근할 수 있음

    // static 변수  : 클래스로 만들어지는 객체와 별개로 오직 한개만 존재하는 변수
    //              : 클래스 이름으로 접근할 수 있음
    private static T_Class instance;

    private static readonly object _lock = new object();

    public static T_Class Instance
    {
        get
        {
            lock (_lock)
            {
                if (instance == null)
                {
                    Debug.LogWarning($"{typeof(T_Class).Name}싱글톤이 아직 생성되지 않았습니다.");
                    return null;
                }
                // null이 아니면 그대로 참조
                return instance;
            }
                
        }

        protected set
        {
            instance = value;
        }
    }

    public virtual void MakeSingleTone()
    {
        lock(_lock)
        {
            // 싱글톤 인스턴스 설정 및 중복 방지
            if (instance == null)
            {
                instance = this as T_Class; // this 객체를 타입 T로 변환하려고 시도하며, 변환이 실패하면 예외를 던지지 않고 null을 반환

                // 최상위 객체에만 의미가 있으니
                if(transform.root == transform)
                    DontDestroyOnLoad(gameObject);
            }
            else if (instance != this)
            {
                Destroy(gameObject); // 중복된 싱글톤 파괴
            }
        }
    }

    

    protected virtual void Awake()
    {
        MakeSingleTone();
    }
}