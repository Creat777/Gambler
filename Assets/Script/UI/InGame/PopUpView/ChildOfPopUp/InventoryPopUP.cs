using PublicSet;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPopUp : PopUpBase<InventoryPopUp>
{
    public sItem currentClickItem; // ���� Ŭ���� ������


    public override void RefreshPopUp()
    {
        // �÷��̾��� ������ ���� �ҷ�����
        HashSet<sItem> Player_items = PlayerPrefsManager.Instance.LoadItems();

        Debug.Log($"Player_items.Count == {Player_items.Count}");
        RefreshPopUp(Player_items.Count,
            () =>
            {
                int num = 0;
                foreach (sItem item in Player_items)
                {
                    // ������������ �ʱ�ȭ�� ��ü
                    GameObject obj = ActiveObjList[num];

                    // ������ ���������� ȣ��
                    cItemInfo itemInfo = CsvManager.Instance.GetItemInfo(item.type);

                    // Ȱ��ȭ�� �� ��ü�� ������ �ʱ�ȭ
                    ItemDefault itemDefault = obj.GetComponent<ItemDefault>();
                    if (itemDefault != null)
                    {
                        itemDefault.InitItemData(item);
                    }
                    else
                    {
                        Debug.LogAssertion($"{obj.name}�� itemScript == null");
                    }


                    // �������� ��밡���� ��� 
                    if (itemInfo.isAvailable)
                    {
                        // ������ ������ ���� ��ư�ݹ� ����
                        itemDefault.SetButtonCallback(
                            () =>
                            {
                                YesOrNoPopUp yesOrNoPopUp_Script = (GameManager.connector as Connector_InGame).popUpView_Script.yesOrNoPopUp.GetComponent<YesOrNoPopUp>();
                                if (yesOrNoPopUp_Script == null) { Debug.LogAssertion("�˾�â�� ����"); return; }

                                // �������� ������ �˾�â�� ����
                                (GameManager.connector as Connector_InGame).popUpView_Script.YesOrNoPopUpOpen();

                                // �˾�â�� ���� �� ������ �̸��� ������ ������Ʈ
                                string inputString = $"������ �̸� : {itemInfo.name}\n\n{itemInfo.description}";

                                yesOrNoPopUp_Script.UpdateMainDescription(inputString);

                                // �ݹ鿡�� Ȱ���ϱ� ���� ������ ������ ����
                                currentClickItem = item;

                                // �Ҹ� �������� ���
                                if (itemInfo.isConsumable)
                                {
                                    Debug.Log($"{item}�� �������� �Ҹ��Դϴ�.");
                                    // yes������ ������ ������ ���� �ݹ��� ó���ϵ��� ����
                                    yesOrNoPopUp_Script.SetYesButtonCallBack(
                                        () =>
                                        {
                                            yesOrNoPopUp_Script.gameObject.SetActive(false);
                                            itemInfo.itemCallback();
                                            obj.GetComponent<ItemDefault>().UsedByPlayer();
                                            RefreshPopUp();
                                        }
                                        );
                                }
                                // �Ҹ��� �ƴ� ���
                                else
                                {
                                    Debug.Log($"{item}�� �������� �Ҹ��� �ƴ�.");
                                    yesOrNoPopUp_Script.SetYesButtonCallBack(
                                        () =>
                                        {
                                            yesOrNoPopUp_Script.gameObject.SetActive(false);
                                            itemInfo.itemCallback();
                                        }
                                        );
                                }
                            });
                    }

                    // ��Ÿ ������ ��� 
                    else if (itemInfo.isAvailable == false)
                    {
                        //Ȯ��â�� �������� ����
                        itemDefault.SetButtonCallback(
                            () =>
                            {
                                (GameManager.connector as Connector_InGame).popUpView_Script.CheckPopUpOpen();

                                // �˾�â�� �ʱ�ȭ
                                CheckPopUp checkPopUp_Script = (GameManager.connector as Connector_InGame).popUpView_Script.checkPopUp.GetComponent<CheckPopUp>();
                                string inputString = $"������ �̸� : {itemInfo.name}\n\n{itemInfo.description}";
                                checkPopUp_Script.UpdateMainDescription(inputString);

                                currentClickItem = item;
                            });
                    }

                    // ������ �ʱ�ȭ
                    obj.transform.localScale = Vector3.one;

                    num++;
                }
            });

    }

    
}
