using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    [Header("Drag And Drop Settings")]
    [SerializeField] Vector2 size;
    [SerializeField] LayerMask layerMask;

    [Header("Customer Animation Manager")]
    [SerializeField] Animator anim;

    Sprite sprite_biasa;
    SpriteRenderer sr;
    Vector3 initial_position;
    public bool Lagi_Duduk = false;
    private Collider2D currentCollider;
    public Collider2D coll;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        sprite_biasa = sr.sprite;
    }

    public void OnMouseDown()
    {
        if (Lagi_Duduk) return;
        initial_position = transform.position;
    }

    private void OnMouseDrag()
    {
        if (Lagi_Duduk) return;
        transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseUp()
    {
        if (Lagi_Duduk) return;
        Vector3 new_position;
        coll = Physics2D.OverlapBox(transform.position, size, 0, layerMask);

        if (coll) // kalo semisal di kursi
        {
            new_position = coll.transform.position;
            transform.localScale = new Vector3(coll.transform.localScale.x * -1, 1, 1);
            anim.Play("sit");
            Lagi_Duduk = true;
        }
        else
        {
            new_position = initial_position;
            sr.sprite = sprite_biasa;
        }

        transform.position = new_position;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, size);
    }
}
