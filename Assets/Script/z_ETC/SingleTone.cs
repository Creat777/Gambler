using Unity.VisualScripting;
using UnityEngine;

public abstract class Singleton<T_Class> : MonoBehaviour where T_Class : MonoBehaviour
{
    // ���μ��� ���� ���� �ϳ��� �����ϴ� ��ü == �̱���  : 0���� �ƴϰ� 2�� �̻� �ƴϰ� ���� 1��
    // ��? �����?
    //  -> 1�� �޸� ����ȭ ����
    //  -> 2�� ���ν��� ó�� �ߺ� ����
    //  -> 3�� Ŭ�����̸����� ���ν����� ������ �� ����

    // static ����  : Ŭ������ ��������� ��ü�� ������ ���� �Ѱ��� �����ϴ� ����
    //              : Ŭ���� �̸����� ������ �� ����
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
                    Debug.LogWarning($"{typeof(T_Class).Name}�̱����� ���� �������� �ʾҽ��ϴ�.");
                    return null;
                }
                // null�� �ƴϸ� �״�� ����
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
            // �̱��� �ν��Ͻ� ���� �� �ߺ� ����
            if (instance == null)
            {
                instance = this as T_Class; // this ��ü�� Ÿ�� T�� ��ȯ�Ϸ��� �õ��ϸ�, ��ȯ�� �����ϸ� ���ܸ� ������ �ʰ� null�� ��ȯ

                // �ֻ��� ��ü���� �ǹ̰� ������
                if(transform.root == transform)
                    DontDestroyOnLoad(gameObject);
            }
            else if (instance != this)
            {
                Destroy(gameObject); // �ߺ��� �̱��� �ı�
            }
        }
    }

    

    protected virtual void Awake()
    {
        MakeSingleTone();
    }
}