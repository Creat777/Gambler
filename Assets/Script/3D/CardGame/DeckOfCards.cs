using DG.Tweening;
using PublicSet;
using System;
using Unity.VisualScripting;
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
    private GameObject[] Cards;
    private Vector3 floorPos;
    
    private int PlayerCount;


    private void Awake()
    {
        floorPos = floor.transform.position;

        

        // 처음에 위치 설정
        SetCardPositions();
    }

    private void Start()
    {
        if (cardGamePlayManager == null)
            Debug.LogAssertion($"cardGamePlayManager == null ");

        // 카드 초기화
        InitCards();
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
        string fileNamePath = "CSV/Card/Card";
        CsvManager.Instance.LoadCsv<cTrumpCardInfo>(fileNamePath,
            (row, trumpCardInfo) => // trumpCard는 컴포넌트로 쓰일 스크립트
            {
                // 저장공간을 할당받지 못한 경우
                if (trumpCardInfo == null) return;

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
                        default: break;
                    }
                    field_num++;
                }

                // 처리된 데이터를 각 카드에 삽입
                if(trumpCardInfo.cardIndex>=0 && trumpCardInfo.cardIndex < Cards.Length)
                {
                    Cards[trumpCardInfo.cardIndex].AddComponent<TrumpCard>().SetTrumpCard(trumpCardInfo);
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
        for (int i = 0; i < Players.Length; i++)
        {
            for (int j = Players[i].transform.childCount-1; j >= 0; j--)
            {
                Transform child = Players[i].transform.GetChild(j);

                // 자색 객체가 나의 레이어를 사용중이면 반환시 레이어도 디폴트로 변경
                if (Players[i].layer == cardGamePlayManager.layerOfMe)
                {
                    child.gameObject.layer = 0;
                }

                child.SetParent(this.transform);
            }
        }
    }

    public void CardDistribution(GameObject player, int diceValue)
    {
        float delay = 0.5f;
        DG.Tweening.Sequence sequence = DG.Tweening.DOTween.Sequence();

        Debug.Log($"{player.name}의 카드 분배 시작");

        // 한장씩 꺼내어 플레이어한테 분배
        for (int i = 0; i < diceValue; i++)
        {
            // 항상 0번인덱스의 자식객체(카드)는 존재함
            Transform child = transform.GetChild(0);

            // 카드를 플레이어의 자식객체로 설정
            child.SetParent(player.transform);

            // 스케일 초기화
            child.localScale = Vector3.one * transform.localScale.x;

            // 만약 배분받은 플레이어가 나 자신이라면 자식객체도 같은 레이어를 사용 -> sub카메라의 culling mask에 사용
            if (player.layer == cardGamePlayManager.layerOfMe)
            {
                Debug.Log($"레이어 설정 : {player.layer}");
                child.gameObject.layer = cardGamePlayManager.layerOfMe;
            }

            // 카드를 플레이어한테 분배
            sequence.AppendCallback(()=> child.position += Vector3.up*5);
            sequence.Append(
                child.DOMove(child.parent.position + Vector3.up * (i+1), delay)
                );

            // 플레이어 방향에 맞게 카드를 회전
            Vector3 targetRotation = child.parent.rotation.eulerAngles;
            sequence.Join(
                child.DORotate(targetRotation, delay, RotateMode.WorldAxisAdd)
                );
        }

        // 카드배분 후 잠시 기다림
        sequence.AppendInterval(delay * 2 );

        // 플레이어가 받은 카드를 펼치기
        sequence.AppendInterval(delay); // join과 같은 타임
        float width = float.MinValue;
        float multiple = 1.1f;
        for (int i = 0; i < player.transform.childCount; i++)
        {
            if (width < -1)
            {
                Renderer render = player.transform.GetChild(i).gameObject.GetComponent<Renderer>();

                // 카메라는 무시
                if (render != null)
                {
                    width = render.bounds.size.x * multiple;
                }
            }


            // 플레이어를 기준으로 모든 카드를 펼침
            // 짝수의 경우 카드 사이가 중앙으로 위치
            // 홀수의 경우 카드가 중앙으로 위치
            float offset = (diceValue % 2 == 0) ? (i - diceValue / 2 + 0.5f) : (i - diceValue / 2);
            sequence.Join(player.transform.GetChild(i).DOLocalMoveX(width * offset, delay));
        }

        // 플레이어(Me)의 카드 배분이 끝났으면 자기 카드를 확인 할 수 있도록 만듬
        if(player.layer == cardGamePlayManager.layerOfMe)
        {
            //Debug.Log("내 카드 보기 활성화!");
            sequence.AppendCallback(()=> cardGamePlayManager.cardGameView.cardScreenButton.Activate_Button());
        }

        // 모든 처리가 끝났으면 카드게임 매니저가 다음 순서의 플레이어를 실행하도록 요청
        sequence.AppendCallback(cardGamePlayManager.NPC_Dice);

        sequence.SetLoops(1);
        sequence.Play();
    }

    /*
    public void CardDistribution(GameObject player, int diceValue)
    {
        float delay = 0.5f;
        DG.Tweening.Sequence sequence = DG.Tweening.DOTween.Sequence();

        int count = 20;

        // 한장씩 꺼내어 플레이어한테 분배
        for (int i = 0; i < count; i++)
        {
            // 플레이어 배분 순서
            int PlayerOrder = i % 4;

            // 플레이어마다 갖는 카드의 배분 순서
            int PlayerCardNumber = i / 4;

            Transform child = transform.GetChild(0);

            // 카드를 플레이어의 자식객체로 설정
            child.SetParent(Players[PlayerOrder].transform);

            // 스케일 초기화
            child.localScale = Vector3.one * transform.localScale.x;

            // 만약 배분받은 플레이어가 나 자신이라면 자식객체도 같은 레이어를 사용
            if (Players[PlayerOrder].layer == layerOfMe)
            {
                Debug.Log($"레이어 설정 : {Players[PlayerOrder].layer}");
                child.gameObject.layer = layerOfMe;
            }

            // 각 카드를 플레이어한테 분배
            sequence.AppendCallback(() => child.position += Vector3.up * 5);
            sequence.Append(
                child.DOMove(child.parent.position + Vector3.up * PlayerCardNumber, delay)
                );

            // 플레이어 방향에 맞게 카드를 회전
            Vector3 targetRotation = child.parent.rotation.eulerAngles;
            sequence.Join(
                child.DORotate(targetRotation, delay, RotateMode.WorldAxisAdd)
                );
        }

        // 카드배분 후 잠시 기다림
        sequence.AppendInterval(delay);

        float width = float.MinValue;
        float multiple = 1.2f;
        // 각 플레이어가 받은 카드를 펼치기
        sequence.AppendInterval(1);
        for (int i = 0; i < Players.Length; i++)
        {
            for (int i = 0; i < Players[i].transform.childCount; i++)
            {
                if (width < -1)
                {
                    Renderer render = Players[i].transform.GetChild(i).gameObject.GetComponent<Renderer>();

                    // 카메라는 무시
                    if (render != null)
                    {
                        width = render.bounds.size.x * multiple;
                    }
                }

                // 플레이어를 기준으로 모든 플레이어가 동시에 5개를 펼침
                sequence.Join(Players[i].transform.GetChild(i).DOLocalMoveX(width * (i - 2), delay));
            }
        }

        if (endCallback != null)
        {
            sequence.AppendCallback(() => endCallback());
        }

        sequence.SetLoops(1);
        sequence.Play();
    }
    */

    // 카드의 위치를 설정하는 함수
    private void SetCardPositions()
    {
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
