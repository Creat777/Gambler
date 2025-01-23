using UnityEngine;
using UnityEngine.UI;

public class OptionCanvas : Option
{
    public Text difficulty_ImageText;
    public Slider difficultySlider;

    void Start()
    {
        LoadSliderData();
    }

    public override void LoadSliderData()
    {
        base.LoadSliderData();
        // 현재 게임 난이도 슬라이더에 적용
        if(difficultySlider!= null)
        {
            difficultySlider.value = GameManager.Instance.gameDifficulty;
        }
        
    }

    public override void ButtonOptionReset()
    {
        base.ButtonOptionReset();
        difficultySlider.value = 3.0f;
    }

    // 슬라이더에서 값을 변경 시 실행
    public void DifficultyUpdate()
    {
        // 슬라이더값으로 게임 난이도 반영 및 저장
        int difficultyValue = (int)difficultySlider.value;
        difficulty_ImageText.text = $"게임 난이도 : {difficultyValue.ToString()}";
        GameManager.Instance.gameDifficulty = difficultyValue;
        PlayerInfoManager.Instance.SaveData(GameManager.Instance.gameDifficulyKey, difficultyValue);
        Debug.Log("gameDifficulty : " + difficultyValue);
    }
}
