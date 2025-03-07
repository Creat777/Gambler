using UnityEngine;
using UnityEngine.UI;

public class SelectAttackObject : MonoBehaviour
{
    public Image image {  get; private set; }
    public Button button { get; private set; }

    private void Awake()
    {
        image = GetComponent<Image>();
        button = GetComponent<Button>();
    }
}
