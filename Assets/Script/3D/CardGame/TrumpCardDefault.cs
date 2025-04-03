using DG.Tweening;
using PublicSet;
using UnityEditor.Build;
using UnityEngine;

public class TrumpCardDefault : MonoBehaviour
{
    public TrumpCardAnimaition animationScript {  get; private set; }
    public cTrumpCardInfo trumpCardInfo {  get; private set; }

    [SerializeField] bool _isSelected;
    public bool isSelected { get { return _isSelected; } set { _isSelected = value; } }
    [SerializeField] bool _isFaceDown;
    public bool isFaceDown { get { return _isFaceDown; } set { _isFaceDown = value; } }

    private void Awake()
    {
        animationScript = GetComponent<TrumpCardAnimaition>();
    }

    public void SetTrumpCard(cTrumpCardInfo value)
    {
        //Debug.Log($"SetTrumpCard실행, {gameObject.name}(객체이름) : {value.cardName}(설정된 카드 이름)");
        trumpCardInfo = value;

        // 제대로 데이터가 삽입되었는지 확인
        //CsvManager.Instance.PrintProperties(trumpCardInfo);
    }

    public void InitAttribute()
    {
        isSelected = false;
        isFaceDown = true;
    }

    public bool GetSequnce_TryCardOpen(Sequence sequence, CardGamePlayerBase playerScript)
    {
        if(isSelected)
        {
            sequence.AppendCallback(()=>
                    { if(gameObject.layer != 0) gameObject.layer = 0; }
            ); // 서브스크린에서 지우기 위함

            playerScript.SetParent_OpenBox(gameObject);
            animationScript.GetSequnce_Animation_CardOpen(sequence);

            return true;
        }
        else
        {
            //Debug.Log($"{gameObject.name}은 선택되지 않았음");
            return false;
        }
    }

    public bool GetSequnce_TryCardClose(Sequence sequence, CardGamePlayerBase playerScript)
    {
        if (isSelected == false)
        {
            // 레이어 재설정
            if(playerScript.tag == "Player") gameObject.layer = CardGamePlayManager.Instance.layerOfMe;
            //else gameObject.layer = 0;
            
            playerScript.SetParent_CloseBox(gameObject);
            animationScript.GetSequnce_Animation_CardClose(sequence);

            return true;
        }
        else
        {
            //Debug.Log($"{gameObject.name}은 선택되지 않았음");
            return false;
        }
    }

    private void CantSelectThisCard()
    {
        // 화면에 메세지를 띄워야함
        Debug.Log("현재 카드는 선택될 수 없음");
    }

    public bool TrySelectThisCard_OnGameSetting(CardGamePlayerBase player)
    {
        if(player == null)
        {
            Debug.Log($"{player.gameObject.name}의 {player.name} == null");
            return false;
        }
        if(trumpCardInfo == null)
        {
            Debug.LogAssertion($"{gameObject.name}의 trumpCardInfo == null");
            return false;
        }

        if (player.TryDownCountPerCardType(trumpCardInfo))
        {
            Debug.Log($"선택된 카드 : {trumpCardInfo.cardName}");
            isSelected = true;
            return true;
        }
        else
        {
            CantSelectThisCard();
            return false;
        }

    }
    public bool TrySelectThisCard_OnPlayTime(CardGamePlayerBase player)
    {
        // 동일한 디버깅은 생략함

        

        // 플레이어의 경우 선택이 제한됨
        if (player.tag == "Player")
        {
            PlayerMe playerMe = (player as PlayerMe);
            if (playerMe.isCompleteSelect_OnPlayTime == false)
            {
                isSelected = true;

                playerMe.TyrSetPresentedCard(this);

                playerMe.Set_isCompleteSelect_OnPlayTime(isSelected);

                Debug.Log($"선택된 카드 : {trumpCardInfo.cardName}");
                return isSelected;
            }
            else
            {
                Debug.Log($"{player.gameObject.name}은 이미 카드를 선택했음");
                Debug.Log("해당 내용을 게임에서 알려줄 필요가 있음");
                return isSelected;
            }
        }

        // 컴퓨터의 경우 한번만 선택될테니 바로 적용
        else
        {
            isSelected = true;
            return true;
        }
        
    }

    public void UnselectThisCard_OnStartTime(CardGamePlayerBase player)
    {
        player.UpCountPerCardType(trumpCardInfo);
        Debug.Log($"선택 취소된 카드 : {trumpCardInfo.cardName}");
        isSelected = false;
    }

    public void UnselectThisCard_OnPlayTime(CardGamePlayerBase player)
    {
        if(player.tag == "Player")
        {
            PlayerMe playerMe = (player as PlayerMe);

            isSelected = false;
            playerMe.Set_isCompleteSelect_OnPlayTime(isSelected);
            Debug.Log($"선택 취소된 카드 : {trumpCardInfo.cardName}");
        }

        else
        {
            isSelected = false;
        }

    }
}
