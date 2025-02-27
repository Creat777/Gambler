using DG.Tweening;
using System;
using UnityEngine;

public class DeckOfCards : MonoBehaviour
{
    // 에디터 편집
    public Vector3 cardInterval;

    // 에디터 연결
    public GameObject floor;
    public GameObject[] Players;

    // 스크립트 편집
    private GameObject[] Cards;
    private Vector3 floorPos;
    private int layerOfMe { get; set;}


    private void Awake()
    {
        floorPos = floor.transform.position;
        layerOfMe = LayerMask.NameToLayer("Me");

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
    }

    public void ReturnAllOfCards()
    {
        for (int i = 0; i < Players.Length; i++)
        {
            for (int j = Players[i].transform.childCount-1; j >= 0; j--)
            {
                Transform child = Players[i].transform.GetChild(j);

                // 자색 객체가 나의 레이어를 사용중이면 반환시 레이어도 디폴트로 변경
                if (Players[i].layer == layerOfMe)
                {
                    child.gameObject.layer = 0;
                }

                child.SetParent(this.transform);
            }
        }
    }

    public void CardDistribution(Action endCallback = null)
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
            sequence.AppendCallback(()=> child.position += Vector3.up*5);
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
        sequence.AppendInterval(delay );

        float width = float.MinValue;
        float multiple = 1.2f;
        // 각 플레이어가 받은 카드를 펼치기
        sequence.AppendInterval(1);
        for (int i = 0; i < Players.Length; i ++)
        {
            for (int j = 0; j < Players[i].transform.childCount; j++)
            {
                if(width < -1)
                {
                    Renderer render = Players[i].transform.GetChild(j).gameObject.GetComponent<Renderer>();

                    // 카메라는 무시
                    if(render != null)
                    {
                        width = render.bounds.size.x * multiple;
                    }
                }

                // 플레이어를 기준으로 모든 플레이어가 동시에 5개를 펼침
                sequence.Join(Players[i].transform.GetChild(j).DOLocalMoveX(width*(j-2), delay));
            }
        }

        if(endCallback != null)
        {
            sequence.AppendCallback(()=>endCallback());
        }

        sequence.SetLoops(1);
        sequence.Play();
    }

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
