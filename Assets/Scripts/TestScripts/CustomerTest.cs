using UnityEngine;
using System.Collections;
public class CustomerTest : MonoBehaviour
{
/*    public SpriteRenderer eatingGIF;      // GIF or sprite for eating animation
    public SpriteRenderer readingMenuGIF; // GIF or sprite for reading menu animation*/

    public float readMenuTime = 5f;
    public float eatingTime = 10f;
    public float askBillTime = 3f;

    private void Start()
    {
        // Start with standing model
        /*eatingGIF.gameObject.SetActive(false);
        readingMenuGIF.gameObject.SetActive(false);*/
    }

    // Called when customer is seated at the table
    public void SitAtTable()
    {
        Debug.Log("Customer is seated.");
        StartCustomerSequence();
    }

    // Start the sequence of actions: reading menu, eating, asking for bill
    private void StartCustomerSequence()
    {
        StartCoroutine(CustomerSequence());
    }

    private IEnumerator CustomerSequence()
    {
        // Step 1: Read the Menu
        /*readingMenuGIF.gameObject.SetActive(true);  // Show reading menu GIF*/
        Debug.Log("Customer is reading the menu.");
        yield return new WaitForSeconds(readMenuTime);
       /* readingMenuGIF.gameObject.SetActive(false);  // Hide reading menu GIF*/

        // Step 2: Eating
        /*eatingGIF.gameObject.SetActive(true);  // Show eating GIF*/
        Debug.Log("Customer is eating.");
        yield return new WaitForSeconds(eatingTime);
        /*eatingGIF.gameObject.SetActive(false);  // Hide eating GIF*/

        // Step 3: Ask for Bill
        Debug.Log("Customer is asking for the bill.");
        yield return new WaitForSeconds(askBillTime);

        // Step 4: Leave (could trigger an exit animation, destroy object, etc.)
        LeaveTable();
    }

    // Handles leaving the table after eating
    public void LeaveTable()
    {
        Debug.Log("Customer is leaving.");
        // You can destroy the customer object or trigger an exit animation here
        Destroy(this.gameObject);
    }
}