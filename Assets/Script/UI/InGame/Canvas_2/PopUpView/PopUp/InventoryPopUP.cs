using PublicSet;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPopUp : PopUpBase<InventoryPopUp>
{
    public sItem currentClickItem; // 현재 클릭한 아이템

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
                    // 아이템정보로 초기화될 객체
                    ItemDefault itemDefault = ActiveObjList[item.id].GetComponent<ItemDefault>(); ;

                    // 아이템 종합정보를 호출
                    cItemInfo itemInfo = CsvManager.Instance.GetItemInfo(item.type);

                    // 활성화된 각 객체에 정보를 초기화
                    if (itemDefault != null)
                    {
                        itemDefault.InitItemData(item, itemInfo);
                    }
                    else
                    {
                        Debug.LogAssertion($"{itemDefault.gameObject.name}은 itemScript == null");
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
                                List<string> list = new List<string>();
                                list.Add($"아이템 이름 : {itemInfo.name}\n");
                                list.AddRange(itemInfo.descriptionList);

                                yesOrNoPopUp_Script.SetYesText("사용하기");
                                yesOrNoPopUp_Script.UpdateMainDescription(list);

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
                                            itemInfo.itemCallback();
                                            itemDefault.UsedByPlayer();
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
                                GameManager.connector_InGame.popUpView_Script.CheckPopUpOpen();

                                // 팝업창을 초기화
                                CheckPopUp checkPopUp_Script = GameManager.connector_InGame.popUpView_Script.checkPopUp.GetComponent<CheckPopUp>();

                                List<string> list = new List<string>();

                                list.Add($"아이템 이름 : {itemInfo.name}\n");
                                list.AddRange(itemInfo.descriptionList);
                                checkPopUp_Script.UpdateMainDescription(itemInfo.descriptionList);

                                currentClickItem = item;
                            });
                    }

                    // 스케일 초기화
                    itemDefault.transform.localScale = Vector3.one;
                }
            });

    }
}
