using UnityEngine;

public class CheckTableCollision : MonoBehaviour
{
  private Collider2D targetCollider;
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collided object has the tag "Table"
        if (other.CompareTag("Table"))
        {
            Table table = other.GetComponent<Table>(); // Correctly reference `other` instead of `collider`
            if (table != null)
            {
                table.ProcessInteraction(); // Trigger the interaction on the Table script
                Debug.Log($"Player reached and interacting with Table {table.tableID}");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Check if the object leaving the collider has the tag "Table"
        if (other.CompareTag("Table"))
        {
            Debug.Log($"Exited collision with a table: {other.gameObject.name}");
        }
    }
}