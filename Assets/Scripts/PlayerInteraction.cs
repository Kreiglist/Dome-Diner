using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public static bool OrderTaken(Table table)
    {
        // Logic to check if the player interacts with the table to take an order
        Debug.Log($"Order taken for Table {table.tableID}");
        return true; // For now, always return true
    }

    public static bool FoodDelivered(Table table, int tableID)
    {
        // Logic to check if the player delivers food to the correct table
        Debug.Log($"Food delivered to Table {tableID}");
        return true; // For now, always return true
    }

    public static bool MoneyCollected(Table table)
    {
        // Logic to check if the player collects money from the table
        Debug.Log($"Money collected from Table {table.tableID}");
        return true; // For now, always return true
    }
}
