using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Table : MonoBehaviour, IDropHandler
{
    public Transform seatingPosition; // The position where the object should snap to

    // Handles the drop event when an object is dragged and dropped onto the table
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop");
        if (eventData.pointerDrag != null)
        {
            // Snap the dragged object to the seating position
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
        }
    }

    // Seats a customer group at the table
    public void SeatCustomerGroup(GameObject customerPrefab)
    {
        if (seatingPosition != null)
        {
            // Instantiate the customer prefab at the seating position
            GameObject instantiatedCustomer = Instantiate(customerPrefab, seatingPosition.position, Quaternion.identity);

            // Set the instantiated customer as a child of the table
            instantiatedCustomer.transform.SetParent(this.transform);

            // Trigger any customer-specific behavior, such as animations
            Customer customerScript = instantiatedCustomer.GetComponent<Customer>();
            if (customerScript != null)
            {
                customerScript.SitAtTable();
            }
        }
    }
}
