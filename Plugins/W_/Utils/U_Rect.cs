using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class U_Rect : MonoBehaviour
{

    public static Vector4 GetCorner(RectTransform rectTransform)
    {
        return GetCorner(rectTransform.rect);
    }

    public static Vector4 GetCorner(Rect rect)
    {
        Vector4 corner = new Vector4(rect.yMax,rect.yMin,rect.xMin,rect.xMax);
        return corner;
    }

    public static Vector2 GetArea(RectTransform rectTransform)
    {
        return GetArea(rectTransform.rect);
    }
    public static Vector2 GetArea(Rect rect)
    {
        Vector2 area = new Vector2(rect.width, rect.height);
        return area;
    }
}
