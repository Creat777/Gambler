using UnityEngine;

public class GameAudio : MonoBehaviour
{
    public string volumeKey { get; protected set; }
    public AudioSource audioSource;

    public void VolumeUpdate(float value)
    {
        if (audioSource != null)
        {
            audioSource.volume = value;
            PlayerInfoManager.Instance.SaveData(volumeKey, value); // 
        }
        else
        {
            Debug.LogError("audioSource == null");
        }
    }

    public float LoadVolumeData()
    {
        float value = 1.0f;
        if (volumeKey == "BackGroundMusic")
        {
            value = PlayerInfoManager.Instance.LoadData(volumeKey, 0.8f);
        }
        else if (volumeKey == "ButtonClickAudio")
        {
            value = PlayerInfoManager.Instance.LoadData(volumeKey, 1.0f);
        }
        audioSource.volume = value;
        return value;
    }
}
