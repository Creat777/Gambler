using UnityEngine;
using DG.Tweening;

public class TrumpCardAnimaition : MonoBehaviour
{
    private TrumpCardDefault _CardDefault;
    public TrumpCardDefault CardDefault
    {
        get
        {
            if (_CardDefault == null) _CardDefault = GetComponent<TrumpCardDefault>();
            return _CardDefault;
        }
    }

    private BoxCollider _m_BoxCollider;
    public BoxCollider m_BoxCollider
    {
        get 
        {
            if(_m_BoxCollider == null) _m_BoxCollider = GetComponent<BoxCollider>();
            return _m_BoxCollider; 
        }
    }

    private Rigidbody _m_Rigidbody;
    public Rigidbody m_Rigidbody
    {
        get
        {
            if (_m_Rigidbody == null) _m_Rigidbody = GetComponent<Rigidbody>();
            return _m_Rigidbody;
        }
    }

    //private Renderer m_Renderer;
    public float upValue;

    private void Awake()
    {
        upValue = 3f;
    }



    public float GetSequnce_Animation_CardOpen(Sequence sequence)
    {
        float delay = 0.5f;
        float returnDelay = 0;

        // 카드가 1번만 뒤집히도록 만듬
        if (CardDefault.isFaceDown)
        {
            CardDefault.isFaceDown = false;

            // 중력을 잠시 끄고
            sequence.AppendCallback(() => m_Rigidbody.useGravity = false);

            // 카드를 위로 이동(바닥이랑 부딛힘 방지)
            sequence.Append(transform.DOMoveY(transform.position.y + upValue, delay)); returnDelay += delay;

            // 카드 회전
            sequence.Append(transform.DORotate(Vector3.forward * 180f, delay, RotateMode.WorldAxisAdd)); returnDelay += delay;

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

    public float GetSequnce_Animation_CardClose(Sequence sequence)
    {
        float delay = 0.5f;
        float returnDelay = 0;

        // 카드가 1번만 뒤집히도록 만듬
        if (CardDefault.isFaceDown == false)
        {
            CardDefault.isFaceDown = true;

            // 중력을 잠시 끄고
            sequence.AppendCallback(() => m_Rigidbody.useGravity = false);

            // 카드를 위로 이동(바닥이랑 부딛힘 방지)
            sequence.Append(transform.DOMoveY(transform.position.y + upValue, delay)); returnDelay += delay;

            // 카드 회전
            sequence.Append(transform.DORotate(Vector3.forward * -180f, delay, RotateMode.WorldAxisAdd)); returnDelay += delay;

            // 중력 다시 활성화
            sequence.AppendCallback(() => m_Rigidbody.useGravity = true);
            //sequence.AppendCallback(() => Debug.Log("애니메이션 정상적으로 수행 완료"));
        }
        else
        {
            Debug.Log($"{gameObject.name}은 이미 정리된 카드입니다");
        }
        return returnDelay;
    }
}
