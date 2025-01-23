using UnityEngine;

public class Attack : MonoBehaviour
{
    public AudioClip soundClip;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = soundClip;

        audioSource.Play();
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
