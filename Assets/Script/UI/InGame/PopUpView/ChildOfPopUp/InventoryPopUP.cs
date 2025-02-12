using PublicSet;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPopUp : PopUp
{
    private void OnEnable()
    {
        // ���� ��� ����
        foreach (Transform child in content.transform)
        {
            Destroy(child.gameObject);
        }

        HashSet<Item> Player_items = PlayerPrefsManager.Instance.LoadItems();

        foreach (Item item in Player_items)
        {
            // ������ ���������� ȣ��
            cItemInfo itemInfo = CsvManager.Instance.GetItemInfo((eItemSerialNumber)item.serialNumber);

            // ������ �ν��Ͻ�
            GameObject obj = Instantiate(itemInfo.itemPrefab);
            obj.SetActive(true);

            // ������ Ŭ���� ó�� �Լ� ����
            Button button = obj.GetComponent<Button>();
            button.onClick.RemoveAllListeners();

            // �������� ��밡���� ��� 
            if (itemInfo.isAvailable)
            {
                //������ Ŭ���� ��뿩�θ� ���� �˾�â�� �������� �ݹ��� ����
                button.onClick.AddListener(GameManager.Connector.popUpView_Script.YesOrNoPopUpOpen);

                // �˾�â�� �ʱ�ȭ �� yes������ ������ ������ ���� �ݹ��� ó���ϵ��� ����
                YesOrNoPopUp yesOrNoPopUp_Script =  GameManager.Connector.popUpView_Script.yesOrNoPopUp.GetComponent<YesOrNoPopUp>();
                string inputString = $"������ �̸� : {itemInfo.name}\n\n{itemInfo.description}";
                yesOrNoPopUp_Script.UpdateMainDescription(inputString);
                yesOrNoPopUp_Script.AddYesButtonCallBack(
                    ()=>
                    { 
                        yesOrNoPopUp_Script.gameObject.SetActive(false);
                        itemInfo.itemCallback(); 
                    }
                    );
            }

            // ��Ÿ ������ ��� 
            else if(itemInfo.isAvailable == false)
            {
                //Ȯ��â�� �������� ����
                button.onClick.AddListener(GameManager.Connector.popUpView_Script.CheckPopUpOpen);

                // �˾�â�� �ʱ�ȭ
                CheckPopUp checkPopUp_Script = GameManager.Connector.popUpView_Script.checkPopUp.GetComponent<CheckPopUp>();
                string inputString = $"������ �̸� : {itemInfo.name}\n\n{itemInfo.description}";
                checkPopUp_Script.UpdateMainDescription(inputString);
            }

            // �θ�ü ����
            obj.transform.SetParent(content.transform);

            // ������ �ʱ�ȭ
            obj.transform.localScale = Vector3.one;
        }
    }
}
