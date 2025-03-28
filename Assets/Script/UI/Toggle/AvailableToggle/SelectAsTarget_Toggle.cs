using UnityEngine;
using UnityEngine.UI;

public class SelectAsTarget_Toggle : ToggleBase
{
    private CardGamePlayerBase player;

    private void Start()
    {
        SetToggleCallback(SelectPlayer);
    }

    public void SetPlayer(CardGamePlayerBase player)
    {
        this.player = player;
        SetInteractable(true);
    }

    public void SelectPlayer(bool isSelected)
    {
        if(isSelected)
        {
            bool result = CardGamePlayManager.Instance.playerMe.TrySetAttackTarget(player);

            GameAssistantPopUp_OnlyOneLives.Instance.PlaceRestrictionToSelections(this);
        }
        else
        {
            CardGamePlayManager.Instance.playerMe.ClearAttackTarget();

            GameAssistantPopUp_OnlyOneLives.Instance.LiftRestrictionToSelections(this);
        }
    }
}
