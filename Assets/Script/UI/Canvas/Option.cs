using UnityEngine;
using UnityEngine.UI;

public class Option : MonoBehaviour
{
    public Slider BackgroundVolumeSlider;
    public Slider EffactVolumeSlider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LoadSliderData();
    }

    public virtual void LoadSliderData()
    {
        GameManager.Instance.LoadPlayerSetting();
        BackgroundVolumeSlider.value = BackGroundMusic.Instance.LoadVolumeData();
        EffactVolumeSlider.value = ButtonClickAudio.Instance.LoadVolumeData();
        
    }

    public void ButtonBackToMenu()
    {
        ButtonClickAudio.Instance.PlayClip();
        SceneManager.Instance.Button_GoToMenu();
    }

    public virtual void ButtonOptionReset()
    {
        ButtonClickAudio.Instance.PlayClip();
        // 각 value를 기본값으로 초기화
        BackgroundVolumeSlider.value = 0.8f;
        EffactVolumeSlider.value = 1.0f;
    }

    // 슬라이더에서 값을 변경 시 실행
    public void BackgroundVolumeUpdate()
    {
        if (BackgroundVolumeSlider != null)
        {
            BackGroundMusic.Instance.VolumeUpdate(BackgroundVolumeSlider.value);
        }
        else
        {
            Debug.LogError("BackgroundVolumeSlider == null");
        }
    }

    // 슬라이더에서 값을 변경 시 실행
    public void EffactVolumeUpdate()
    {

        if (EffactVolumeSlider != null)
        {
            ButtonClickAudio.Instance.VolumeUpdate(EffactVolumeSlider.value);
        }
        else
        {
            Debug.LogError("EffactVolumeSlider == null");
        }
    }

    // 슬라이더에서 마우스를 떼어낼 때 실행
    public void EffactVolumeChanged()
    {
        ButtonClickAudio.Instance.PlayClip();
    }
}
