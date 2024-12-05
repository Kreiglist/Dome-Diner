using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HoverTransparency : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Image buttonImage;

    private void Awake()
    {
        buttonImage = GetComponent<Image>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Make the button fully transparent
        if (buttonImage != null)
        {
            buttonImage.color = new Color(buttonImage.color.r, buttonImage.color.g, buttonImage.color.b, 0f);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Restore the button's original transparency
        if (buttonImage != null)
        {
            buttonImage.color = new Color(buttonImage.color.r, buttonImage.color.g, buttonImage.color.b, 1f);
        }
    }
}
