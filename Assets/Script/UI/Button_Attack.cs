using UnityEngine;
using System.Collections;
using UnityEngine.Audio;


public class Button_Attack : MonoBehaviour
{
    public AudioClip soundClip;
    private AudioSource audioSource = null;
    public bool m_IsButtonDowning;
    private float attackDelay;
    private bool isDelay;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = soundClip;
        attackDelay = 1f;
        isDelay = false;
    }

    

    public void PointerDown()
    {
        m_IsButtonDowning = true;
    }

    public void PointerUp()
    {
        m_IsButtonDowning = false;
    }

    void Update()
    {
        if(m_IsButtonDowning && isDelay == false)
        {
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        isDelay = true;
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.volume = ButtonClickAudio.Instance.audioSource.volume;
            audioSource.Play();
        }
        BulletFactory.Instance.EnableBullets();
        yield return new WaitForSeconds(attackDelay/GameManager.Instance.gameSpeed);
        isDelay = false;
    }
}
