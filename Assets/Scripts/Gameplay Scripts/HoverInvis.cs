using UnityEngine;
using UnityEngine.EventSystems;

public class HoverInvis : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Renderer objectRenderer;  // For 3D objects (MeshRenderer)
    private SpriteRenderer spriteRenderer;  // For 2D objects (SpriteRenderer)
    private Color originalColor;
    private bool isTransparent = false;

    private void Awake()
    {
        // Try to get the SpriteRenderer first, and then the Renderer for 3D objects
        spriteRenderer = GetComponent<SpriteRenderer>();
        objectRenderer = GetComponent<Renderer>();

        // Store the original color of the object
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;  // Store original color for SpriteRenderer
        }
        else if (objectRenderer != null)
        {
            originalColor = objectRenderer.material.color;  // Store original color for MeshRenderer
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Make the object fully transparent
        if (spriteRenderer != null && !isTransparent)
        {
            // Change the alpha value to 0 (fully transparent) for SpriteRenderer
            Color transparentColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
            spriteRenderer.color = transparentColor;
            isTransparent = true;  // Track that the object is now transparent
        }
        else if (objectRenderer != null && !isTransparent)
        {
            // Change the alpha value to 0 (fully transparent) for MeshRenderer
            Color transparentColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
            objectRenderer.material.color = transparentColor;
            isTransparent = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Restore the object's original transparency
        if (spriteRenderer != null && isTransparent)
        {
            // Restore the original color for SpriteRenderer
            spriteRenderer.color = originalColor;
            isTransparent = false;  // Track that the object is no longer transparent
        }
        else if (objectRenderer != null && isTransparent)
        {
            // Restore the original color for MeshRenderer
            objectRenderer.material.color = originalColor;
            isTransparent = false;
        }
    }
}
