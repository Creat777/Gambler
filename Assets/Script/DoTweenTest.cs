using UnityEngine;
using DG.Tweening;

public class DoTweenTest : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.AppendCallback(() => Debug.LogAssertion("½Ã¹ß"));
    }
}
