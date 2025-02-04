using UnityEngine;

public class InventoryPopUp : PopUp
{
    public GameObject content;
    public GameObject paperPrefab;
    public GameObject eggfab;
    public GameObject meatfab;
    private void OnEnable()
    {
        // ���� ��� ����
        foreach (Transform child in content.transform)
        {
            Destroy(child.gameObject);
        }

        // Do => ���ο� ��� �߰�
    }
}
