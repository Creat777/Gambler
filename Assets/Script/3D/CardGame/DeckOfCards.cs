using DG.Tweening;
using PublicSet;
using System;
using System.Collections;
using UnityEngine;

public class DeckOfCards : MonoBehaviour
{
    // 에디터 편집
    public CardGamePlayManager cardGamePlayManager;
    public Vector3 cardInterval;

    // 에디터 연결
    public GameObject floor;
    public GameObject[] Players;

    // 스크립트 편집
    public GameObject[] Cards {  get; private set; }
    private Vector3 floorPos;
    
    private int PlayerCount;


    private void Awake()
    {
        floorPos = floor.transform.position;
    }

    private void Start()
    {
        if (cardGamePlayManager == null)
            Debug.LogAssertion($"cardGamePlayManager == null ");

        // 카드 초기화
        InitCards();

        // 처음에 위치 설정
        SetCardPositions();
    }

    private void InitCards()
    {
        Cards = new GameObject[transform.childCount];

        Vector3 cardSize = Vector3.zero;
        for (int i = 0; i < transform.childCount; i++)
        {
            Cards[i] = transform.GetChild(i).gameObject;
        }
        ProcessCsvOfCard(Cards);
    }

    private void ProcessCsvOfCard(GameObject[] Cards)
    {
        Debug.Log("ProcessCsvOfCard 실행 시작");

        //string path = "CSV/TrumpCardInfo/CardsInfo";

        string path = System.IO.Path.Combine("CSV","TrumpCardInfo","CardsInfo");

        if (Application.platform == RuntimePlatform.Android)
        {
            path = System.IO.Path.Combine(Application.streamingAssetsPath, path);
        }

        CsvManager.Instance.LoadCsv<cTrumpCardInfo>(path,
            (row, trumpCardInfo) => // trumpCard는 컴포넌트로 쓰일 스크립트
            {
                // 저장공간을 할당받지 못한 경우
                if (trumpCardInfo == null)
                {
                    Debug.LogAssertion("trumpCardInfo == null");
                    return;
                }

                    int field_num = 0;


                // 각 행의 처리 시작
                foreach (string field in row)
                {
                    switch (field_num)
                    {
                        case 0: 
                            if (int.TryParse(field, out int intField))
                            {
                                trumpCardInfo.cardIndex = intField; break;
                            }
                            else
                            {
                                Debug.LogAssertion($"타입오류 -> {field}");
                            }
                            break;

                        case 1: trumpCardInfo.cardName = field;
                            break;

                        case 2:
                            if (eCardType.TryParse(field, out eCardType eCardTypeField))
                            {
                                trumpCardInfo.cardType = eCardTypeField; break;
                            }
                            else
                            {
                                Debug.LogAssertion($"타입오류 -> {field}");
                            }
                            break;

                        case 3:
                            if (int.TryParse(field, out int intField2))
                            {
                                trumpCardInfo.cardValue = intField2; break;
                            }
                            else
                            {
                                Debug.LogAssertion($"타입오류 -> {field}");
                            }
                            break;

                        default: Debug.LogAssertion($"잘못된 데이터 : {field}");
                            break;
                    }
                    field_num++;
                }

                // 처리된 데이터를 각 카드에 삽입
                if(trumpCardInfo.cardIndex>=0 && trumpCardInfo.cardIndex < Cards.Length)
                {
                    trumpCardInfo.isFaceDown = true; // 모든 카드는 처음에 뒤집어져있음
                    Cards[trumpCardInfo.cardIndex].GetComponent<TrumpCardDefault>().SetTrumpCard(trumpCardInfo);
                }
                
            }
            );
    }

    public void SetPlayerNumber()
    {
        PlayerCount = Players.Length;
    }

    public void SetPlayerNumber(int vlaue)
    {
        PlayerCount = vlaue;
    }

    public void ReturnAllOfCards()
    {
        for (int i = 0; i < Cards.Length; i++)
        {
            // 모든 카드를 활성화
            Cards[i].SetActive(true);

            // 레이어를 디폴트로 바꿔줌
            if (Cards[i].layer != 0)
            {
                Cards[i].layer = 0;
            }
            // 카드를 회수하고 스케일 정정
            Cards[i].transform.SetParent(transform, false);
            Cards[i].transform.localScale = Vector3.one;
        }

    }

    private void ReturnAllOfCardFromCardBox(Transform cardBox)
    {
        // 자식객체에 있는 카드박스들
        for (int j = 0; j < cardBox.childCount; j++)
        {
            Transform child = cardBox.GetChild(j);

            // 레이어가 디폴트값이 아니면 디폴트로 변경
            if (cardBox.gameObject.layer != 0)
            {
                child.gameObject.layer = 0;
            }

            child.SetParent(this.transform);
        }
    }

