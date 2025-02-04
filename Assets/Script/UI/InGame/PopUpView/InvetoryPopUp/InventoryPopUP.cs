using UnityEngine;

public class InventoryPopUp : PopUp
{
    public GameObject content;
    public GameObject paperPrefab;
    public GameObject eggfab;
    public GameObject meatfab;
    private void OnEnable()
    {
        // 기존 목록 삭제
        foreach (Transform child in content.transform)
        {
            Destroy(child.gameObject);
        }

        // Do => 새로운 목록 추가
    }
}
