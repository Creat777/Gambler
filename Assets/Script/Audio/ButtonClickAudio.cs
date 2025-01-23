using UnityEngine;

public class ButtonClickAudio : GameAudio
{

    public static ButtonClickAudio Instance { get; private set; }

    private void MakeSingleTone()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Awake()
    {
        MakeSingleTone();
        volumeKey = "ButtonClickAudio";
    }

    void Start()
    {
        
    }

    public void PlayClip()
    {
        audioSource.Play();
    }
}
