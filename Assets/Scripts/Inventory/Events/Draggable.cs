using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private Vector3 offset;

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        Vector3 worldPos = MouseHelper.GetMousePosition();
        worldPos.z = transform.position.z;
        offset = transform.position - worldPos;
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        Vector3 worldPos = MouseHelper.GetMousePosition();
        worldPos.z = transform.position.z;
        transform.position = worldPos + offset;
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
    }
}
