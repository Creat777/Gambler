using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public abstract class SimplePopUpBase : MonoBehaviour
{
    [SerializeField]protected Text mainDescription;

    public virtual void UpdateMainDescription(List<string> descriptionList)
    {
        // üũ�˾� �⺻ ������Ʈ
        CheckPopUp check = this as CheckPopUp;
        if(check != null)
        {
            check.PopUpUpChange(checkCase.@default);
        }

        if(descriptionList.Count > 0)
            mainDescription.text = descriptionList[0];

        // 2�� �̻��� ���ڿ��� ��� �ٹٲ��� ������
        for (int i = 1; i<descriptionList.Count; i++)
        {
            mainDescription.text += $"\n{descriptionList[i]}";
        }
    }

    public virtual void UpdateMainDescription(string description)
    {
        // üũ�˾� �⺻ ������Ʈ
        CheckPopUp check = this as CheckPopUp;
        if (check != null)
        {
            check.PopUpUpChange(checkCase.@default);
        }

        mainDescription.text = description;
    }
}
