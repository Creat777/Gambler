using UnityEngine;
using UnityEngine.UI;

public class EventView : MonoBehaviour
{
    [SerializeField] private Text _eventText;
    public Text eventText {get {return _eventText;} set { _eventText = value; } }
    public void SetTextContent(string value)
    {
        eventText.text = value;
    }
    public void SetTextColer(Color value)
    {
        eventText.color = value;
    }
}
