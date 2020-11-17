
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DragTarget : MonoBehaviour
{
    public Texture2D[] _cursor;
    Ray clickRay;

    RaycastHit clickPoint;
    RaycastHit posPoint;
    LayerMask mask = 1 << 9;

    void Start()
    {
    }

    void Update()
    {
        if(Input.GetMouseButton(0))
            Cursor.SetCursor(_cursor[0], Vector2.zero, CursorMode.Auto);
        else
            Cursor.SetCursor(_cursor[1], Vector2.zero, CursorMode.Auto);

        clickRay = Camera.main.ScreenPointToRay(Input.mousePosition);
    }
    void OnMouseDown()
    {
        //如果射线与物体相碰，则调用OnMouseDrag()
        if (Physics.Raycast(clickRay, out clickPoint))
        {
            OnMouseDrag();
        }
    }
    void OnMouseDrag()
    {
        Physics.Raycast(clickRay, out posPoint, Mathf.Infinity, mask.value);
        Vector3 mouseMove = posPoint.point;
        transform.position = (new Vector3(mouseMove.x, transform.position.y, mouseMove.z));
        return;
    }

}

