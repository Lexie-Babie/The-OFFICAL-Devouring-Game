using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class CombatInventoryUI : MonoBehaviour
{
    public static CombatInventoryUI Instance;
    public GameObject itemSelectionPanel;
    public TextMeshProUGUI panelTitleText;
    public Transform itemButtonContainer;
    public GameObject itemButtonPrefab;
    private System.Action<ItemData> onItemSelected; // callback for what to do when item is slected

    void Awake() //makes this script findable to other scripts 
    {
        Instance = this;
    }

    public void ShowDamageItems(System.Action<ItemData> callback) //called onCookButon - shows damage items
    {
        panelTitleText.text = "Ingredients";
        PopulatePanel(InventoryManager.Instance.damageInventory, callback);
    }
    public void ShowHealItems(System.Action<ItemData> callback) //called onHealButon - shows heal items
    {
        panelTitleText.text = "Healing Salves";
        PopulatePanel(InventoryManager.Instance.healInventory, callback);
    }

    void PopulatePanel(List<ItemData> items, System.Action<ItemData> callback)
    {
        onItemSelected = callback;

        foreach (Transform child in itemButtonContainer) // clears out old buttons from the container before adding new ones
        {
            Destroy(child.gameObject);
        }

        foreach (ItemData item in items) // loops through the items and creates a button for each one
        {
            GameObject buttonObj = Instantiate(itemButtonPrefab, itemButtonContainer); // makes a button for each item in the inventory (the prefab)
            TextMeshProUGUI buttonText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = item.itemName + " (Value: " + item.value + ")"; // sets the button text to the item name and value
            ItemData captureItem = item; // capture the item in a local variable to avoid closure issues (whatever that means :))
            Button button = buttonObj.GetComponent<Button>();
            button.onClick.AddListener(() => OnItemButtonClicked(captureItem)); // adds a listener to the button that calls OnItemButtonClicked with the captured item when clicked
        }

        itemSelectionPanel.SetActive(true);
    }
    
    void OnItemButtonClicked(ItemData item)
    {
        HidePanel();
        onItemSelected?.Invoke(item); // calls the callback with the selected item
    }

    public void HidePanel()
    {
        itemSelectionPanel.SetActive(false);
        foreach (Transform child in itemButtonContainer) // clears out the buttons from the container when hiding the panel, so that it's ready for the next time it's shown
        {
            Destroy(child.gameObject);
        }
    }
     
}