    public void CardDistribution(GameObject player, int diceValue)
    {
        if(cardGamePlayManager == null)
        {
            Debug.LogAssertion("cardGamePlayManager == null");
            return;
        }

        float delay = 0.5f;
        DG.Tweening.Sequence sequence = DG.Tweening.DOTween.Sequence();

        Debug.Log($"{player.name}의 카드 분배 시작");
        CardGamePlayerBase playerScript = player.GetComponent<CardGamePlayerBase>();

        if(playerScript == null)
        {
            Debug.LogAssertion("playerScript == null");
            return;
        }

        // 한장씩 꺼내어 플레이어한테 분배
        for (int i = 0; i < diceValue; i++)
        {
            // 항상 0번인덱스의 자식객체(카드)는 존재함
            Transform child = transform.GetChild(0);

            // 카드를 플레이어의 자식객체로 설정, 플레이어는 받은 카드를 정리
            playerScript.AddCard(child);

            // 스케일 초기화
            child.localScale = Vector3.one * transform.localScale.x;

            // 만약 배분받은 플레이어가 나 자신이라면 자식객체도 같은 레이어를 사용 -> sub카메라의 culling mask에 사용
            if (player.layer == cardGamePlayManager.layerOfMe)
            {
                Debug.Log($"레이어 설정 : {player.layer}");
                child.gameObject.layer = cardGamePlayManager.layerOfMe;

                // 조커에 붙은 이미지도 레이어 변경 해줘야 서브카메라에 이미지를 담을 수 있음
                if(child.transform.childCount != 0)
                {
                    child.transform.GetChild(0).gameObject.layer = cardGamePlayManager.layerOfMe;
                }
            }

            // 카드를 플레이어한테 분배 Animaition
            sequence.AppendCallback(()=> child.position += Vector3.up*5);
            sequence.Append(
                child.DOMove(child.parent.position + Vector3.up * (i+1), delay)
                );

            // 플레이어 방향에 맞게 카드를 회전 Animaition
            Vector3 targetRotation = child.parent.rotation.eulerAngles;
            sequence.Join(
                child.DORotate(targetRotation, delay, RotateMode.WorldAxisAdd)
                );
        }

        // 카드배분 후 잠시 기다림
        sequence.AppendInterval(delay * 2);

        if(playerScript!= null)
        {
            // 정리된 카드를 펼침
            playerScript.GetSequnece_CardSpread(sequence, delay);
        }
        else
        {
            Debug.LogAssertion("playerScript == null");
            return;
        }


        // 플레이어(Me)의 카드 배분이 끝났으면 자기 카드를 확인 할 수 있도록 만듬
        if (player.layer == cardGamePlayManager.layerOfMe)
        {
            //Debug.Log("내 카드 보기 활성화!");
            sequence.AppendCallback(()=> 
            {
                cardGamePlayManager.cardGameView.cardScreenButton.TryActivate_Button();
                cardGamePlayManager.CardButtonMemoryPool.InitCardButton(playerScript.closeBox.transform);
            });
        }

        // 모든 처리가 끝났으면 카드게임 매니저가 게임세팅을 이어서 하도록 요청
        sequence.AppendCallback(()=>cardGamePlayManager.GameSetting());

        sequence.SetLoops(1);
        sequence.Play();
    }


    public void StartDisappearEffect()
    {
        float animationDuration = 0.5f;
        float parentScale = transform.localScale.x;

        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject obj = transform.GetChild(i).gameObject;

            // 중력 끄기
            Rigidbody rigid = obj.GetComponent<Rigidbody>();

            rigid.useGravity = false;

            // 이동
            obj.transform.DOLocalMoveY(5f / parentScale, animationDuration);
            obj.transform.DOLocalMoveZ(10f / parentScale, animationDuration).SetDelay(animationDuration);

            // 화면에서 사라진 뒤 비활성화
            StartCoroutine(CallbackSetDealy(
                () => 
                {
                    rigid.useGravity = true; // 중력 활성화
                    obj.SetActive(false);
                }
                , animationDuration *2)
                );
        }
    }

    IEnumerator CallbackSetDealy(Action callback,float delay)
    {
        yield return new WaitForSeconds(delay);
        callback();
    }

    // 카드의 위치를 설정하는 함수
    public void SetCardPositions()
    {
        ReturnAllOfCards();
        for (int i = 0; i < transform.childCount; i++)
        {
            // z축으로 카드 뒤집기
            transform.GetChild(i).transform.rotation = Quaternion.Euler(90, 0, 0);

            // 자식객체중 인덱스가 큰 순으로 위쪽에 위치함
            transform.GetChild(i).position = floorPos + cardInterval * (i + 1);
        }
    }

    public void CardShuffle()
    {
        // 자식 객체의 순서를 섞음
        for (int i = 0; i < transform.childCount; i++)
        {
            int newSiblingIndex = UnityEngine.Random.Range(0, transform.childCount);
            transform.GetChild(i).transform.SetSiblingIndex(newSiblingIndex);
        }

        // 카드를 섞은 후, 새 위치를 설정
        SetCardPositions();
    }


}
