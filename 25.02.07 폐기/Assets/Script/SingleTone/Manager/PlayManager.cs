using UnityEngine;

public class PlayManager : Singleton<PlayManager>
{
    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        AddExp();
        AddItem();
        DoQuest();
    }

    public void AddExp()
    {

    }

    public void LevelUp()
    {

    }

    public void AddItem()
    {
        // PlayerPrefs ø° æ∆¿Ã≈€ »πµÊ ±‚∑œ
    }

    public void DoQuest()
    {

    }

    void Update()
    {
        
    }
}
