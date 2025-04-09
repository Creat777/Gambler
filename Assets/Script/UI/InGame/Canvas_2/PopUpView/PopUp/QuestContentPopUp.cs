using UnityEngine;

public class QuestContentPopUp : PopUpBase<QuestContentPopUp>
{
    public QuestDescriptionPanel descriptionPanel {  get; private set; }

    public void InitPopUp()
    {
        InitializePool(1);
        GameObject obj = GetObject();
        descriptionPanel = obj.GetComponent<QuestDescriptionPanel>();
        ChangeContentRectTransform();
    }
}
