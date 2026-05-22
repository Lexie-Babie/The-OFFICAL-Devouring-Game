using UnityEngine;
using TMPro;

public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance; //makes PopupManager findable without needing to make a new variable each script (i believe)
    public GameObject popupPanel; //the panel that will be shown when a node is clicked
    public TextMeshProUGUI popupText; //the text component that will display the node's information

    void Awake()
    {
        Instance = this; //sets the instance variable to the current instance of PopupManager when the script is loaded
    }

    public void ShowPopup(string message)
    {
        popupText.text = message;
        popupPanel.SetActive(true);
    }

    public void PopupClose()
    {
        popupPanel.SetActive(false);
        MapManager.Instance.OnPopupClosed();
    }
}
