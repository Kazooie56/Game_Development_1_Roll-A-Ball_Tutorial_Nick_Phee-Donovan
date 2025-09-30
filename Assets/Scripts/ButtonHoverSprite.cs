using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHoverSprite : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image hoverImage; // drag your Hierarchy image here
    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();
        hoverImage.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (button != null && button.spriteState.highlightedSprite != null)
        {
            hoverImage.sprite = button.spriteState.highlightedSprite;
        }
        hoverImage.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hoverImage.gameObject.SetActive(false);
    }
}