using PublicSet;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPopUp : PopUp
{
    private void OnEnable()
    {
        // 기존 목록 삭제
        foreach (Transform child in content.transform)
        {
            Destroy(child.gameObject);
        }

        HashSet<Item> Player_items = PlayerPrefsManager.Instance.LoadItems();

        foreach (Item item in Player_items)
        {
            // 아이템 종합정보를 호출
            cItemInfo itemInfo = CsvManager.Instance.GetItemInfo((eItemSerialNumber)item.serialNumber);

            // 아이템 인스턴시
            GameObject obj = Instantiate(itemInfo.itemPrefab);
            obj.SetActive(true);

            // 아이템 클릭시 처리 함수 정리
            Button button = obj.GetComponent<Button>();
            button.onClick.RemoveAllListeners();

            // 아이템이 사용가능한 경우 
            if (itemInfo.isAvailable)
            {
                //아이템 클릭시 사용여부를 묻는 팝업창이 나오도록 콜백을 연결
                button.onClick.AddListener(GameManager.Connector.popUpView_Script.YesOrNoPopUpOpen);

                // 팝업창을 초기화 및 yes선택을 누를시 아이템 고유 콜백을 처리하도록 연결
                YesOrNoPopUp yesOrNoPopUp_Script =  GameManager.Connector.popUpView_Script.yesOrNoPopUp.GetComponent<YesOrNoPopUp>();
                string inputString = $"아이템 이름 : {itemInfo.name}\n\n{itemInfo.description}";
                yesOrNoPopUp_Script.UpdateMainDescription(inputString);
                yesOrNoPopUp_Script.AddYesButtonCallBack(
                    ()=>
                    { 
                        yesOrNoPopUp_Script.gameObject.SetActive(false);
                        itemInfo.itemCallback(); 
                    }
                    );
            }

            // 기타 잡템의 경우 
            else if(itemInfo.isAvailable == false)
            {
                //확인창만 나오도록 만듬
                button.onClick.AddListener(GameManager.Connector.popUpView_Script.CheckPopUpOpen);

                // 팝업창을 초기화
                CheckPopUp checkPopUp_Script = GameManager.Connector.popUpView_Script.checkPopUp.GetComponent<CheckPopUp>();
                string inputString = $"아이템 이름 : {itemInfo.name}\n\n{itemInfo.description}";
                checkPopUp_Script.UpdateMainDescription(inputString);
            }

            // 부모객체 설정
            obj.transform.SetParent(content.transform);

            // 스케일 초기화
            obj.transform.localScale = Vector3.one;
        }
    }
}
