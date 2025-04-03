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

        // ī�尡 1���� ���������� ����
        if (CardDefault.isFaceDown)
        {
            CardDefault.isFaceDown = false;

            // �߷��� ��� ����
            sequence.AppendCallback(() => m_Rigidbody.useGravity = false);

            // ī�带 ���� �̵�(�ٴ��̶� �ε��� ����)
            sequence.Append(transform.DOMoveY(transform.position.y + upValue, delay)); returnDelay += delay;

            // ī�� ȸ��
            sequence.Append(transform.DORotate(Vector3.forward * 180f, delay, RotateMode.WorldAxisAdd)); returnDelay += delay;

            // �߷� �ٽ� Ȱ��ȭ
            sequence.AppendCallback(() => m_Rigidbody.useGravity = true);
            //sequence.AppendCallback(() => Debug.Log("�ִϸ��̼� ���������� ���� �Ϸ�"));
        }
        else
        {
            Debug.Log($"{gameObject.name}�� �̹� ���µ� ī���Դϴ�");
        }
        return returnDelay;
    }

    public float GetSequnce_Animation_CardClose(Sequence sequence)
    {
        float delay = 0.5f;
        float returnDelay = 0;

        // ī�尡 1���� ���������� ����
        if (CardDefault.isFaceDown == false)
        {
            CardDefault.isFaceDown = true;

            // �߷��� ��� ����
            sequence.AppendCallback(() => m_Rigidbody.useGravity = false);

            // ī�带 ���� �̵�(�ٴ��̶� �ε��� ����)
            sequence.Append(transform.DOMoveY(transform.position.y + upValue, delay)); returnDelay += delay;

            // ī�� ȸ��
            sequence.Append(transform.DORotate(Vector3.forward * -180f, delay, RotateMode.WorldAxisAdd)); returnDelay += delay;

            // �߷� �ٽ� Ȱ��ȭ
            sequence.AppendCallback(() => m_Rigidbody.useGravity = true);
            //sequence.AppendCallback(() => Debug.Log("�ִϸ��̼� ���������� ���� �Ϸ�"));
        }
        else
        {
            Debug.Log($"{gameObject.name}�� �̹� ������ ī���Դϴ�");
        }
        return returnDelay;
    }
}
