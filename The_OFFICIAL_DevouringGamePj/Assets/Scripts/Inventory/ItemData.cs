using System.Runtime.CompilerServices;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public string description;
    public PickupType itemType;
    public int value;
}

public enum  PickupType { Damage, Heal }
