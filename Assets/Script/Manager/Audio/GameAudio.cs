using UnityEngine;

public abstract class GameAudio : MonoBehaviour
{
    public string volumeKey { get; protected set; }
    public AudioSource audioSource;

    public void VolumeUpdate(float value)
    {
        if (audioSource != null)
        {
            audioSource.volume = value;
            PlayerPrefsManager.Instance.SaveData(volumeKey, value); // 
        }
        else
        {
            Debug.LogError("audioSource == null");
        }
    }

    abstract public float LoadVolumeData();
}
