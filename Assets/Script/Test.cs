using UnityEngine;
using DG.Tweening;

public class Test : MonoBehaviour
{
    public void DiceRotation()
    {
        Sequence sequence = DOTween.Sequence();

        int count = Random.Range(1, 11);
        for(int i = 0; i < count; i++)
        {
            float delay = Random.Range(0.5f, 1.5f);
            // X, Y, Z �� �࿡ ���� 0���� 360���� ������ ȸ������ ����
            float randomX = Random.Range(0f, 360f);
            float randomY = Random.Range(0f, 360f);
            float randomZ = Random.Range(0f, 360f);

            sequence.Append(transform.DORotate(new Vector3(randomX, randomY, randomZ), delay, RotateMode.FastBeyond360));

            // Quaternion�� ����� ������ ȸ������ ����
            //transform.rotation = Quaternion.Euler(randomX, randomY, randomZ);
        }

        sequence.SetLoops(1);
        sequence.Play();
    }
}
