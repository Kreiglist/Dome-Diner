using UnityEngine;
using System.Collections;
public class Chair : MonoBehaviour
{
    public DragAndDrop dragAndDrop;
    public GameObject chair;
    public GameObject oppositeChair;
    private void OnTriggerEnter2D(Collider2D other)
    {
        dragAndDrop = other.GetComponentInParent<DragAndDrop>();

        if (dragAndDrop != null)
        {
            Debug.Log("DragAndDrop component found!");
            StartCoroutine(CheckLagiDuduk());
        }
        else
        {
            Debug.Log("DragAndDrop component not found on " + other.gameObject.name);
        }
    }
    private IEnumerator CheckLagiDuduk()
    {
        while (true)
        {
            if (dragAndDrop.Lagi_Duduk)
            {
                chair.GetComponent<Renderer>().enabled = false;
                chair.GetComponent<Collider2D>().enabled = false;

                Debug.Log("Parent is sitting on the chair.");

                MoveToPosition(dragAndDrop.gameObject, chair.transform.position);

                foreach (Transform child in dragAndDrop.transform)
                {
                    MoveToPosition(child.gameObject, oppositeChair.transform.position);
                    PlaySittingAnimation(child.gameObject);
                }
            }
            else
            {
                chair.GetComponent<Renderer>().enabled = true;
                chair.GetComponent<Collider2D>().enabled = true;

                Debug.Log("No one is sitting.");
            }

            yield return new WaitForSeconds(0.5f);
        }
    }
    private void MoveToPosition(GameObject obj, Vector3 targetPosition)
    {
        obj.transform.position = targetPosition;
    }
    private void PlaySittingAnimation(GameObject obj)
    {
        Animator animator = obj.GetComponent<Animator>();
        if (animator != null)
        {
            animator.Play("sit");
        }
    }
}