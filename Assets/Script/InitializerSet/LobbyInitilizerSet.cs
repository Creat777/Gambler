using UnityEngine;


// �����ͽ� �̱��� ��ü�� ó�� ���
public class LobbyInitilizerSet : MonoBehaviour
{
    //

    public KeyBoardView keyBoardView;
    void Start()
    {
        Player.Instance.keyBoardView = keyBoardView;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
