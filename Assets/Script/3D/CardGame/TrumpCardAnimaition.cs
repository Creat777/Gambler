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

        // ī�尡 1���� ���������� ����
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

            // �߷��� ��� ����
            sequence.AppendCallback(() => m_Rigidbody.useGravity = false);

            // ī�带 ���� �̵�(�ٴ��̶� �ε��� ����)
            sequence.Append(transform.DOMoveY(transform.position.y + upValue, delay)); returnDelay += delay;

            // ī�� ȸ��
            sequence.Append(transform.DORotate(Vector3.forward * 180, delay, RotateMode.WorldAxisAdd)); returnDelay += delay;

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
}
