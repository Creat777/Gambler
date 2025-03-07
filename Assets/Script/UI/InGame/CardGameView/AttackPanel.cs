using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackPanel : MonoBehaviour
{
    public SelectAttackObject[] AttackOjects;
    public Sprite[] computerSprites;

    private void Awake()
    {
        checkAttribute();
    }

    private void checkAttribute()
    {
        foreach (var att in AttackOjects)
        {
            if(att == null)
            {
                Debug.LogAssertion("공격대상 연결 안됐음");
                break;
            }
        }

        foreach(var att in computerSprites)
        {
            if (computerSprites == null)
            {
                Debug.LogAssertion("컴퓨터 이미지 연결 안됐음");
                break;
            }
        }
    }

    public void InitPlayers()
    {
        RandomImageToComputer();
    }

    private void RandomImageToComputer()
    {
        // 중복되는 이미지를 방지하기위한 해쉬셋
        HashSet<int> indexHash = new HashSet<int>();

        for (int i = 0; i < AttackOjects.Length; i++)
        {
            // 플레이어 이미지는 제외
            if (AttackOjects[i].CompareTag("Player"))
            {
                continue;
            }

            // 이미지가 설정 될 때 까지 반복
            while(true)
            {
                int index = Random.Range(0, computerSprites.Length);

                if (indexHash.Contains(index))
                {
                    continue;
                }
                else
                {
                    indexHash.Add(index);
                    AttackOjects[i].image.sprite = computerSprites[index];
                    break;
                }
            }
        }
    }
}
