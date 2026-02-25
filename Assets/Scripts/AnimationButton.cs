using UnityEngine;
using UnityEngine.EventSystems;
public class AnimationButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler , IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]private float buttonScale = 1.2f;
    private float pressScale = 0.9f;
    private bool isHovering ;

    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
        transform.localScale = originalScale * buttonScale;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
        transform.localScale = originalScale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.localScale = originalScale * pressScale;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        transform.localScale = isHovering ? originalScale * buttonScale : originalScale;
    }

}
