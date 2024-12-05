using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Chef : MonoBehaviour
{
    public Counter counter; // Reference to the Counter script
    public int maxOrders = 2; // Maximum number of orders the chef can take
    private Queue<int> orderQueue = new Queue<int>(); // Queue of table IDs for orders
    private bool isCooking = false; // Whether the chef is currently cooking
    public Animator chefAnimator; // Reference to the Animator component

    /// <summary>
    /// Adds an order to the chef's queue if not full.
    /// </summary>
    public void ReceiveOrder(int tableID)
    {
        if (orderQueue.Count < maxOrders)
        {
            orderQueue.Enqueue(tableID);
            Debug.Log($"Order for Table {tableID} added to the queue. Current queue: {orderQueue.Count}");

            // Start processing orders if not already cooking
            if (!isCooking)
            {
                StartCoroutine(ProcessOrder());
            }
        }
        else
        {
            Debug.LogWarning($"Chef cannot take more orders. Queue is full! Max orders: {maxOrders}");
        }
    }

    /// <summary>
    /// Processes the order queue sequentially, cooking each order.
    /// </summary>
    private IEnumerator ProcessOrder()
    {
        isCooking = true;
        UpdateChefAnimation();

        while (orderQueue.Count > 0)
        {
            int currentOrder = orderQueue.Dequeue();
            Debug.Log($"Cooking food for Table {currentOrder}...");

            // Simulate cooking time
            yield return new WaitForSeconds(5f);

            // Attempt to place food at the counter
            if (counter != null)
            {
                if (counter.SpawnFood(currentOrder))
                {
                    Debug.Log($"Food for Table {currentOrder} is ready at the counter.");
                }
                else
                {
                    Debug.LogWarning("No available spawn point on the counter!");
                }
            }

            yield return null;
        }

        isCooking = false;
        UpdateChefAnimation();
        Debug.Log("Chef has finished all orders.");
    }

    /// <summary>
    /// Updates the chef's animation based on the isCooking state.
    /// </summary>
    private void UpdateChefAnimation()
    {
        if (chefAnimator != null)
        {
            chefAnimator.SetBool("isCooking", isCooking);
        }
        else
        {
            Debug.LogWarning("Animator is not assigned to the Chef script.");
        }
    }
}
