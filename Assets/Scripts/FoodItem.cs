using UnityEngine;
using TMPro;

public class FoodItem : MonoBehaviour
{
    public int tableID; // The ID of the table this food is for
    public TextMeshPro tableIDText; // TMP component to display the table number

    public void SetTableID(int id)
    {
        tableID = id;

        // Update the TMP text to display the table ID
        if (tableIDText != null)
        {
            tableIDText.text = $"{id}";
        }
    }

    public int GetTableID()
    {
        return tableID;
    }
}
