using UnityEngine;
using System.Collections;

public class PlayerInteraction : MonoBehaviour
{
    public static void StartPlayerInteraction(Table table)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
            Clickable clickable = table.GetComponent<Clickable>();

            if (playerMovement != null && clickable != null)
            {
                playerMovement.QueueMovement(clickable.associatedNode);
                playerMovement.StartCoroutine(WaitForPlayerToReachNode(playerMovement, table));
            }
        }
    }

    private static IEnumerator WaitForPlayerToReachNode(PlayerMovement playerMovement, Table table)
    {
        while (playerMovement.IsMoving())
        {
            yield return null;
        }

        Debug.Log($"Player reached Table {table.tableID}. Interacting...");
    }

    public static bool FoodDelivered(Table table, int tableID)
    {
        Debug.Log($"Food delivered to Table {tableID}");
        return true;
    }

    public static bool MoneyCollected(Table table)
    {
        Debug.Log($"Money collected from Table {table.tableID}");
        return true;
    }
}
