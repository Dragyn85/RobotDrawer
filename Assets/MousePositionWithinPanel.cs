using UnityEngine;
using UnityEngine.EventSystems;

public class MousePositionWithinPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private bool isMouseOver = false;
    private RectTransform rectTransform;
    

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        
    }

    public Vector2 GetNormalizedPosition(Vector3 mousePosition)
    {
        Vector2 localMousePosition;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, mousePosition, null, out localMousePosition))
        {
            var rect = rectTransform.rect;
            float normalizedX = (localMousePosition.x - rect.xMin) / rect.width;
            float normalizedY = (localMousePosition.y - rect.yMin) / rect.height;
            return new Vector2(normalizedX, normalizedY);
        }
        return Vector2.zero; // Default return value in case of failure
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isMouseOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseOver = false;
    }
}