using PublicSet;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPopUp : PopUp
{
    public sItem currentClickItem; // ���� Ŭ���� ������

    private void Awake()
    {
    }

    private void OnEnable()
    {
        RefreshInventory();
    }

    public void RefreshInventory()
    {
        // ���� ��� ����
        foreach (Transform child in content.transform)
        {
            Destroy(child.gameObject);
        }

        // �÷��̾��� ������ ���� �ҷ�����
        HashSet<sItem> Player_items = PlayerPrefsManager.Instance.LoadItems();
        
        foreach (sItem item in Player_items)
        {
            // ������ ���������� ȣ��
            cItemInfo itemInfo = CsvManager.Instance.GetItemInfo(item.serialNumber);

            // ������ �ν��Ͻ�
            GameObject obj = Instantiate(itemInfo.itemPrefab);
            obj.SetActive(true);

            // �ν��Ͻõ� �����ۿ� �� ������ ����
            ItemDefault itemScript = obj.GetComponent<ItemDefault>();
            if (itemScript != null)
            {
                itemScript.SaveItemData(item);
            }
            else
            {
                Debug.LogAssertion("itemScript == null");
            }


            // ������ Ŭ���� ���� �ݹ��Լ��� �Է��ϱ� �� ��� ����
            Button button = obj.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.RemoveAllListeners();
            }
            else
            {
                Debug.LogAssertion("button ==  null");
            }
            

            // �������� ��밡���� ��� 
            if (itemInfo.isAvailable)
            {
                // ������ ������ ���� ��ư�ݹ� ����
                button.onClick.AddListener(
                    () => 
                    {
                        YesOrNoPopUp yesOrNoPopUp_Script = GameManager.Connector.popUpView_Script.yesOrNoPopUp.GetComponent<YesOrNoPopUp>();
                        if (yesOrNoPopUp_Script == null) { Debug.LogAssertion("�˾�â�� ����"); return; }

                        // �������� ������ �˾�â�� ����
                        GameManager.Connector.popUpView_Script.YesOrNoPopUpOpen();
                        
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
                            yesOrNoPopUp_Script.AddYesButtonCallBack(
                                () =>
                                {
                                    
                                    yesOrNoPopUp_Script.gameObject.SetActive(false);
                                    itemInfo.itemCallback();
                                    obj.GetComponent<ItemDefault>().UsedByPlayer();
                                }
                                );
                        }
                        // �Ҹ��� �ƴ� ���
                        else
                        {
                            Debug.Log($"{item}�� �������� �Ҹ��� �ƴ�.");
                            yesOrNoPopUp_Script.AddYesButtonCallBack(
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
                button.onClick.AddListener(
                    ()=>
                    {
                        GameManager.Connector.popUpView_Script.CheckPopUpOpen();

                        // �˾�â�� �ʱ�ȭ
                        CheckPopUp checkPopUp_Script = GameManager.Connector.popUpView_Script.checkPopUp.GetComponent<CheckPopUp>();
                        string inputString = $"������ �̸� : {itemInfo.name}\n\n{itemInfo.description}";
                        checkPopUp_Script.UpdateMainDescription(inputString);

                        currentClickItem = item;
                    });
            }

            // �θ�ü ����
            obj.transform.SetParent(content.transform);

            // ������ �ʱ�ȭ
            obj.transform.localScale = Vector3.one;
        }
    }

}
