using UnityEngine;

public class CanvasDafualt : MonoBehaviour
{
    private void Awake()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
