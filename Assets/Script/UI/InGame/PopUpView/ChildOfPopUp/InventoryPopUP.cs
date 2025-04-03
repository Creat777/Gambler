using PublicSet;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPopUp : PopUpBase<InventoryPopUp>
{
    public sItem currentClickItem; // 현재 클릭한 아이템


    public override void RefreshPopUp()
    {
        // 플레이어의 아이템 정보 불러오기
        HashSet<sItem> Player_items = PlayerPrefsManager.Instance.LoadItems();

        Debug.Log($"Player_items.Count == {Player_items.Count}");
        RefreshPopUp(Player_items.Count,
            () =>
            {
                int num = 0;
                foreach (sItem item in Player_items)
                {
                    // 아이템정보로 초기화될 객체
                    GameObject obj = ActiveObjList[num];

                    // 아이템 종합정보를 호출
                    cItemInfo itemInfo = CsvManager.Instance.GetItemInfo(item.type);

                    // 활서화된 각 객체에 정보를 초기화
                    ItemDefault itemDefault = obj.GetComponent<ItemDefault>();
                    if (itemDefault != null)
                    {
                        itemDefault.InitItemData(item);
                    }
                    else
                    {
                        Debug.LogAssertion($"{obj.name}은 itemScript == null");
                    }


                    // 아이템이 사용가능한 경우 
                    if (itemInfo.isAvailable)
                    {
                        // 아이템 각각에 대한 버튼콜백 설정
                        itemDefault.SetButtonCallback(
                            () =>
                            {
                                YesOrNoPopUp yesOrNoPopUp_Script = (GameManager.connector as Connector_InGame).popUpView_Script.yesOrNoPopUp.GetComponent<YesOrNoPopUp>();
                                if (yesOrNoPopUp_Script == null) { Debug.LogAssertion("팝업창이 없음"); return; }

                                // 아이템을 누르면 팝업창이 켜짐
                                (GameManager.connector as Connector_InGame).popUpView_Script.YesOrNoPopUpOpen();

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
                                // 소모성이 아닐 경우
                                else
                                {
                                    Debug.Log($"{item}이 아이템은 소모성이 아님.");
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

                    // 기타 잡템의 경우 
                    else if (itemInfo.isAvailable == false)
                    {
                        //확인창만 나오도록 만듬
                        itemDefault.SetButtonCallback(
                            () =>
                            {
                                (GameManager.connector as Connector_InGame).popUpView_Script.CheckPopUpOpen();

                                // 팝업창을 초기화
                                CheckPopUp checkPopUp_Script = (GameManager.connector as Connector_InGame).popUpView_Script.checkPopUp.GetComponent<CheckPopUp>();
                                string inputString = $"아이템 이름 : {itemInfo.name}\n\n{itemInfo.description}";
                                checkPopUp_Script.UpdateMainDescription(inputString);

                                currentClickItem = item;
                            });
                    }

                    // 스케일 초기화
                    obj.transform.localScale = Vector3.one;

                    num++;
                }
            });

    }

    
}
