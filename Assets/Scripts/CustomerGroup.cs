
using UnityEngine;
using System.Collections.Generic;

public class CustomerGroup : MonoBehaviour
{
    public List<GameObject> customers = new List<GameObject>();

    public void SitGroupAtTable(Table table)
    {
        foreach (GameObject customer in customers)
        {
            Customer customerScript = customer.GetComponent<Customer>();
            if (customerScript != null)
            {
                customerScript.SitAtTable();  // Each customer in the group sits at the table
            }
        }
    }
}
