using UnityEngine;
using DG.Tweening;
public class PlayerInterface_CardGame : MonoBehaviour
{
    //에디터
    public CardScreenButton cardScreenButton;
    public DiceButton diceButton;
    public GameObject SubScreen_Dice;

    //스크립트
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
        // 활성화되면 밖에서 대기
        transform.position = OutOfScreenPos;

        cardScreenButton.TryDeactivate_Button();
        diceButton.TryDeactivate_Button();

        // content에 활성화할 객체 초기화
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
        // 인터페이스를 숨기고
        GetSequnce_InterfaceOff(sequence);

        // 내용물을 변경한 다음
        sequence.AppendCallback(() => DiceSetAcive(false));
        sequence.AppendCallback(() => cardScreenButton.gameObject.SetActive(true));
        sequence.AppendCallback(() => cardScreenButton.TryActivate_Button());

        // 다시 등장
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
