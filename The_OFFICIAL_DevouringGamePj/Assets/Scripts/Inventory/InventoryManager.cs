using UnityEngine;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public int maxDamageSlots = 10;
    public int MaxHealSlots = 5;
    public List<ItemData> damageInventory = new List<ItemData>();
    public List<ItemData> healInventory = new List<ItemData>();

    void Awake() // ensures only one instance of the inventory manager exists
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); //makes sure the inventory manager persists across scene changes
        }
        else
        {
            Destroy(gameObject); //prevents duplicates
        }
    }

    public bool AddItem(ItemData item) // adds an item to the appropriate inventory based on its type, and checks for inventory limits
    {
        if (item.itemType == PickupType.Damage)
        {
           if (damageInventory.Count >= maxDamageSlots)
            {
                Debug.Log("Damage Inventory full!");
                return false;
            }
            damageInventory.Add(item);
            Debug.Log(item.itemName + " added to damage inventory.");
        }
        else if (item.itemType == PickupType.Heal)
        {
            if (healInventory.Count >= MaxHealSlots)
            {
                Debug.Log("Heal Inventory full!");
                return false;
            }
            healInventory.Add(item);
            Debug.Log(item.itemName + " added to heal inventory.");
            Debug.Log("heal inventory count is now: " + healInventory.Count);
        }
        else
        {
            Debug.LogWarning("Unknown item type: " + item.itemType);
            return false;
        }
        // refreshes the UI if it's opened 
        if (InventoryUI.Instance != null) //This prevents unity from exploding since the inventory UI exists only in the MapScene at the moment
        {
            InventoryUI.Instance.RefreshUI();
        }
        return true;  
    }

    public void RemoveItem(ItemData item) // removes an item from the appropriate inventory based on its type
    {
        if (item.itemType == PickupType.Damage)
        {
            damageInventory.Remove(item);
        }
        else if (item.itemType == PickupType.Heal)
        {
            healInventory.Remove(item);
        }
        else
        {
            Debug.LogWarning("Unknown item type: " + item.itemType);
            return;
        }

        if (InventoryUI.Instance != null)
        {
            InventoryUI.Instance.RefreshUI();
        }
    }

    public void ClearAllInventories() // clears both inventories and refreshes the UI
    {
        damageInventory.Clear();
        healInventory.Clear();

        if (InventoryUI.Instance != null)
        {
            InventoryUI.Instance.RefreshUI();
        }
    } 
}
