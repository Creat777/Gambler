using PublicSet;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPopUp : PopUpBase<InventoryPopUp>
{
    public sItem currentClickItem; // ���� Ŭ���� ������

    HashSet<sItem> playerItems
    {
        get { return ItemManager.ItemHashSet; }
    }

    private void OnEnable()
    {
        RefreshPopUp();
    }


    public override void RefreshPopUp()
    {
        Debug.Log($"Player_items.Count == {playerItems.Count}");
        RefreshPopUp(playerItems.Count,
            () =>
            {
                foreach (sItem item in playerItems)
                {
                    // ������������ �ʱ�ȭ�� ��ü
                    ItemDefault itemDefault = ActiveObjList[item.id].GetComponent<ItemDefault>(); ;

                    // ������ ���������� ȣ��
                    cItemInfo itemInfo = CsvManager.Instance.GetItemInfo(item.type);

                    // Ȱ��ȭ�� �� ��ü�� ������ �ʱ�ȭ
                    if (itemDefault != null)
                    {
                        itemDefault.InitItemData(item, itemInfo);
                    }
                    else
                    {
                        Debug.LogAssertion($"{itemDefault.gameObject.name}�� itemScript == null");
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
                                List<string> list = new List<string>();
                                list.Add($"������ �̸� : {itemInfo.name}\n");
                                list.AddRange(itemInfo.descriptionList);

                                yesOrNoPopUp_Script.SetYesText("����ϱ�");
                                yesOrNoPopUp_Script.UpdateMainDescription(list);

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
                                            itemInfo.itemCallback();
                                            itemDefault.UsedByPlayer();
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
                                GameManager.connector_InGame.popUpView_Script.CheckPopUpOpen();

                                // �˾�â�� �ʱ�ȭ
                                CheckPopUp checkPopUp_Script = GameManager.connector_InGame.popUpView_Script.checkPopUp.GetComponent<CheckPopUp>();

                                List<string> list = new List<string>();

                                list.Add($"������ �̸� : {itemInfo.name}\n");
                                list.AddRange(itemInfo.descriptionList);
                                checkPopUp_Script.UpdateMainDescription(itemInfo.descriptionList);

                                currentClickItem = item;
                            });
                    }

                    // ������ �ʱ�ȭ
                    itemDefault.transform.localScale = Vector3.one;
                }
            });

    }
}
