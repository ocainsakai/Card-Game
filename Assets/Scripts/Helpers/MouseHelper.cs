using UnityEngine;

public class MouseHelper
{
    public static Vector2 GetMousePosition()
    {
        return (Vector2)(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }
}