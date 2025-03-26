using UnityEngine;
using DG.Tweening;
public class PlayerInterface_CardGame : MonoBehaviour
{
    //������
    public CardScreenButton cardScreenButton;
    public DiceButton diceButton;
    public GameObject SubScreen_Dice;

    //��ũ��Ʈ
    private Vector3 InScreenPos;
    private Vector3 OutOfScreenPos;
    float delay = 1.0f;

    private void Awake()
    {
        InScreenPos = transform.position;
        OutOfScreenPos = transform.position + Vector3.right * 300;
    }

    private void Start()
    {
        if(cardScreenButton == null)
        {
            Debug.LogAssertion("cardScreenButton == null");
        }

        if (diceButton == null)
        {
            Debug.LogAssertion("diceButton == null");
        }

        if (SubScreen_Dice == null)
        {
            Debug.LogAssertion("SubScreen_Dice == null");
        }
    }

    private void OnEnable()
    {
        
    }

    private void DiceSetAcive(bool value)
    {
        diceButton.gameObject.SetActive(value);
        SubScreen_Dice.SetActive(value);
    }

    public void InitInterface()
    {
        // Ȱ��ȭ�Ǹ� �ۿ��� ���
        transform.position = OutOfScreenPos;

        cardScreenButton.TryDeactivate_Button();
        diceButton.TryDeactivate_Button();

        // content�� Ȱ��ȭ�� ��ü �ʱ�ȭ
        DiceSetAcive(true);
        cardScreenButton.gameObject.SetActive(false);
    }

    public void returnInterface()
    {
        diceButton.TryDeactivate_Button();

        Sequence sequence = DOTween.Sequence();
        GetSequnce_InterfaceOff(sequence);
        sequence.AppendCallback(()=>DiceSetAcive(true));
        sequence.AppendCallback(()=>cardScreenButton.gameObject.SetActive(false));

        sequence.SetLoops(1);
        sequence.Play();
    }

    public void GetSequnce_ChangeInterfaceNext(Sequence sequence)
    {
        // �������̽��� �����
        GetSequnce_InterfaceOff(sequence);

        // ���빰�� ������ ����
        sequence.AppendCallback(() => DiceSetAcive(false));
        sequence.AppendCallback(() => cardScreenButton.gameObject.SetActive(true));
        sequence.AppendCallback(() => cardScreenButton.TryActivate_Button());

        // �ٽ� ����
        GetSequnce_InterfaceOn(sequence);
    }

    public void GetSequnce_InterfaceOn(Sequence sequence)
    {
        sequence.Append(transform.DOMove(InScreenPos, delay));
    }
    public void PlaySequnce_InterfaceOn()
    {
        Sequence sequence = DOTween.Sequence();
        GetSequnce_InterfaceOn(sequence);
        sequence.SetLoops(1);
        sequence.Play();
    }
    public void GetSequnce_InterfaceOff(Sequence sequence)
    {
        sequence.Append(transform.DOMove(OutOfScreenPos, delay));
    }
    public void PlaySequnce_InterfaceOff()
    {
        Sequence sequence = DOTween.Sequence();
        GetSequnce_InterfaceOff(sequence);
        sequence.SetLoops(1);
        sequence.Play();
    }
}
