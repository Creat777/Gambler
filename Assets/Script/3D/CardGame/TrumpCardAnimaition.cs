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
        //m_Renderer = GetComponent<Renderer>();


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

    //public void Animation_CardOrganize(float delay, GameObject cardGamePlayer)
    //{
        

    //    CardGamePlayerBase playerScript = cardGamePlayer.GetComponent<CardGamePlayerBase>();
    //    if(playerScript == null)
    //    {
    //        Debug.LogAssertion("gamePlayer == null");
    //        return;
    //    }


    //    float width = float.MinValue;
    //    float multiple = 1.1f;

    //    Renderer render = cardGamePlayer.transform.GetChild(0).gameObject.GetComponent<Renderer>();
    //    if (render != null)
    //    {
    //        width = render.bounds.size.x * multiple;
    //    }
    //    else
    //    {
    //        Debug.LogAssertion("render == null");
    //    }

    //    int openCardLayer = 8;
    //    int closCardLayer = 9;
    //    CardSpread(delay, playerScript.openCardCount, width, playerScript.gameObject, openCardLayer);
    //    CardSpread(delay, playerScript.closeCardCount, width, playerScript.gameObject, closCardLayer);
    //}

    //private void CardSpread(float delay, int count, float width, GameObject cardGamePlayer, int layer)
    //{
    //    Sequence sequence = DOTween.Sequence();
        
    //    sequence.AppendInterval(delay);
    //    for (int i = 0; i < cardGamePlayer.transform.childCount; i++)
    //    {
    //        // �÷��̾ �������� ��� ī�带 ��ħ
    //        // ¦���� ��� ī�� ���̰� �߾����� ��ġ
    //        // Ȧ���� ��� ī�尡 �߾����� ��ġ

    //        if(cardGamePlayer.transform.GetChild(i).gameObject.layer == layer)
    //        {
    //            float offset = (count % 2 == 0) ? (i - count / 2 + 0.5f) : (i - count / 2);
    //            sequence.Join(cardGamePlayer.transform.GetChild(i).DOLocalMoveX(width * offset, delay));
    //        }
    //    }

    //    sequence.SetLoops(1);
    //    sequence.Play();
    //}
}
