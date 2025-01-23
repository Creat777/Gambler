using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Coin : Item
{
    public GameObject scorePanel;
    private Score score;

    public int bonusScore;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bonusScore = 10;
    }

    private void OnEnable()
    {
        scorePanel = GameObject.Find("ScorePanel");
        score = scorePanel.GetComponent<Score>();
    }

    protected override void onTriggerPlayer(GameObject player)
    {
        score.bonusScore += bonusScore;
    }

    protected override void OutofScreen()
    {
        CoinMemoryPool.Instance.ReturnObject(gameObject);
    }
}
