using UnityEngine;
using DG.Tweening;

public class TrumpCardAnimaition : MonoBehaviour
{
    private BoxCollider m_BoxCollider;
    private Rigidbody m_Rigidbody;
    //private Renderer m_Renderer;
    public float upValue;
    bool m_isCardOpened = false;

    private void Awake()
    {
        m_BoxCollider = GetComponent<BoxCollider>();
        m_Rigidbody = GetComponent<Rigidbody>();

        upValue = 3f;
    }



    public float GetSequnce_Animation_CardOpen(Sequence sequence)
    {
        float delay = 0.5f;
        float returnDelay = 0;

        // 카드가 1번만 뒤집히도록 만듬
        if (m_isCardOpened == false)
        {
            m_isCardOpened = true;
            if (m_BoxCollider == null || m_Rigidbody == null)
            {
                if (m_BoxCollider != null)
                    Debug.LogAssertion("m_BoxCollider == null");

                if (m_Rigidbody == null)
                    Debug.LogAssertion("m_Rigidbody == null");

                return 0f;
            }

            // 중력을 잠시 끄고
            sequence.AppendCallback(() => m_Rigidbody.useGravity = false);

            // 카드를 위로 이동(바닥이랑 부딛힘 방지)
            sequence.Append(transform.DOMoveY(transform.position.y + upValue, delay)); returnDelay += delay;

            // 카드 회전
            sequence.Append(transform.DORotate(Vector3.forward * 180, delay, RotateMode.WorldAxisAdd)); returnDelay += delay;

            // 중력 다시 활성화
            sequence.AppendCallback(() => m_Rigidbody.useGravity = true);
            //sequence.AppendCallback(() => Debug.Log("애니메이션 정상적으로 수행 완료"));
        }
        else
        {
            Debug.Log($"{gameObject.name}은 이미 오픈된 카드입니다");
        }
        return returnDelay;
    }
}
