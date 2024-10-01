using UnityEngine;

public class Customer : MonoBehaviour
{
    public bool IsServed = false;  // This will track whether the customer has been served

    // Method to mark the customer as served
    public void ServeCustomer()
    {
        IsServed = true;
        // Add any additional logic like animations, score increase, etc.
    }
}
