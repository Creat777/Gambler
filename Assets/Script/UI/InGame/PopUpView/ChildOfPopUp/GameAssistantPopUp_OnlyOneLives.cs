using UnityEngine;

public class GameAssistantPopUp_OnlyOneLives : PopUpBase
{

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ChangeContentRectTransform();
        }
#endif
    }
}
