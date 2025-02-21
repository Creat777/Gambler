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
            // X, Y, Z 각 축에 대해 0부터 360까지 랜덤한 회전값을 생성
            float randomX = Random.Range(0f, 360f);
            float randomY = Random.Range(0f, 360f);
            float randomZ = Random.Range(0f, 360f);

            sequence.Append(transform.DORotate(new Vector3(randomX, randomY, randomZ), delay, RotateMode.FastBeyond360));

            // Quaternion을 사용해 랜덤한 회전값을 적용
            //transform.rotation = Quaternion.Euler(randomX, randomY, randomZ);
        }

        sequence.SetLoops(1);
        sequence.Play();
    }
}
