using UnityEngine;

public class GoRight : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed at which the object moves

    void Update()
    {
        // Move the object to the right over time
        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
    }
}

