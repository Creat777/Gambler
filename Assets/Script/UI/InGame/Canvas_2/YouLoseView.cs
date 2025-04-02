using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class YouLoseView : ButtonBase
{
    // 에디터
    public Text YouLoseText;
    public Text ContinueText;

    // 스크립트
    Sequence YouLoseSequence;
    Sequence ContinueSequence;
    readonly Color OriginColor = new Color(1f, 1f, 1f, 1f);
    readonly Color BlinkColor = new Color(1f, 1f, 1f, 0f);

    private void OnEnable()
    {
        // 화면 클릭시 로비로 이동
        SetButtonCallback(()=> SceneManager.LoadScene(1));
        PlaySequnce_YouLoseText_AppearAnimaition();
    }

    private void OnDisable()
    {
        if(YouLoseSequence != null && YouLoseSequence.IsPlaying()) YouLoseSequence.Kill();
        if (ContinueSequence != null && ContinueSequence.IsPlaying()) ContinueSequence.Kill();
    }

    
    public void PlaySequnce_YouLoseText_AppearAnimaition()
    {
        YouLoseText.color = BlinkColor;
        ContinueText.color = BlinkColor;

        YouLoseSequence = DOTween.Sequence();
        float delay = 3f;

        YouLoseSequence.AppendInterval(delay * 0.1f);
        YouLoseSequence.Append(YouLoseText.DOColor(OriginColor, delay).SetEase(Ease.Linear));
        YouLoseSequence.AppendCallback(PlaySequnce_ContinueText_BlinkAnimation);
        YouLoseSequence.SetLoops(1);
        YouLoseSequence.Play();
    }


    
    public void PlaySequnce_ContinueText_BlinkAnimation()
    {
        ContinueSequence = DOTween.Sequence();
        float delay = 1f;

        ContinueSequence.Append(ContinueText.DOColor(OriginColor, delay * 1.5f).SetEase(Ease.Linear));
        ContinueSequence.Append(ContinueText.DOColor(BlinkColor, delay).SetEase(Ease.Linear));
        ContinueSequence.AppendInterval(delay * 0.1f);
        ContinueSequence.SetLoops(-1);
        ContinueSequence.Play();
    }
}
