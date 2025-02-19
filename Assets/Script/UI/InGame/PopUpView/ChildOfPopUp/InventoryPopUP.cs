using PublicSet;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPopUp : PopUp
{
    public sItem currentClickItem; // 현재 클릭한 아이템

    private void Awake()
    {
    }

    private void OnEnable()
    {
        RefreshInventory();
    }

    public void RefreshInventory()
    {
        // 기존 목록 삭제
        foreach (Transform child in content.transform)
        {
            Destroy(child.gameObject);
        }

        // 플레이어의 아이템 정보 불러오기
        HashSet<sItem> Player_items = PlayerPrefsManager.Instance.LoadItems();
        
        foreach (sItem item in Player_items)
        {
            // 아이템 종합정보를 호출
            cItemInfo itemInfo = CsvManager.Instance.GetItemInfo(item.serialNumber);

            // 아이템 인스턴시
            GameObject obj = Instantiate(itemInfo.itemPrefab);
            obj.SetActive(true);

            // 인스턴시된 아이템에 각 정보를 저장
            ItemDefault itemScript = obj.GetComponent<ItemDefault>();
            if (itemScript != null)
            {
                itemScript.SaveItemData(item);
            }
            else
            {
                Debug.LogAssertion("itemScript == null");
            }


            // 아이템 클릭에 대한 콜백함수를 입력하기 전 모두 삭제
            Button button = obj.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.RemoveAllListeners();
            }
            else
            {
                Debug.LogAssertion("button ==  null");
            }
            

            // 아이템이 사용가능한 경우 
            if (itemInfo.isAvailable)
            {
                // 아이템 각각에 대한 버튼콜백 설정
                button.onClick.AddListener(
                    () => 
                    {
                        YesOrNoPopUp yesOrNoPopUp_Script = GameManager.Connector.popUpView_Script.yesOrNoPopUp.GetComponent<YesOrNoPopUp>();
                        if (yesOrNoPopUp_Script == null) { Debug.LogAssertion("팝업창이 없음"); return; }

                        // 아이템을 누르면 팝업창이 켜짐
                        GameManager.Connector.popUpView_Script.YesOrNoPopUpOpen();
                        
                        // 팝업창이 켜진 후 아이템 이름과 설명을 업데이트
                        string inputString = $"아이템 이름 : {itemInfo.name}\n\n{itemInfo.description}";

                        yesOrNoPopUp_Script.UpdateMainDescription(inputString);

                        // 콜백에서 활용하기 위해 아이템 정보를 저장
                        currentClickItem = item;

                        // 소모성 아이템인 경우
                        if (itemInfo.isConsumable)
                        {
                            Debug.Log($"{item}이 아이템은 소모성입니다.");
                            // yes선택을 누를시 아이템 고유 콜백을 처리하도록 연결
                            yesOrNoPopUp_Script.AddYesButtonCallBack(
                                () =>
                                {
                                    
                                    yesOrNoPopUp_Script.gameObject.SetActive(false);
                                    itemInfo.itemCallback();
                                    obj.GetComponent<ItemDefault>().UsedByPlayer();
                                }
                                );
                        }
                        // 소모성이 아닐 경우
                        else
                        {
                            Debug.Log($"{item}이 아이템은 소모성이 아님.");
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

            // 기타 잡템의 경우 
            else if (itemInfo.isAvailable == false)
            {
                //확인창만 나오도록 만듬
                button.onClick.AddListener(
                    ()=>
                    {
                        GameManager.Connector.popUpView_Script.CheckPopUpOpen();

                        // 팝업창을 초기화
                        CheckPopUp checkPopUp_Script = GameManager.Connector.popUpView_Script.checkPopUp.GetComponent<CheckPopUp>();
                        string inputString = $"아이템 이름 : {itemInfo.name}\n\n{itemInfo.description}";
                        checkPopUp_Script.UpdateMainDescription(inputString);

                        currentClickItem = item;
                    });
            }

            // 부모객체 설정
            obj.transform.SetParent(content.transform);

            // 스케일 초기화
            obj.transform.localScale = Vector3.one;
        }
    }

}
