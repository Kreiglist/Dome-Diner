using UnityEngine;
using UnityEngine.UI; // For handling UI elements
using TMPro; // For TextMeshProUGUI
using System.Collections.Generic;

public class PlayerInventory : MonoBehaviour
{
    public Image[] itemSlotsUI; // UI images representing the two item slots
    public Sprite emptySlotSprite; // Sprite to represent an empty slot
    public Sprite orderPaperSprite; // Sprite for order paper
    public Sprite foodSprite; // Sprite for food
    public Sprite emptyPlateSprite; // Sprite for empty plate
    public TextMeshProUGUI[] itemSlotTexts; // TextMeshPro components to display table IDs on each slot

    private string[] items = new string[2]; // Internal inventory: "OrderPaper:<TableID>", "Food:<TableID>", or "EmptyPlate:<TableID>"

    private void Start()
    {
        // Initialize the inventory UI to empty
        UpdateInventoryUI();
    }

    /// <summary>
    /// Adds an order to the inventory with the given table ID.
    /// </summary>
    public bool AddOrder(int tableID)
    {
        string item = $"OrderPaper:{tableID}";
        return AddItem(item, orderPaperSprite, tableID); // Pass `tableID` as an int
    }

    /// <summary>
    /// Adds food to the inventory with the given table ID.
    /// </summary>
    public bool AddFood(int tableID)
    {
        string item = $"Food:{tableID}";
        return AddItem(item, foodSprite, tableID); // Pass `tableID` as an int
    }

    /// <summary>
    /// Adds an empty plate to the inventory with the given table ID.
    /// </summary>
    public bool AddEmptyPlate(int tableID)
    {
        string item = $"EmptyPlate:{tableID}";
        return AddItem(item, emptyPlateSprite, tableID); // Pass `tableID` as an int
    }

    /// <summary>
    /// Adds an item to the inventory.
    /// </summary>
    public bool AddItem(string item, Sprite itemSprite, int tableID)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (string.IsNullOrEmpty(items[i]))
            {
                items[i] = item; // Add item to internal inventory
                itemSlotsUI[i].sprite = itemSprite; // Set the sprite
                if (itemSlotTexts[i] != null)
                {
                    itemSlotTexts[i].text = tableID.ToString(); // Display the table ID as a string
                }
                return true; // Successfully added item
            }
        }

        Debug.LogWarning("Inventory is full!");
        return false; // Inventory is full
    }

    /// <summary>
    /// Removes an item from the specified slot.
    /// </summary>
    public void RemoveItem(int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < items.Length && !string.IsNullOrEmpty(items[slotIndex]))
        {
            Debug.Log($"Removed {items[slotIndex]} from slot {slotIndex}");
            items[slotIndex] = null; // Clear the item
            itemSlotsUI[slotIndex].sprite = emptySlotSprite; // Reset to empty slot sprite
            if (itemSlotTexts[slotIndex] != null)
            {
                itemSlotTexts[slotIndex].text = ""; // Clear the text
            }
        }
        else
        {
            Debug.LogWarning("Cannot remove item: Slot is empty or invalid.");
        }
    }

    /// <summary>
    /// Gets the item from the specified slot.
    /// </summary>
    public string GetItem(int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < items.Length)
        {
            return items[slotIndex]; // Return the item string
        }

        return null;
    }

    /// <summary>
    /// Updates the inventory UI, setting empty slots and clearing text.
    /// </summary>
    private void UpdateInventoryUI()
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (string.IsNullOrEmpty(items[i]))
            {
                itemSlotsUI[i].sprite = emptySlotSprite; // Set slot to empty sprite
                if (itemSlotTexts[i] != null)
                {
                    itemSlotTexts[i].text = ""; // Clear text
                }
            }
        }
    }

    /// <summary>
    /// Checks if the inventory contains an item.
    /// </summary>
    public bool HasItem(string itemToCheck)
    {
        foreach (string item in items)
        {
            if (!string.IsNullOrEmpty(item) && item.Contains(itemToCheck))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Finds the slot index of the specified item.
    /// </summary>
    public int FindItemSlot(string itemToCheck)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (!string.IsNullOrEmpty(items[i]) && items[i].Contains(itemToCheck))
            {
                return i;
            }
        }
        return -1; // Item not found
    }
    public bool RemoveItemByType(string itemType)
{
    for (int i = 0; i < items.Length; i++)
    {
        if (items[i] != null && items[i].Contains(itemType))
        {
            items[i] = null; // Clear the item
            itemSlotsUI[i].sprite = emptySlotSprite; // Update UI to show empty slot
            itemSlotTexts[i].text = ""; // Clear associated text
            return true; // Successfully removed
        }
    }
    return false; // No matching item found
}

}
