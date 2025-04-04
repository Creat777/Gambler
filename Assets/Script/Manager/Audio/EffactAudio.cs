using UnityEngine;

public class EffactAudio : GameAudio
{
    [SerializeField] private AudioClip click;
    [SerializeField] private AudioClip effact;

    private void Awake()
    {
        volumeKey = "ButtonClickAudio";
    }

    void Start()
    {
        
    }

    private void Update()
    {
#if UNITY_EDITOR
        // 마우스 클릭을 감지
        if (Input.GetMouseButtonDown(0)) // 0은 좌클릭
        {
            playClick();
        }
#endif
        // 모바일 터치도 감지
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            playClick();
        }
    }

    private void playClick()
    {
        audioSource.clip = click;
        audioSource.Play();
    }

    public override float LoadVolumeData()
    {
        float value = 1.0f;

        //value = PlayerPrefsManager.Instance.LoadData(volumeKey, 1.0f);

        audioSource.volume = value;

        return value;
    }
}
