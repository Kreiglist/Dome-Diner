/*using UnityEngine;
using UnityEngine.EventSystems;

public class Table : MonoBehaviour, IDropHandler
{
    public Transform seatingPosition; // The position where the object should snap to

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("onDrop");

        if (eventData.pointerDrag != null)
        {
            // Snap the dropped object to the seating position
            eventData.pointerDrag.transform.position = seatingPosition.position;

            // Optionally, set the dropped object as a child of the table
            eventData.pointerDrag.transform.SetParent(this.transform);
        }
    }

    public void SeatCustomerGroup(GameObject customerPrefab)
    {
        // Instantiate the customer at the seating position
        GameObject instantiatedCustomer = Instantiate(customerPrefab, seatingPosition.position, Quaternion.identity);

        // Set the customer as a child of the tabl
        instantiatedCustomer.transform.SetParent(this.transform);

        // Call any customer-specific logic, like seating animations
        Customer customerScript = instantiatedCustomer.GetComponent<Customer>();
        if (customerScript != null)
        {
            customerScript.SitAtTable();
        }
    }
}*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Table : MonoBehaviour, IDropHandler
{

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop");
        if (eventData.pointerDrag != null)
        {
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
        }
    }

}