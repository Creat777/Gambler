using UnityEngine;

public class AudioManager : MonoBehaviour
{
    const string masterVolumeKey = "masterVolumeKey100";
    public string MasterVolumeKey {  get { return masterVolumeKey; } }

    [SerializeField]private BackGroundMusic backGroundMusic;
    public BackGroundMusic BackGroundMusic { get { return backGroundMusic; } }

    [SerializeField] private EffactAudio effactAudio;
    public EffactAudio EffactAudio { get { return effactAudio; } }
}
