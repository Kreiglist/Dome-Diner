using UnityEngine;

public class CustomerGroup : MonoBehaviour
{
    public GameObject[] customers;  // Array to hold the individual customers in the group

    public void SeatAtTable(Transform[] seatingPositions)
    {
        for (int i = 0; i < customers.Length; i++)
        {
            customers[i].transform.position = seatingPositions[i].position;  // Seat each customer at their designated position
        }
    }

    public bool IsGroupServed()
    {
        // Logic to check if all customers in the group have been served
        foreach (GameObject customer in customers)
        {
            if (!customer.GetComponent<Customer>().IsServed)
                return false;
        }
        return true;
    }
}
