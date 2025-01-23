using UnityEngine;

public class CallBackAndLambda : MonoBehaviour
{
    void Start()
    {
        // 콜백 함수
        PerformCalculation1(
            10,
            12,
            OnPlayCallback1
            );

        // 콜백 프로시저
        PerformCalculation2(
            10,
            12,
            OnPlayCallback2
            );

        // 콜백 람다식
        PerformCalculation3(
            10,
            12,
            (res1, res2) =>
            {
                return 0.0f;
            }
            );
    }


    // 함수
    void PerformCalculation1(int a, int b, System.Func<int,int, float> callback) // System.Action : 콜백함수 타입
    {
        float c = callback?.Invoke(a, b) ?? 0f; // 매개변수로 넘어온 콜백함수를 호출할 수 있음.
        Debug.Log("콜백함수의 반환값 : " + c);

        // 또는
        if (callback != null)
        {
            c = callback.Invoke(a, b);
        }
        else
        {
            c = 0f;
        }
        Debug.Log("콜백함수의 반환값 : " + c);
    }
    float OnPlayCallback1(int res, int res2)
    {
        Debug.Log("콜백함수호출");
        return 0.5f;
    }


    //프로시저
    void PerformCalculation2(int a, int b, System.Action<int, int> callback) // System.Action : 콜백함수 타입
    {
        callback?.Invoke(a, b); // 매개변수로 넘어온 콜백함수를 호출할 수 있음.
        int result = a + b;
    }
    void OnPlayCallback2(int res, int res2)
    {
        Debug.Log("콜백프로시저호출");
    }

    void PerformCalculation3(int a, int b, System.Func<int, int, float> callback) // System.Action : 콜백함수 타입
    {
        float c = callback?.Invoke(a, b) ?? 0f; // 매개변수로 넘어온 콜백함수를 호출할 수 있음.
        Debug.Log("콜백함수의 반환값 : " + c);

        // 또는
        if (callback != null)
        {
            c = callback.Invoke(a, b);
        }
        else
        {
            c = 0f;
        }
        Debug.Log("콜백함수의 반환값 : " + c);
    }

}
