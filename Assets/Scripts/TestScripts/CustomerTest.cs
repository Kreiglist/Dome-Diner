using UnityEngine;
using System.Collections;
public class CustomerTest : MonoBehaviour
{
    public GameObject customer;
    private DragAndDrop dragAndDrop;
    private bool isCoroutineRunning = false;
    private void Start()
    {
        if (customer != null)
        {
            dragAndDrop = customer.GetComponent<DragAndDrop>();
            if (dragAndDrop == null)
            {
                Debug.LogError("DragAndDrop component not found on customer GameObject!");
            }
        }
        else
        {
            Debug.LogError("Customer GameObject is not assigned!");
        }
    }
    private void Update()
    {
        // Check if the coroutine needs to start
        if (dragAndDrop != null && dragAndDrop.Lagi_Duduk && !isCoroutineRunning)
        {
            StartCoroutine(CustomerBehaviour());
        }
    }
    private IEnumerator CustomerBehaviour()
    {
        isCoroutineRunning = true;
        while (dragAndDrop.Lagi_Duduk)
        {
            Debug.Log("Customer");
            yield return new WaitForSeconds(1f);
        }
        isCoroutineRunning = false;
    }
}