using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance;
    public GameObject bookPanel;
    public GameObject damagePage;
    public GameObject healPage;
    public List<InventorySlots> damageSlots = new List<InventorySlots>();
    public List<InventorySlots> healSlots = new List<InventorySlots>();
    private bool isOpen = false;

    void Awake()
    {
        Instance = this; 
    }

    void Start()
    {
        bookPanel.SetActive(false);
        RefreshUI();
    }

    public void ToggleInventory()
    {
        isOpen = !isOpen;
        bookPanel.SetActive(isOpen); 

        if (isOpen)
        {
            ShowDamagePage(); // always open to damage gpage first
            RefreshUI();
        }
    }

    public void CloseInventory()
    {
        isOpen = false;
        bookPanel.SetActive(false);
    }

    public void ShowDamagePage()
    {
        damagePage.SetActive(true);
        healPage.SetActive(false);
    }

    public void ShowHealPage()
    {
        damagePage.SetActive(false);
        healPage.SetActive(true);
    }

    public void RefreshUI()
    {
        for(int i = 0; i < damageSlots.Count; i++)
        {
            if (i < InventoryManager.Instance.damageInventory.Count)
            {
                damageSlots[i].SetItem(InventoryManager.Instance.damageInventory[i]);
            }
            else
            {
                damageSlots[i].ClearSlot();
            }
        }

        Debug.Log("Rerfreshing healslots. healslot inventory Count: " + InventoryManager.Instance.healInventory.Count);

        for (int i = 0; i < healSlots.Count; i++)
        {
            if (i < InventoryManager.Instance.healInventory.Count)
            {
                healSlots[i].SetItem(InventoryManager.Instance.healInventory[i]);
            }
            else
            {
                healSlots[i].ClearSlot();
            }
        }
    }
}
