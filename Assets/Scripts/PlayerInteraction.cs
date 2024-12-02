using UnityEngine;
using System.Collections;
public class PlayerInteraction : MonoBehaviour
{
    public static bool IsTableClicked(Table table)
    {
        if (table != null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
                Clickable clickable = table.GetComponent<Clickable>();

                if (playerMovement != null && clickable != null)
                {
                    playerMovement.QueueMovement(clickable.associatedNode);
                    StartPlayerInteraction(playerMovement, table);
                    return true;
                }
            }
        }

        Debug.LogWarning("No valid table or associated node found.");
        return false;
    }

    public static void StartPlayerInteraction(PlayerMovement playerMovement, Table table)
    {
        playerMovement.StartCoroutine(WaitForPlayerToReachNode(playerMovement, table));
    }

    private static IEnumerator WaitForPlayerToReachNode(PlayerMovement playerMovement, Table table)
    {
        while (playerMovement.IsMoving())
        {
            yield return null;
        }

        Debug.Log($"Player reached Table {table.tableID}. Taking order...");
        table.OnPlayerInteraction();
    }

    public static bool FoodDelivered(Table table, int tableID)
    {
        Debug.Log($"Food delivered to Table {tableID}");
        return true; // Stubbed functionality
    }

    public static bool MoneyCollected(Table table)
    {
        Debug.Log($"Money collected from Table {table.tableID}");
        return true; // Stubbed functionality
    }
}
