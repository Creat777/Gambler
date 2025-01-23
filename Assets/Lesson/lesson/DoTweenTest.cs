using UnityEngine;
using DG.Tweening;

public class DoTweenTest : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, -490), 3f);
        //GetComponent<RectTransform>().DOScale(3.0f, 7.0f); // 스케일링
    }

    private void OnEnable()
    {
        // 화면 밖으로 이동
        GetComponent<RectTransform>().localPosition = new Vector2(0, -900);

        // 화면 밖에서 서서히 안쪽으로 이동
        GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, -490), 3f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
