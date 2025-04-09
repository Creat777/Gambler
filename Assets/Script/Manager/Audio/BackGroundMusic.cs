using UnityEngine;
using UnityEngine.Audio;

public class BackGroundMusic : GameAudio
{
    [SerializeField] private AudioClip defaultMusic;
    [SerializeField] private AudioClip playMusic;
    //public AudioClip bossMusic;
    public static BackGroundMusic Instance { get; private set; }

    private void Awake()
    {
        volumeKey = "BackGroundMusic";
    }
    void Start()
    {
        if (audioSource != null)
        {
            if(audioSource.isPlaying == false)
            {
                audioSource.loop = true;
                audioSource.clip = defaultMusic;
                audioSource.Play();
                Debug.Log("���� ��ŸƮ");
            }
            else
            {
                Debug.Log("������� ������");
            }
        }
        else
        {
            Debug.LogError("audioSource == null");
        }
    }
    public void DefaultPlay()
    {
        if (audioSource != null)
        {
            // �̹� ����Ǵ� �����̸� �Լ� ����
            if (audioSource.clip == defaultMusic) return;

            audioSource.Stop();
            audioSource.loop = true;
            audioSource.clip = defaultMusic;
            audioSource.Play();
        }
        else
        {
            Debug.LogError("audioSource == null");
        }
    }

    public void LetsPlay()
    {

        if (audioSource != null)
        {
            // �̹� ����Ǵ� �����̸� �Լ� ����
            if (audioSource.clip == playMusic) return;

            audioSource.Stop();
            audioSource.loop = true;
            audioSource.clip = playMusic;
            audioSource.Play();
        }
        else
        {
            Debug.LogError("audioSource == null");
        }
    }

    public override float LoadVolumeData()
    {
        float value = 1.0f;

        value = PlayerSaveManager.Instance.LoadData(volumeKey, 0.8f);

        audioSource.volume = value;

        return value;
    }

}
