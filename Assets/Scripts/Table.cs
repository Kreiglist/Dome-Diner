using UnityEngine;

public class Table : MonoBehaviour
{
    public Transform seatingPosition;

    public void SeatCustomerGroup(GameObject customerPrefab)
    {
        GameObject instantiatedCustomer = Instantiate(customerPrefab, seatingPosition.position, Quaternion.identity);
        instantiatedCustomer.transform.SetParent(this.transform);

        Customer customerScript = instantiatedCustomer.GetComponent<Customer>();
        if (customerScript != null)
        {
            customerScript.SitAtTable();
        }
    }
}
