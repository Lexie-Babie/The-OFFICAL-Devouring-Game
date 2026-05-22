using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class InventorySlots : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image itemIcon;
    private ItemData currentItem;
    public void SetItem(ItemData item)
    {
        currentItem = item;
        
        if (itemIcon == null) //safty check to prevent null reference exception, warns if ItemIcon is not assigned in the inspector
        {
            Debug.LogWarning("ItemIcon is not assigned on " + gameObject.name);
            return;
        }

        if (item != null)
        {
            Debug.Log(item.itemName + " Icon is: " + item.icon);
            
            if (item.icon != null) // safty check that warns if the item itself has no sprite assigned 
            {
                itemIcon.sprite = item.icon;
            }
            else
            {
                itemIcon.sprite = null;
                Debug.LogWarning(item.itemName + " has no icon assigned.");
            }
            itemIcon.enabled = true; 
        }
        else
        {
            itemIcon.sprite = null;
            itemIcon.enabled = false;
        }     
    }

    public void ClearSlot()
    {
        SetItem(null);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (currentItem == null)
        {
            return;
        }
        
        string tooltipContent = 
            "<b>" + currentItem.itemName + "</b>\n" + currentItem.description + "\n" + "Value: " + currentItem.value;

        TooltipUI.Instance.ShowTooltip(tooltipContent);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipUI.Instance.HideTooltip();
    }
}
