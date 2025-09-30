using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHoverPreview : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public RawImage previewImage;       // Drag your RawImage object from the Hierarchy
    public Texture previewTexture;      // Assign the preview texture for THIS button

    private void Awake()
    {
        if (previewImage != null)
            previewImage.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (previewImage != null)
        {
            previewImage.texture = previewTexture;
            previewImage.gameObject.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (previewImage != null)
            previewImage.gameObject.SetActive(false);
    }
}