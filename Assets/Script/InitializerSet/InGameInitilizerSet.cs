using UnityEngine;


// �����ͽ� �̱��� ��ü�� ó�� ���
public class InGameInitilizerSet : MonoBehaviour
{
    public KeyBoardView keyBoardView;
    void Start()
    {
        Player.Instance.keyBoardView = keyBoardView;
        GameManager.Instance.Join_In_Game();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
