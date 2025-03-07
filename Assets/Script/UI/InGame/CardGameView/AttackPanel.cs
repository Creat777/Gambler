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
                Debug.LogAssertion("���ݴ�� ���� �ȵ���");
                break;
            }
        }

        foreach(var att in computerSprites)
        {
            if (computerSprites == null)
            {
                Debug.LogAssertion("��ǻ�� �̹��� ���� �ȵ���");
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
        // �ߺ��Ǵ� �̹����� �����ϱ����� �ؽ���
        HashSet<int> indexHash = new HashSet<int>();

        for (int i = 0; i < AttackOjects.Length; i++)
        {
            // �÷��̾� �̹����� ����
            if (AttackOjects[i].CompareTag("Player"))
            {
                continue;
            }

            // �̹����� ���� �� �� ���� �ݺ�
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
