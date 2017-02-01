using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Ruler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    public GameObject FirstPoint;
    public GameObject SecondPoint;

    public float GetUnityDistances()
    {
        return Vector2.Distance(new Vector2(FirstPoint.transform.localPosition.x, 0),
            new Vector2(SecondPoint.transform.localPosition.x, 0));
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
    }
}
